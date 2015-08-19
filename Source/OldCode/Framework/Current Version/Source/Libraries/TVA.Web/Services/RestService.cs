﻿//*******************************************************************************************************
//  RestService.cs - Gbtc
//
//  Tennessee Valley Authority, 2009
//  No copyright is claimed pursuant to 17 USC § 105.  All Other Rights Reserved.
//
//  This software is made freely available under the TVA Open Source Agreement (see below).
//
//  Code Modification History:
//  -----------------------------------------------------------------------------------------------------
//  08/27/2009 - Pinal C. Patel
//       Generated original version of source code.
//  09/02/2009 - Pinal C. Patel
//       Modified configuration of the default WebHttpBinding to enable receiving of large payloads.
//  09/15/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  11/27/2009 - Pinal C. Patel
//       Fixed bug in the initialization of service contract name.
//  03/30/2010 - Pinal C. Patel
//       Updated CanRead and CanWrite to not include Enabled in its evaluation.
//  05/28/2010 - Pinal C. Patel
//       Added an endpoint for web service help.
//  06/21/2010 - Pinal C. Patel
//       Added Singleton property for added control over the hosting process.
//
//*******************************************************************************************************

#region [ TVA Open Source Agreement ]
/*

 THIS OPEN SOURCE AGREEMENT ("AGREEMENT") DEFINES THE RIGHTS OF USE,REPRODUCTION, DISTRIBUTION,
 MODIFICATION AND REDISTRIBUTION OF CERTAIN COMPUTER SOFTWARE ORIGINALLY RELEASED BY THE
 TENNESSEE VALLEY AUTHORITY, A CORPORATE AGENCY AND INSTRUMENTALITY OF THE UNITED STATES GOVERNMENT
 ("GOVERNMENT AGENCY"). GOVERNMENT AGENCY IS AN INTENDED THIRD-PARTY BENEFICIARY OF ALL SUBSEQUENT
 DISTRIBUTIONS OR REDISTRIBUTIONS OF THE SUBJECT SOFTWARE. ANYONE WHO USES, REPRODUCES, DISTRIBUTES,
 MODIFIES OR REDISTRIBUTES THE SUBJECT SOFTWARE, AS DEFINED HEREIN, OR ANY PART THEREOF, IS, BY THAT
 ACTION, ACCEPTING IN FULL THE RESPONSIBILITIES AND OBLIGATIONS CONTAINED IN THIS AGREEMENT.

 Original Software Designation: openPDC
 Original Software Title: The TVA Open Source Phasor Data Concentrator
 User Registration Requested. Please Visit https://naspi.tva.com/Registration/
 Point of Contact for Original Software: J. Ritchie Carroll <mailto:jrcarrol@tva.gov>

 1. DEFINITIONS

 A. "Contributor" means Government Agency, as the developer of the Original Software, and any entity
 that makes a Modification.

 B. "Covered Patents" mean patent claims licensable by a Contributor that are necessarily infringed by
 the use or sale of its Modification alone or when combined with the Subject Software.

 C. "Display" means the showing of a copy of the Subject Software, either directly or by means of an
 image, or any other device.

 D. "Distribution" means conveyance or transfer of the Subject Software, regardless of means, to
 another.

 E. "Larger Work" means computer software that combines Subject Software, or portions thereof, with
 software separate from the Subject Software that is not governed by the terms of this Agreement.

 F. "Modification" means any alteration of, including addition to or deletion from, the substance or
 structure of either the Original Software or Subject Software, and includes derivative works, as that
 term is defined in the Copyright Statute, 17 USC § 101. However, the act of including Subject Software
 as part of a Larger Work does not in and of itself constitute a Modification.

 G. "Original Software" means the computer software first released under this Agreement by Government
 Agency entitled openPDC, including source code, object code and accompanying documentation, if any.

 H. "Recipient" means anyone who acquires the Subject Software under this Agreement, including all
 Contributors.

 I. "Redistribution" means Distribution of the Subject Software after a Modification has been made.

 J. "Reproduction" means the making of a counterpart, image or copy of the Subject Software.

 K. "Sale" means the exchange of the Subject Software for money or equivalent value.

 L. "Subject Software" means the Original Software, Modifications, or any respective parts thereof.

 M. "Use" means the application or employment of the Subject Software for any purpose.

 2. GRANT OF RIGHTS

 A. Under Non-Patent Rights: Subject to the terms and conditions of this Agreement, each Contributor,
 with respect to its own contribution to the Subject Software, hereby grants to each Recipient a
 non-exclusive, world-wide, royalty-free license to engage in the following activities pertaining to
 the Subject Software:

 1. Use

 2. Distribution

 3. Reproduction

 4. Modification

 5. Redistribution

 6. Display

 B. Under Patent Rights: Subject to the terms and conditions of this Agreement, each Contributor, with
 respect to its own contribution to the Subject Software, hereby grants to each Recipient under Covered
 Patents a non-exclusive, world-wide, royalty-free license to engage in the following activities
 pertaining to the Subject Software:

 1. Use

 2. Distribution

 3. Reproduction

 4. Sale

 5. Offer for Sale

 C. The rights granted under Paragraph B. also apply to the combination of a Contributor's Modification
 and the Subject Software if, at the time the Modification is added by the Contributor, the addition of
 such Modification causes the combination to be covered by the Covered Patents. It does not apply to
 any other combinations that include a Modification. 

 D. The rights granted in Paragraphs A. and B. allow the Recipient to sublicense those same rights.
 Such sublicense must be under the same terms and conditions of this Agreement.

 3. OBLIGATIONS OF RECIPIENT

 A. Distribution or Redistribution of the Subject Software must be made under this Agreement except for
 additions covered under paragraph 3H. 

 1. Whenever a Recipient distributes or redistributes the Subject Software, a copy of this Agreement
 must be included with each copy of the Subject Software; and

 2. If Recipient distributes or redistributes the Subject Software in any form other than source code,
 Recipient must also make the source code freely available, and must provide with each copy of the
 Subject Software information on how to obtain the source code in a reasonable manner on or through a
 medium customarily used for software exchange.

 B. Each Recipient must ensure that the following copyright notice appears prominently in the Subject
 Software:

          No copyright is claimed pursuant to 17 USC § 105.  All Other Rights Reserved.

 C. Each Contributor must characterize its alteration of the Subject Software as a Modification and
 must identify itself as the originator of its Modification in a manner that reasonably allows
 subsequent Recipients to identify the originator of the Modification. In fulfillment of these
 requirements, Contributor must include a file (e.g., a change log file) that describes the alterations
 made and the date of the alterations, identifies Contributor as originator of the alterations, and
 consents to characterization of the alterations as a Modification, for example, by including a
 statement that the Modification is derived, directly or indirectly, from Original Software provided by
 Government Agency. Once consent is granted, it may not thereafter be revoked.

 D. A Contributor may add its own copyright notice to the Subject Software. Once a copyright notice has
 been added to the Subject Software, a Recipient may not remove it without the express permission of
 the Contributor who added the notice.

 E. A Recipient may not make any representation in the Subject Software or in any promotional,
 advertising or other material that may be construed as an endorsement by Government Agency or by any
 prior Recipient of any product or service provided by Recipient, or that may seek to obtain commercial
 advantage by the fact of Government Agency's or a prior Recipient's participation in this Agreement.

 F. In an effort to track usage and maintain accurate records of the Subject Software, each Recipient,
 upon receipt of the Subject Software, is requested to register with Government Agency by visiting the
 following website: https://naspi.tva.com/Registration/. Recipient's name and personal information
 shall be used for statistical purposes only. Once a Recipient makes a Modification available, it is
 requested that the Recipient inform Government Agency at the web site provided above how to access the
 Modification.

 G. Each Contributor represents that that its Modification does not violate any existing agreements,
 regulations, statutes or rules, and further that Contributor has sufficient rights to grant the rights
 conveyed by this Agreement.

 H. A Recipient may choose to offer, and to charge a fee for, warranty, support, indemnity and/or
 liability obligations to one or more other Recipients of the Subject Software. A Recipient may do so,
 however, only on its own behalf and not on behalf of Government Agency or any other Recipient. Such a
 Recipient must make it absolutely clear that any such warranty, support, indemnity and/or liability
 obligation is offered by that Recipient alone. Further, such Recipient agrees to indemnify Government
 Agency and every other Recipient for any liability incurred by them as a result of warranty, support,
 indemnity and/or liability offered by such Recipient.

 I. A Recipient may create a Larger Work by combining Subject Software with separate software not
 governed by the terms of this agreement and distribute the Larger Work as a single product. In such
 case, the Recipient must make sure Subject Software, or portions thereof, included in the Larger Work
 is subject to this Agreement.

 J. Notwithstanding any provisions contained herein, Recipient is hereby put on notice that export of
 any goods or technical data from the United States may require some form of export license from the
 U.S. Government. Failure to obtain necessary export licenses may result in criminal liability under
 U.S. laws. Government Agency neither represents that a license shall not be required nor that, if
 required, it shall be issued. Nothing granted herein provides any such export license.

 4. DISCLAIMER OF WARRANTIES AND LIABILITIES; WAIVER AND INDEMNIFICATION

 A. No Warranty: THE SUBJECT SOFTWARE IS PROVIDED "AS IS" WITHOUT ANY WARRANTY OF ANY KIND, EITHER
 EXPRESSED, IMPLIED, OR STATUTORY, INCLUDING, BUT NOT LIMITED TO, ANY WARRANTY THAT THE SUBJECT
 SOFTWARE WILL CONFORM TO SPECIFICATIONS, ANY IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
 PARTICULAR PURPOSE, OR FREEDOM FROM INFRINGEMENT, ANY WARRANTY THAT THE SUBJECT SOFTWARE WILL BE ERROR
 FREE, OR ANY WARRANTY THAT DOCUMENTATION, IF PROVIDED, WILL CONFORM TO THE SUBJECT SOFTWARE. THIS
 AGREEMENT DOES NOT, IN ANY MANNER, CONSTITUTE AN ENDORSEMENT BY GOVERNMENT AGENCY OR ANY PRIOR
 RECIPIENT OF ANY RESULTS, RESULTING DESIGNS, HARDWARE, SOFTWARE PRODUCTS OR ANY OTHER APPLICATIONS
 RESULTING FROM USE OF THE SUBJECT SOFTWARE. FURTHER, GOVERNMENT AGENCY DISCLAIMS ALL WARRANTIES AND
 LIABILITIES REGARDING THIRD-PARTY SOFTWARE, IF PRESENT IN THE ORIGINAL SOFTWARE, AND DISTRIBUTES IT
 "AS IS."

 B. Waiver and Indemnity: RECIPIENT AGREES TO WAIVE ANY AND ALL CLAIMS AGAINST GOVERNMENT AGENCY, ITS
 AGENTS, EMPLOYEES, CONTRACTORS AND SUBCONTRACTORS, AS WELL AS ANY PRIOR RECIPIENT. IF RECIPIENT'S USE
 OF THE SUBJECT SOFTWARE RESULTS IN ANY LIABILITIES, DEMANDS, DAMAGES, EXPENSES OR LOSSES ARISING FROM
 SUCH USE, INCLUDING ANY DAMAGES FROM PRODUCTS BASED ON, OR RESULTING FROM, RECIPIENT'S USE OF THE
 SUBJECT SOFTWARE, RECIPIENT SHALL INDEMNIFY AND HOLD HARMLESS  GOVERNMENT AGENCY, ITS AGENTS,
 EMPLOYEES, CONTRACTORS AND SUBCONTRACTORS, AS WELL AS ANY PRIOR RECIPIENT, TO THE EXTENT PERMITTED BY
 LAW.  THE FOREGOING RELEASE AND INDEMNIFICATION SHALL APPLY EVEN IF THE LIABILITIES, DEMANDS, DAMAGES,
 EXPENSES OR LOSSES ARE CAUSED, OCCASIONED, OR CONTRIBUTED TO BY THE NEGLIGENCE, SOLE OR CONCURRENT, OF
 GOVERNMENT AGENCY OR ANY PRIOR RECIPIENT.  RECIPIENT'S SOLE REMEDY FOR ANY SUCH MATTER SHALL BE THE
 IMMEDIATE, UNILATERAL TERMINATION OF THIS AGREEMENT.

 5. GENERAL TERMS

 A. Termination: This Agreement and the rights granted hereunder will terminate automatically if a
 Recipient fails to comply with these terms and conditions, and fails to cure such noncompliance within
 thirty (30) days of becoming aware of such noncompliance. Upon termination, a Recipient agrees to
 immediately cease use and distribution of the Subject Software. All sublicenses to the Subject
 Software properly granted by the breaching Recipient shall survive any such termination of this
 Agreement.

 B. Severability: If any provision of this Agreement is invalid or unenforceable under applicable law,
 it shall not affect the validity or enforceability of the remainder of the terms of this Agreement.

 C. Applicable Law: This Agreement shall be subject to United States federal law only for all purposes,
 including, but not limited to, determining the validity of this Agreement, the meaning of its
 provisions and the rights, obligations and remedies of the parties.

 D. Entire Understanding: This Agreement constitutes the entire understanding and agreement of the
 parties relating to release of the Subject Software and may not be superseded, modified or amended
 except by further written agreement duly executed by the parties.

 E. Binding Authority: By accepting and using the Subject Software under this Agreement, a Recipient
 affirms its authority to bind the Recipient to all terms and conditions of this Agreement and that
 Recipient hereby agrees to all terms and conditions herein.

 F. Point of Contact: Any Recipient contact with Government Agency is to be directed to the designated
 representative as follows: J. Ritchie Carroll <mailto:jrcarrol@tva.gov>.

*/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using TVA.Configuration;
using TVA.Reflection;

namespace TVA.Web.Services
{
    /// <summary>
    /// A base class for web service that can send and receive data over REST (Representational State Transfer) interface.
    /// </summary>
    public class RestService : IRestService
    {
        #region [ Members ]

        // Events

        /// <summary>
        /// Occurs when the <see cref="ServiceHost"/> has been created for the specified <see cref="ServiceUri"/>.
        /// </summary>
        /// <remarks>
        /// When <see cref="ServiceHostCreated"/> event is fired, changes like adding new endpoints can be made to the <see cref="ServiceHost"/>.
        /// </remarks>
        public event EventHandler ServiceHostCreated;

        /// <summary>
        /// Occurs when the <see cref="ServiceHost"/> has can process requests via all of its endpoints.
        /// </summary>
        public event EventHandler ServiceHostStarted;

        /// <summary>
        /// Occurs when an <see cref="Exception"/> is encountered when processing a request.
        /// </summary>
        /// <remarks>
        /// <see cref="EventArgs{T}.Argument"/> is the exception encountered when processing a request.
        /// </remarks>
        public event EventHandler<EventArgs<Exception>> ServiceProcessException;

        // Fields
        private bool m_singleton;
        private string m_serviceUri;
        private string m_serviceContract;
        private DataFlowDirection m_serviceDataFlow;
        private bool m_persistSettings;
        private string m_settingsCategory;
        private bool m_enabled;
        private bool m_disposed;
        private bool m_initialized;
        private WebServiceHost m_serviceHost;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the web service.
        /// </summary>
        protected RestService()
        {
            Type type = this.GetType();
            m_serviceContract = type.Namespace + ".I" + type.Name + ", " + type.AssemblyQualifiedName.Split(',')[1].Trim();
            m_serviceDataFlow = DataFlowDirection.BothWays;
            m_persistSettings = true;
            m_settingsCategory = type.Name;
        }

        /// <summary>
        /// Releases the unmanaged resources before the web service is reclaimed by <see cref="GC"/>.
        /// </summary>
        ~RestService()
        {
            Dispose(false);
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets a boolean value that indicates whether the <see cref="ServiceHost"/> will use the current instance of the web service for processing 
        /// requests or base the web service instance creation on <see cref="InstanceContextMode"/> specified in its <see cref="ServiceBehaviorAttribute"/>.
        /// </summary>
        public bool Singleton
        {
            get
            {
                return m_singleton;
            }
            set
            {
                m_singleton = value;
            }
        }

        /// <summary>
        /// Gets or sets the URI where the web service is to be hosted.
        /// </summary>
        /// <remarks>
        /// Set <see cref="ServiceUri"/> to a null or empty string to disable web service hosting.
        /// </remarks>
        public string ServiceUri
        {
            get
            {
                return m_serviceUri;
            }
            set
            {
                m_serviceUri = value;
            }
        }

        /// <summary>
        /// Gets or sets the contract interface implemented by the web service.
        /// </summary>
        /// <remarks>
        /// This is the <see cref="Type.FullName"/> of the contract interface implemented by the web service.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The value being assigned is a null or empty string.</exception>
        public string ServiceContract
        {
            get
            {
                return m_serviceContract;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("value");

                m_serviceContract = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DataFlowDirection"/> of the web service.
        /// </summary>
        public DataFlowDirection ServiceDataFlow
        {
            get
            {
                return m_serviceDataFlow;
            }
            set
            {
                m_serviceDataFlow = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates whether the web service is currently enabled.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return m_enabled;
            }
            set
            {
                m_enabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates whether the web service settings are to be saved to the config file.
        /// </summary>
        public bool PersistSettings
        {
            get
            {
                return m_persistSettings;
            }
            set
            {
                m_persistSettings = value;
            }
        }

        /// <summary>
        /// Gets or sets the category under which the web service settings are to be saved to the config file if the <see cref="PersistSettings"/> property is set to true.
        /// </summary>
        /// <exception cref="ArgumentNullException">The value being assigned is a null or empty string.</exception>
        public string SettingsCategory
        {
            get
            {
                return m_settingsCategory;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("value");

                m_settingsCategory = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="WebServiceHost"/> hosting the web service.
        /// </summary>
        /// <remarks>
        /// By default, the <see cref="ServiceHost"/> only has <see cref="WebHttpBinding"/> endpoint at the <see cref="ServiceUri"/>. 
        /// Additional endpoints can be added to the <see cref="ServiceHost"/> when <see cref="ServiceHostCreated"/> event is fired.
        /// </remarks>
        public WebServiceHost ServiceHost
        {
            get
            {
                return m_serviceHost;
            }
        }

        /// <summary>
        /// Gets a boolean value that indicates whether data can be requested from the web service.
        /// </summary>
        protected bool CanRead
        {
            get
            {
                if (m_serviceDataFlow == DataFlowDirection.Outgoing ||
                    m_serviceDataFlow == DataFlowDirection.BothWays)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Gets a boolean value that indicates whether data can be published to the web service.
        /// </summary>
        protected bool CanWrite
        {
            get
            {
                if (m_serviceDataFlow == DataFlowDirection.Incoming ||
                    m_serviceDataFlow == DataFlowDirection.BothWays)
                    return true;
                else
                    return false;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Releases all the resources used by the web service.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Initializes the web service.
        /// </summary>
        public virtual void Initialize()
        {
            if (!m_initialized)
            {
                LoadSettings();
                if (m_enabled && !string.IsNullOrEmpty(m_serviceUri))
                {
                    // Initialize host and binding.
                    if (m_singleton)
                        m_serviceHost = new WebServiceHost(this, new Uri(m_serviceUri));
                    else
                        m_serviceHost = new WebServiceHost(this.GetType(), new Uri(m_serviceUri));
                    WebHttpBinding serviceBinding = new WebHttpBinding();

                    // Add an endpoint for the service.
                    ServiceEndpoint serviceEndpoint = m_serviceHost.AddServiceEndpoint(Type.GetType(m_serviceContract), serviceBinding, "");
                    serviceEndpoint.Behaviors.Add(new WebHttpBehavior());
                    OnServiceHostCreated();

                    // Change data serialization behavior.
                    foreach (ServiceEndpoint endpoint in m_serviceHost.Description.Endpoints)
                    {
                        foreach (OperationDescription operation in endpoint.Contract.Operations)
                        {
                            // Following behavior property must be set for all operations of the web service to allow for the maximum number 
                            // of items of any object sent or received by the operation to be serialized/deserialized by the serializer.
                            DataContractSerializerOperationBehavior behavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
                            if (behavior != null)
                                behavior.MaxItemsInObjectGraph = int.MaxValue;
                        }
                    }

                    // Start the service.
                    m_serviceHost.Open();
                    OnServiceHostStarted();
                }

                // Initialize only once.
                m_initialized = true;
            }
        }

        /// <summary>
        /// Saves web service settings to the config file if the <see cref="PersistSettings"/> property is set to true.
        /// </summary>
        /// <exception cref="ConfigurationErrorsException"><see cref="SettingsCategory"/> has a value of null or empty string.</exception>
        public virtual void SaveSettings()
        {
            if (m_persistSettings)
            {
                // Ensure that settings category is specified.
                if (string.IsNullOrEmpty(m_settingsCategory))
                    throw new ConfigurationErrorsException("SettingsCategory property has not been set");

                // Save settings under the specified category.
                ConfigurationFile config = ConfigurationFile.Current;
                CategorizedSettingsElementCollection settings = config.Settings[m_settingsCategory];
                settings["Enabled", true].Update(m_enabled);
                settings["Singleton", true].Update(m_singleton);
                settings["ServiceUri", true].Update(m_serviceUri);
                settings["ServiceContract", true].Update(m_serviceContract);
                settings["ServiceDataFlow", true].Update(m_serviceDataFlow);
                config.Save();
            }
        }

        /// <summary>
        /// Loads saved web service settings from the config file if the <see cref="PersistSettings"/> property is set to true.
        /// </summary>
        /// <exception cref="ConfigurationErrorsException"><see cref="SettingsCategory"/> has a value of null or empty string.</exception>
        public virtual void LoadSettings()
        {
            if (m_persistSettings)
            {
                // Ensure that settings category is specified.
                if (string.IsNullOrEmpty(m_settingsCategory))
                    throw new ConfigurationErrorsException("SettingsCategory property has not been set");

                // Load settings from the specified category.
                ConfigurationFile config = ConfigurationFile.Current;
                CategorizedSettingsElementCollection settings = config.Settings[m_settingsCategory];
                settings.Add("Enabled", m_enabled, "True if this web service is enabled; otherwise False.");
                settings.Add("Singleton", m_singleton, "True if this web service is singleton; otherwise False.");
                settings.Add("ServiceUri", m_serviceUri, "URI where this web service is to be hosted.");
                settings.Add("ServiceContract", m_serviceContract, "Assembly qualified name of the contract interface implemented by this web service.");
                settings.Add("ServiceDataFlow", m_serviceDataFlow, "Flow of data (Incoming; Outgoing; BothWays) allowed for this web service.");
                Enabled = settings["Enabled"].ValueAs(m_enabled);
                Singleton = settings["Singleton"].ValueAs(m_singleton);
                ServiceUri = settings["ServiceUri"].ValueAs(m_serviceUri);
                ServiceContract = settings["ServiceContract"].ValueAs(m_serviceContract);
                ServiceDataFlow = settings["ServiceDataFlow"].ValueAs(m_serviceDataFlow);
            }
        }

        /// <summary>
        /// Returns an HTML help page containing a list of endpoints published by this REST web service along with a description of the endpoint if one is available.
        /// </summary>
        /// <returns>An <see cref="Stream"/> object containing the HTML help.</returns>
        [Description("Returns an HTML help page containing a list of endpoints published by this REST web service along with a description of the endpoint if one is available.")]
        public Stream Help()
        {
            // Local variables used in preparing the HTML.
            Type serviceType = this.GetType();
            string serviceName = serviceType.Name;
            StringBuilder responseText = new StringBuilder();
            WebGetAttribute webGetAttribute = null;
            WebInvokeAttribute webInvokeAttribute = null;
            DescriptionAttribute descriptionAttribute = null;

            // Prepare the HTML to be returned.
            responseText.Append("<html>");
            responseText.AppendLine();
            responseText.Append("<head>");
            responseText.AppendLine();
            responseText.AppendFormat("    <title>{0} - Documentation</title>", serviceName);
            responseText.AppendLine();
            responseText.Append("    <style type=\"text/css\">");
            responseText.AppendLine();
            responseText.Append("        body {margin: 0px;padding: 0px;font-family: Tahoma, Arial;}");
            responseText.AppendLine();
            responseText.Append("        .banner {padding: 10px 0px 3px 15px;font-size: 20pt;color: #ffffff;background-color: #003366;}");
            responseText.AppendLine();
            responseText.Append("        .content {margin: 15px;font-size: 10pt;}");
            responseText.AppendLine();
            responseText.Append("    </style>");
            responseText.AppendLine();
            responseText.Append("</head>");
            responseText.AppendLine();
            responseText.Append("<body>");
            responseText.AppendLine();
            responseText.AppendFormat("    <div class=\"banner\">{0}</div>", serviceName);
            responseText.AppendLine();
            responseText.Append("    <div class=\"content\">");
            responseText.AppendLine();
            responseText.Append("        <h3>Available Endpoints</h3>");
            responseText.AppendLine();
            responseText.Append("        <ul>");
            responseText.AppendLine();
            foreach (MethodInfo method in GetMethods(Type.GetType(m_serviceContract)))
            {
                // Check if the method in the service interface is REST.
                if (method.TryGetAttribute(out webGetAttribute))
                    responseText.AppendFormat("            <li><strong>GET {0}</strong>", webGetAttribute.UriTemplate);
                else if (method.TryGetAttribute(out webInvokeAttribute))
                    responseText.AppendFormat("            <li><strong>{0} {1}</strong>", webInvokeAttribute.Method, webInvokeAttribute.UriTemplate);

                // Check if the REST method has a description we can use.
                if (webGetAttribute != null || webInvokeAttribute != null)
                {
                    if (method.TryGetAttribute(out descriptionAttribute) ||
                        serviceType.GetMethod(method.Name).TryGetAttribute(out descriptionAttribute))
                        responseText.AppendFormat(" - {0}</li>", descriptionAttribute.Description);
                    else
                        responseText.Append("</li>");
                    responseText.AppendLine();
                }
            }
            responseText.Append("        </ul>");
            responseText.AppendLine();
            responseText.Append("    </div>");
            responseText.AppendLine();
            responseText.Append("</body>");
            responseText.AppendLine();
            responseText.Append("</html>");
            responseText.AppendLine();

            // Put the prepared HTML in a stream.
            MemoryStream response = new MemoryStream(Encoding.UTF8.GetBytes(responseText.ToNonNullString()));
            response.Position = 0;
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";

            // Return the stream containg the HTML.
            return response;
        }

        /// <summary>
        /// Releases the unmanaged resources used by the web service and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                try
                {
                    // This will be done regardless of whether the object is finalized or disposed.				
                    if (disposing)
                    {
                        // This will be done only when the object is disposed by calling Dispose().
                        SaveSettings();

                        if (m_serviceHost != null)
                            m_serviceHost.Close();
                    }
                }
                finally
                {
                    m_disposed = true;  // Prevent duplicate dispose.
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="ServiceHostCreated"/> event.
        /// </summary>
        protected virtual void OnServiceHostCreated()
        {
            if (ServiceHostCreated != null)
                ServiceHostCreated(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="ServiceHostStarted"/> event.
        /// </summary>
        protected virtual void OnServiceHostStarted()
        {
            if (ServiceHostStarted != null)
                ServiceHostStarted(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="ServiceProcessException"/> event.
        /// </summary>
        /// <param name="exception"><see cref="Exception"/> to sent to <see cref="ServiceProcessException"/> event.</param>
        protected virtual void OnServiceProcessException(Exception exception)
        {
            if (ServiceProcessException != null)
                ServiceProcessException(this, new EventArgs<Exception>(exception));
        }

        private IEnumerable<MemberInfo> GetMethods(Type contract)
        {
            List<Type> types = new List<Type>();
            List<MemberInfo> methods = new List<MemberInfo>();
            types.Add(contract);
            types.AddRange(contract.GetInterfaces());
            foreach (Type type in types)
            {
                methods.AddRange(type.GetMethods());
            }

            return methods.OrderBy(method => method.Name);
        }

        #endregion
    }
}