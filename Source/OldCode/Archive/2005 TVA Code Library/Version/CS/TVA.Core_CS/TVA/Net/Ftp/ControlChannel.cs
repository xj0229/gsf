//*******************************************************************************************************
//  ControlChannel.cs
//  Copyright � 2008 - TVA, all rights reserved - Gbtc
//
//  Build Environment: C#, Visual Studio 2008
//  Primary Developer: James R Carroll
//      Office: PSO TRAN & REL, CHATTANOOGA - MR 2W-C
//       Phone: 423/751-2827
//       Email: jrcarrol@tva.gov
//
//  Code Modification History:
//  -----------------------------------------------------------------------------------------------------
//  05/22/2003 - James R Carroll
//       Generated original version of source code.
//
//*******************************************************************************************************

using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace TVA.Net.Ftp
{
    #region [ Enumerations ]

    public enum TransferMode
    {
        Ascii,
        Binary,
        Unknown
    }

    #endregion

    public class ControlChannel
    {
        #region [ Members ]

        // Fields
        private Session m_sessionHost;
        private SessionConnected m_session;
        private System.Net.Sockets.TcpClient m_connection;
        private string m_server;
        private int m_port;
        private TransferMode m_currentTransferMode;
        private Response m_lastResponse;

        #endregion

        #region [ Constructors ]

        internal ControlChannel(Session host)
        {
            m_connection = new TcpClient();
            m_server = "localhost";
            m_port = 21;
            m_sessionHost = host;
            m_currentTransferMode = TransferMode.Unknown;
        }

        ~ControlChannel()
        {
            if (m_connection != null)
                m_connection.Close();
        }

        #endregion

        #region [ Properties ]

        internal string Server
        {
            get
            {
                return m_server;
            }
            set
            {
                if (value.Length == 0)
                    throw new ArgumentNullException("Server", "Server property must not be blank.");

                m_server = value;
            }
        }

        internal int Port
        {
            get
            {
                return m_port;
            }
            set
            {
                m_port = value;
            }
        }

        public Response LastResponse
        {
            get
            {
                return m_lastResponse;
            }
        }

        internal SessionConnected Session
        {
            get
            {
                return m_session;
            }
            set
            {
                m_session = value;
            }
        }

        #endregion

        #region [ Methods ]

        internal void Connect()
        {
            m_connection.Connect(m_server, m_port);

            try
            {
                m_lastResponse = new Response(m_connection.GetStream());

                if (m_lastResponse.Code != Response.ServiceReady)
                    throw new ServerDownException("FTP service unavailable.", m_lastResponse);
            }
            catch
            {
                Close();
                throw;
            }
        }

        internal void Close()
        {
            try
            {
                m_connection.Close();
            }
            catch
            {
                // We keep going even if we can't close the connection
            }

            m_connection = null;
        }


        public void Command(string cmd)
        {
            byte[] buff = System.Text.Encoding.Default.GetBytes(cmd + Environment.NewLine);
            NetworkStream stream = m_connection.GetStream();

            m_sessionHost.OnCommandSent(cmd);
            stream.Write(buff, 0, buff.Length);
            RefreshResponse();
        }

        public void RefreshResponse()
        {
            lock (this)
            {
                m_lastResponse = new Response(m_connection.GetStream());
            }

            foreach (string s in m_lastResponse.Respones)
            {
                m_sessionHost.OnResponseReceived(s);
            }
        }

        internal void REST(long offset)
        {
            Command("REST " + offset);

            if (m_lastResponse.Code != Response.RequestFileActionPending)
                throw new ResumeNotSupportedException(m_lastResponse);
        }

        internal void STOR(string name)
        {
            Type(TransferMode.Binary);
            Command("STOR " + name);

            if (m_lastResponse.Code != Response.DataChannelOpenedTransferStart && m_lastResponse.Code != Response.FileOkBeginOpenDataChannel)
                throw new CommandException("Failed to send file " + name + ".", m_lastResponse);
        }

        internal void RETR(string name)
        {
            Type(TransferMode.Binary);
            Command("RETR " + name);

            if (m_lastResponse.Code != Response.DataChannelOpenedTransferStart && m_lastResponse.Code != Response.FileOkBeginOpenDataChannel)
                throw new CommandException("Failed to retrieve file " + name + ".", m_lastResponse);
        }

        internal void DELE(string fileName)
        {
            Command("DELE " + fileName);

            if (m_lastResponse.Code != Response.RequestFileActionComplete) // 250)
                throw new CommandException("Failed to delete file " + fileName + ".", m_lastResponse);
        }

        internal void RMD(string dirName)
        {
            Command("RMD " + dirName);

            if (m_lastResponse.Code != Response.RequestFileActionComplete) // 250)
                throw new CommandException("Failed to remove subdirectory " + dirName + ".", m_lastResponse);
        }

        internal string PWD()
        {
            Command("PWD");

            if (m_lastResponse.Code != 257)
                throw new CommandException("Cannot get current directory.", m_lastResponse);

            Match m = m_pwdExpression.Match(m_lastResponse.Message);
            return m.Groups[2].Value;
        }

        internal void CDUP()
        {
            Command("CDUP");

            if (m_lastResponse.Code != Response.RequestFileActionComplete)
                throw new CommandException("Cannot move to parent directory (CDUP).", m_lastResponse);
        }

        internal void CWD(string path)
        {
            Command("CWD " + path);

            if (m_lastResponse.Code != Response.RequestFileActionComplete && m_lastResponse.Code != Response.ClosingDataChannel)
                throw new CommandException("Cannot change directory to " + path + ".", m_lastResponse);
        }

        internal void QUIT()
        {
            Command("QUIT");
        }

        internal void Type(TransferMode mode)
        {
            if (mode == TransferMode.Unknown)
                return;

            if (mode == TransferMode.Ascii && m_currentTransferMode != TransferMode.Ascii)
                Command("TYPE A");
            else if (mode == TransferMode.Binary && m_currentTransferMode != TransferMode.Binary)
                Command("TYPE I");

            m_currentTransferMode = mode;
        }

        internal void Rename(string oldName, string newName)
        {
            Command("RNFR " + oldName);

            if (m_lastResponse.Code != Response.RequestFileActionPending)
                throw new CommandException("Failed to rename file from " + oldName + " to " + newName + ".", m_lastResponse);

            Command("RNTO " + newName);
            if (m_lastResponse.Code != Response.RequestFileActionComplete)
                throw new CommandException("Failed to rename file from " + oldName + " to " + newName + ".", m_lastResponse);
        }

        internal Queue List(bool passive)
        {
            const string errorMsgListing = "Error when listing server directory.";

            try
            {
                Type(TransferMode.Ascii);
                DataStream dataStream = GetPassiveDataStream();
                Queue lineQueue = new Queue();

                Command("LIST");

                if (m_lastResponse.Code != Response.DataChannelOpenedTransferStart && m_lastResponse.Code != Response.FileOkBeginOpenDataChannel)
                    throw new CommandException(errorMsgListing, m_lastResponse);

                StreamReader lineReader = new StreamReader(dataStream, System.Text.Encoding.Default);
                string line = lineReader.ReadLine();

                while (line != null)
                {
                    lineQueue.Enqueue(line);
                    line = lineReader.ReadLine();
                }

                lineReader.Close();

                if (m_lastResponse.Code != Response.ClosingDataChannel)
                    throw new CommandException(errorMsgListing, m_lastResponse);

                return lineQueue;
            }
            catch (IOException ie)
            {
                throw new System.Exception(errorMsgListing, ie);
            }
            catch (SocketException se)
            {
                throw new System.Exception(errorMsgListing, se);
            }
        }

        internal DataStream GetPassiveDataStream()
        {
            return GetPassiveDataStream(TransferDirection.Download);
        }

        internal DataStream GetPassiveDataStream(TransferDirection direction)
        {
            TcpClient client = new TcpClient();
            int port = 0;

            try
            {
                port = GetPassivePort();
                client.Connect(m_server, port);

                if (direction == TransferDirection.Download)
                    return new InputDataStream(this, client);
                else
                    return new OutputDataStream(this, client);
            }
            catch (IOException ie)
            {
                throw new System.Exception("Failed to open passive port (" + port + ") data connection due to IO exception: " + ie.Message + ".", ie);
            }
            catch (SocketException se)
            {
                throw new System.Exception("Failed to open passive port (" + port + ") data connection due to socket exception: " + se.Message + ".", se);
            }
        }

        private int GetPassivePort()
        {
            Command("PASV");

            if (m_lastResponse.Code == Response.EnterPassiveMode)
            {
                string[] numbers = m_regularExpression.Match(m_lastResponse.Message).Groups[2].Value.Split(',');
                return int.Parse(numbers[4]) * 256 + int.Parse(numbers[5]);
            }
            else
            {
                throw new CommandException("Failed to enter passive mode.", m_lastResponse);
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static Regex m_regularExpression = new Regex("(\\()(.*)(\\))");
        private static Regex m_pwdExpression = new Regex("(\")(.*)(\")");

        #endregion
    }
}