﻿//******************************************************************************************************
//  CommonFunctions.cs - Gbtc
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
//  03/31/2011 - Mehulbhai P Thakkar
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using TVA;
using TVA.Data;

namespace TimeSeriesFramework.UI
{
    /// <summary>
    /// Represents a static class containing common methods.
    /// </summary>
    public static class CommonFunctions
    {
        private static Guid s_currentNodeID;

        /// <summary>
        /// Defines the default settings category for TimeSeriesFramework data connections.
        /// </summary>
        public const string DefaultSettingsCategory = "SystemSettings";

        /// <summary>
        /// Defines the current user name as defined in the Thread.CurrentPrincipal.Identity.
        /// </summary>
        public static readonly string CurrentUser = Thread.CurrentPrincipal.Identity.Name;

        /// <summary>
        /// Sets the current user context for the database.
        /// </summary>
        /// <remarks>
        /// Purpose of this method is to supply current user information from the UI to DELETE trigger for change logging.
        /// This method must be called before any delete operation on the database in order to log who deleted this record.
        /// For SQL server it sets user name into CONTEXT_INFO().
        /// For MySQL server it sets user name into session variable @context.
        /// MS Access is not supported for change logging.
        /// For any other database in the future, such as Oracle, this logic must be extended to support change log in the database.
        /// </remarks>
        /// <param name="database"><see cref="AdoDataConnection"/> used to set user context before any delete operation.</param>
        public static void SetCurrentUserContext(AdoDataConnection database)
        {
            bool createdConnection = false;

            try
            {
                if (!string.IsNullOrEmpty(CurrentUser))
                {
                    if (database == null)
                    {
                        database = new AdoDataConnection(DefaultSettingsCategory);
                        createdConnection = true;
                    }

                    IDbCommand command;
                    string connectionType = database.Connection.GetType().Name.ToLower();

                    // Set Current User for the database session for this connection.

                    switch (connectionType)
                    {
                        case "sqlconnection":
                            string contextSql = "DECLARE @context VARBINARY(128)\n SELECT @context = CONVERT(VARBINARY(128), CONVERT(VARCHAR(128), @userName))\n SET CONTEXT_INFO @context";
                            command = database.Connection.CreateCommand();
                            command.CommandText = contextSql;
                            command.AddParameterWithValue("@userName", CurrentUser);
                            command.ExecuteNonQuery();
                            break;
                        case "mysqlconnection":
                            command = database.Connection.CreateCommand();
                            command.CommandText = "SET @context = '" + CurrentUser + "';";
                            command.ExecuteNonQuery();
                            break;
                        default:
                            break;
                    }
                }
            }
            finally
            {
                if (createdConnection && database != null)
                    database.Dispose();
            }
        }

        /// <summary>
        /// Method to check if source database is Microsoft Access.
        /// </summary>
        /// <param name="database"><see cref="AdoDataConnection"/> to database.</param>
        /// <returns>Boolean, indicating if source database is Microsoft Access.</returns>
        public static bool IsJetEngine(this AdoDataConnection database)
        {
            // TODO: Make this a cached property of AdoDataConnection as an optimization...
            return database.Connection.ConnectionString.Contains("Microsoft.Jet.OLEDB");
        }

        /// <summary>
        /// Method to check if source database is MySQL.
        /// </summary>
        /// <param name="database"><see cref="AdoDataConnection"/> to database.</param>
        /// <returns>Boolean, indicating if source database is MySQL.</returns>
        public static bool IsMySQL(this AdoDataConnection database)
        {
            return database.AdapterType.Name == "MySqlDataAdapter";
        }

        /// <summary>
        /// Assigns <see cref="CurrentNodeID"/> based ID of currently active node.
        /// </summary>
        /// <param name="nodeID">Current node ID <see cref="CurrentNodeID"/> to assign.</param>
        public static void SetAsCurrentNodeID(this Guid nodeID)
        {
            s_currentNodeID = nodeID;
        }

        /// <summary>
        /// Returns current node id <see cref="System.Guid"/> UI is connected to.
        /// </summary>
        /// <param name="database">Connected <see cref="AdoDataConnection"/></param>
        /// <returns>Proper <see cref="System.Guid"/> implementation for current node id.</returns>
        public static object CurrentNodeID(this AdoDataConnection database)
        {
            if (s_currentNodeID == null)
                return database.Guid(System.Guid.Empty);

            return database.Guid(s_currentNodeID);
        }

        /// <summary>
        /// Returns proper <see cref="System.Guid"/> implementation for connected <see cref="AdoDataConnection"/> database type.
        /// </summary>
        /// <param name="database">Connected <see cref="AdoDataConnection"/>.</param>
        /// <param name="guid"><see cref="System.Guid"/> to format per database type.</param>
        /// <returns>Proper <see cref="System.Guid"/> implementation for connected <see cref="AdoDataConnection"/> database type.</returns>
        public static object Guid(this AdoDataConnection database, Guid guid)
        {
            if (database.IsJetEngine())
                return "P" + guid.ToString();

            return guid;
        }

        /// <summary>
        /// Retrieves <see cref="System.Guid"/> based on database type.
        /// </summary>
        /// <param name="database">Connected <see cref="AdoDataConnection"/>.</param>
        /// <param name="row"><see cref="DataRow"/> from which value needs to be retrieved.</param>
        /// <param name="fieldName">Name of the field which contains <see cref="System.Guid"/>.</param>
        /// <returns></returns>
        public static Guid Guid(this AdoDataConnection database, DataRow row, string fieldName)
        {
            if (database.IsJetEngine() || database.IsMySQL())
                return System.Guid.Parse(row.Field<object>(fieldName).ToString());

            return row.Field<Guid>(fieldName);
        }

        /// <summary>
        /// Returns current UTC time in implementation that is proper for connected <see cref="AdoDataConnection"/> database type.
        /// </summary>
        /// <param name="database">Connected <see cref="AdoDataConnection"/>.</param>
        /// <param name="usePrecisionTime">Set to <c>true</c> to use precision time.</param>
        /// <returns>Current UTC time in implementation that is proper for connected <see cref="AdoDataConnection"/> database type.</returns>
        public static object UtcNow(this AdoDataConnection database, bool usePrecisionTime = false)
        {
            if (usePrecisionTime)
            {
                if (database.IsJetEngine())
                    return PrecisionTimer.UtcNow.ToOADate();

                return PrecisionTimer.UtcNow;
            }

            if (database.IsJetEngine())
                return DateTime.UtcNow.ToOADate();

            return DateTime.UtcNow;
        }

        /// <summary>
        /// Returns <see cref="DBNull"/> if given <paramref name="value"/> is <c>null</c>.
        /// </summary>
        /// <param name="value">Value to test for null.</param>
        /// <returns><see cref="DBNull"/> if <paramref name="value"/> is <c>null</c>; otherwise <paramref name="value"/>.</returns>
        public static object ToNotNull(this object value)
        {
            return value ?? (object)DBNull.Value;
        }

        /// <summary>
        /// Returns a collection of down sampling methods.
        /// </summary>
        /// <returns><see cref="Dictionary{T1,T2}"/> type collection of down sampling methods.</returns>
        public static Dictionary<string, string> GetDownsamplingMethodLookupList()
        {
            Dictionary<string, string> downsamplingLookupList = new Dictionary<string, string>();

            downsamplingLookupList.Add("LastReceived", "LastReceived");
            downsamplingLookupList.Add("Closest", "Closest");
            downsamplingLookupList.Add("Filtered", "Filtered");
            downsamplingLookupList.Add("BestQuality", "BestQuality");

            return downsamplingLookupList;
        }

        /// <summary>
        /// Returns a collection of system time zones.
        /// </summary>
        /// <param name="isOptional">Indicates if selection on UI is optional for this collection.</param>
        /// <returns><see cref="Dictionary{T1,T2}"/> type collection of system time zones.</returns>
        public static Dictionary<string, string> GetTimeZones(bool isOptional)
        {
            Dictionary<string, string> timeZonesList = new Dictionary<string, string>();

            if (isOptional)
                timeZonesList.Add("", "Select Time Zone");

            foreach (TimeZoneInfo timeZoneInfo in TimeZoneInfo.GetSystemTimeZones())
            {
                if (!timeZonesList.ContainsKey(timeZoneInfo.StandardName))
                    timeZonesList.Add(timeZoneInfo.Id, timeZoneInfo.DisplayName);
            }

            return timeZonesList;
        }
    }
}
