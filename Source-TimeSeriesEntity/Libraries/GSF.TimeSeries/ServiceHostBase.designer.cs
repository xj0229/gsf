﻿//******************************************************************************************************
//  ServiceHostBase.designer.cs - Gbtc
//
//  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
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
//  08/20/2010 - J. Ritchie Carroll
//       Generated original version of source code.
//  12/20/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

namespace GSF.TimeSeries
{
    public partial class ServiceHostBase
    {
        #region [ Service Binding ]

        internal void StartDebugging(string[] args)
        {
            OnStart(args);
        }

        internal void StopDebugging()
        {
            OnStop();
        }

        /// <summary>
        /// Handles service start event.
        /// </summary>
        /// <param name="args">Service startup arguments, if any.</param>
        protected override void OnStart(string[] args)
        {
            m_serviceHelper.OnStart(args);
        }

        /// <summary>
        /// Handles service stop event.
        /// </summary>
        protected override void OnStop()
        {
            m_serviceHelper.OnStop();
        }

        /// <summary>
        /// Handles service pause event.
        /// </summary>
        protected override void OnPause()
        {
            m_serviceHelper.OnPause();
        }

        /// <summary>
        /// Handles service continue event.
        /// </summary>
        protected override void OnContinue()
        {
            m_serviceHelper.OnResume();
        }

        /// <summary>
        /// Handles service shut down event.
        /// </summary>
        protected override void OnShutdown()
        {
            m_serviceHelper.OnShutdown();
        }

        #endregion

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.m_serviceHelper = new GSF.ServiceProcess.ServiceHelper(this.components);
            this.m_remotingServer = new GSF.Communication.TlsServer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.m_serviceHelper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_serviceHelper.ErrorLogger)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_serviceHelper.ErrorLogger.ErrorLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_serviceHelper.ProcessScheduler)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_serviceHelper.StatusLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_remotingServer)).BeginInit();
            // 
            // m_serviceHelper
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.m_serviceHelper.ErrorLogger.ErrorLog.FileName = "ErrorLog.txt";
            this.m_serviceHelper.ErrorLogger.ErrorLog.PersistSettings = true;
            this.m_serviceHelper.ErrorLogger.ErrorLog.SettingsCategory = "ErrorLog";
            this.m_serviceHelper.ErrorLogger.LogToEventLog = false;
            this.m_serviceHelper.ErrorLogger.PersistSettings = true;
            this.m_serviceHelper.ErrorLogger.SmtpServer = "";
            this.m_serviceHelper.ParentService = this;
            this.m_serviceHelper.PersistSettings = true;
            // 
            // 
            // 
            this.m_serviceHelper.ProcessScheduler.PersistSettings = true;
            this.m_serviceHelper.ProcessScheduler.SettingsCategory = "ProcessScheduler";
            this.m_serviceHelper.RemotingServer = this.m_remotingServer;
            // 
            // 
            // 
            this.m_serviceHelper.StatusLog.FileName = "StatusLog.txt";
            this.m_serviceHelper.StatusLog.PersistSettings = true;
            this.m_serviceHelper.StatusLog.SettingsCategory = "StatusLog";
            // 
            // m_remotingServer
            // 
            this.m_remotingServer.CertificateFile = "Internal.cer";
            this.m_remotingServer.ConfigurationString = "Port=8500";
            this.m_remotingServer.IgnoreInvalidCredentials = true;
            this.m_remotingServer.PayloadAware = true;
            this.m_remotingServer.PersistSettings = true;
            this.m_remotingServer.SettingsCategory = "RemotingServer";
            this.m_remotingServer.TrustedCertificatesPath = "Certs\\Remotes";
            // 
            // ServiceHostBase
            // 
            this.ServiceName = "IaonHost";
            ((System.ComponentModel.ISupportInitialize)(this.m_serviceHelper.ErrorLogger.ErrorLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_serviceHelper.ErrorLogger)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_serviceHelper.ProcessScheduler)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_serviceHelper.StatusLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_serviceHelper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_remotingServer)).EndInit();

        }

        #endregion

        private GSF.ServiceProcess.ServiceHelper m_serviceHelper;
        private Communication.TlsServer m_remotingServer;
    }
}