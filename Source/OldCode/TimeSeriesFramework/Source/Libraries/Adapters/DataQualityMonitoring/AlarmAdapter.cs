﻿//******************************************************************************************************
//  AlarmAdapter.cs - Gbtc
//
//  Copyright © 2010, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  01/31/2012 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using DataQualityMonitoring.Services;
using TimeSeriesFramework;
using TimeSeriesFramework.Adapters;
using TVA;
using TVA.Data;
using System.Text;

namespace DataQualityMonitoring
{
    /// <summary>
    /// Action adapter that generates alarm measurements based on alarm definitions from the database.
    /// </summary>
    [Description("Alarm: Generates alarm events for alarms defined in the database.")]
    public class AlarmAdapter : FacileActionAdapterBase
    {
        #region [ Members ]

        // Constants
        private const string DefaultServiceEndpoints = "http.rest://localhost:5018/alarmservices";
        private const string DefaultServiceSecurityPolicy = "";
        private const int WaitTimeout = 1000;

        // Fields
        private List<Alarm> m_alarms;
        private AlarmService m_alarmService;

        private ConcurrentQueue<IMeasurement> m_measurementQueue;
        private Thread m_processThread;
        private Semaphore m_processSemaphore;
        private long m_eventCount;

        private bool m_supportsTemporalProcessing;
        private bool m_servicePublishMetadata;
        private string m_serviceEndpoints;
        private string m_serviceSecurityPolicy;

        private bool m_disposed;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the flag indicating if this adapter supports temporal processing.
        /// </summary>
        [ConnectionStringParameter,
        Description("Define the flag indicating if this adapter supports temporal processing."),
        DefaultValue(false)]
        public override bool SupportsTemporalProcessing
        {
            get
            {
                return m_supportsTemporalProcessing;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates whether web service
        /// metadata is to made available at all web service endpoints.
        /// </summary>
        [ConnectionStringParameter,
        Description("Define the flag indicating whether the web service metadata is to be made available at all web service endpoints."),
        DefaultValue(true)]
        public bool ServicePublishMetadata
        {
            get
            {
                return m_servicePublishMetadata;
            }
            set
            {
                m_servicePublishMetadata = value;
            }
        }

        /// <summary>
        /// Gets or sets a semicolon delimited list of
        /// URIs where the web service can be accessed.
        /// </summary>
        [ConnectionStringParameter,
        Description("Define a semicolon delimited list of URIs where the web service can be accessed."),
        DefaultValue(DefaultServiceEndpoints)]
        public string ServiceEndpoints
        {
            get
            {
                return m_serviceEndpoints;
            }
            set
            {
                m_serviceEndpoints = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Type.FullName"/> of System.IdentityModel.Policy.IAuthorizationPolicy
        /// to be used for securing all web service <see cref="TVA.ServiceModel.SelfHostingService.Endpoints"/>.
        /// </summary>
        [ConnectionStringParameter,
        Description("Define the full name of the authorization policy to be used for securing all web service endpoints."),
        DefaultValue(DefaultServiceSecurityPolicy)]
        public string ServiceSecurityPolicy
        {
            get
            {
                return m_serviceSecurityPolicy;
            }
            set
            {
                m_serviceSecurityPolicy = value;
            }
        }

        /// <summary>
        /// Returns the detailed status of the data input source.
        /// </summary>
        public override string Status
        {
            get
            {
                StringBuilder statusBuilder = new StringBuilder(base.Status);
                statusBuilder.Append(m_alarmService.Status);
                return statusBuilder.ToString();
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Initializes the <see cref="AlarmAdapter"/>.
        /// </summary>
        public override void Initialize()
        {
            Dictionary<string, string> settings;
            string setting;

            string filterExpression;

            // Run base class initialization
            base.Initialize();
            settings = Settings;

            // Load optional parameters
            if (settings.TryGetValue("supportsTemporalProcessing", out setting))
                m_supportsTemporalProcessing = setting.ParseBoolean();
            else
                m_supportsTemporalProcessing = false;

            if (settings.TryGetValue("servicePublishMetadata", out setting))
                m_servicePublishMetadata = setting.ParseBoolean();
            else
                m_servicePublishMetadata = true;

            if (!settings.TryGetValue("serviceEndpoints", out m_serviceEndpoints))
                m_serviceEndpoints = DefaultServiceEndpoints;

            if (!settings.TryGetValue("serviceSecurityPolicy", out m_serviceSecurityPolicy))
                m_serviceSecurityPolicy = DefaultServiceSecurityPolicy;

            // Create alarms using definitions from the database
            m_alarms = DataSource.Tables["Alarms"].Rows.Cast<DataRow>()
                .Where(row => row.ConvertField<bool>("Enabled"))
                .Select(row => CreateAlarm(row))
                .ToList();

            if (m_alarms.Count > 0)
            {
                // Generate filter expression for input measurements
                filterExpression = m_alarms.Select(a => a.SignalID)
                    .Distinct()
                    .Select(id => id.ToString())
                    .Aggregate((list, id) => list + ";" + id);

                // Set input measurement keys for measurement routing
                InputMeasurementKeys = ParseInputMeasurementKeys(DataSource, filterExpression);
            }

            // Set up alarm service
            m_alarmService = new AlarmService(this);
            m_alarmService.SettingsCategory = base.Name.Replace("!", "") + m_alarmService.SettingsCategory;
            m_alarmService.PublishMetadata = m_servicePublishMetadata;
            m_alarmService.Endpoints = m_serviceEndpoints;
            m_alarmService.SecurityPolicy = m_serviceSecurityPolicy;
            m_alarmService.ServiceProcessException += AlarmService_ServiceProcessException;
            m_alarmService.Initialize();
        }

        /// <summary>
        /// Starts the <see cref="AlarmAdapter"/>, or restarts it if it is already running.
        /// </summary>
        public override void Start()
        {
            base.Start();

            m_measurementQueue = new ConcurrentQueue<IMeasurement>();
            m_processThread = new Thread(ProcessMeasurements);
            m_processSemaphore = new Semaphore(0, int.MaxValue);
            m_eventCount = 0L;

            m_processThread.Start();
        }

        /// <summary>
        /// Stops the <see cref="AlarmAdapter"/>.
        /// </summary>
        public override void Stop()
        {
            base.Stop();

            if ((object)m_processSemaphore != null)
            {
                m_processSemaphore.Dispose();
                m_processSemaphore = null;
            }

            if ((object)m_processThread != null && m_processThread.ThreadState == ThreadState.Running)
            {
                m_processThread.Join();
                m_processThread = null;
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="AlarmAdapter"/> object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                try
                {
                    if (disposing)
                    {
                        if (m_alarmService != null)
                        {
                            m_alarmService.ServiceProcessException -= AlarmService_ServiceProcessException;
                            m_alarmService.Dispose();
                        }
                    }
                }
                finally
                {
                    m_disposed = true;          // Prevent duplicate dispose.
                    base.Dispose(disposing);    // Call base class Dispose().
                }
            }
        }

        /// <summary>
        /// Queues a collection of measurements for processing.
        /// </summary>
        /// <param name="measurements">Measurements to queue for processing.</param>
        public override void QueueMeasurementsForProcessing(IEnumerable<IMeasurement> measurements)
        {
            base.QueueMeasurementsForProcessing(measurements);

            foreach (IMeasurement measurement in measurements)
            {
                m_measurementQueue.Enqueue(measurement);
                m_processSemaphore.Release();
            }
        }

        /// <summary>
        /// Gets a short one-line status of this <see cref="AdapterBase"/>.
        /// </summary>
        /// <param name="maxLength">Maximum number of available characters for display.</param>
        /// <returns>A short one-line summary of the current status of this <see cref="AdapterBase"/>.</returns>
        public override string GetShortStatus(int maxLength)
        {
            return string.Format("{0} events processed since last start", m_eventCount).CenterText(maxLength);
        }

        /// <summary>
        /// Gets a collection containing all the raised alarms in the system.
        /// </summary>
        /// <returns>A collection containing all the raised alarms.</returns>
        public ICollection<Alarm> GetRaisedAlarms()
        {
            lock (m_alarms)
            {
                return m_alarms.Where(alarm => alarm.State == AlarmState.Raised)
                    .ToList();
            }
        }

        // Creates an alarm using data defined in the database.
        private Alarm CreateAlarm(DataRow row)
        {
            object associatedMeasurementId = row.Field<object>("AssociatedMeasurementID");

            return new Alarm()
            {
                ID = row.ConvertField<int>("ID"),
                TagName = row.Field<object>("TagName").ToString(),
                SignalID = Guid.Parse(row.Field<object>("SignalID").ToString()),
                AssociatedMeasurementID = ((object)associatedMeasurementId != null) ? Guid.Parse(associatedMeasurementId.ToString()) : (Guid?)null,
                Description = row.Field<object>("Description").ToString(),
                Severity = row.ConvertField<AlarmSeverity>("Severity"),
                Operation = row.ConvertField<AlarmOperation>("Operation"),
                SetPoint = row.ConvertNullableField<double>("SetPoint"),
                Tolerance = row.ConvertNullableField<double>("Tolerance"),
                Delay = row.ConvertNullableField<double>("Delay"),
                Hysteresis = row.ConvertNullableField<double>("Hysteresis")
            };
        }

        // Processes measurements in the queue.
        private void ProcessMeasurements()
        {
            IMeasurement measurement, alarmEvent;
            List<Alarm> events;

            while (Enabled)
            {
                if ((object)m_processSemaphore != null && m_processSemaphore.WaitOne(WaitTimeout) && m_measurementQueue.TryDequeue(out measurement))
                {
                    lock (m_alarms)
                    {
                        // Get alarms that triggered events
                        events = m_alarms.Where(a => a.SignalID == measurement.ID)
                            .Where(a => a.Test(measurement))
                            .ToList();
                    }

                    // Create event measurements and send them into the system
                    foreach (Alarm alarm in events)
                    {
                        alarmEvent = new Measurement()
                        {
                            Timestamp = measurement.Timestamp,
                            Value = (int)alarm.State
                        };

                        if ((object)alarm.AssociatedMeasurementID != null)
                            alarmEvent.ID = alarm.AssociatedMeasurementID.Value;

                        OnNewMeasurement(alarmEvent);
                        m_eventCount++;
                    }
                }
            }
        }

        // Helper method to raise the NewMeasurements event
        // when only a single measurement is to be provided.
        private void OnNewMeasurement(IMeasurement measurement)
        {
            OnNewMeasurements(new IMeasurement[] { measurement });
        }

        // Processes excpetions thrown by the alarm service.
        private void AlarmService_ServiceProcessException(object sender, EventArgs<Exception> e)
        {
            OnProcessException(e.Argument);
        }

        #endregion
    }
}
