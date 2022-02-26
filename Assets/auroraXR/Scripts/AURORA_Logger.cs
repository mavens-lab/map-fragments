/**
 * AURORA-NET Unity API
 * 
 * Provides management of all known Aurora Objects.
 * 
 * Developer: Stormfish Scientific Corporation
 * Author: Theron T. Trout
 * https://www.stormfish.io
 * 
 * 
 * Copyright (C) 2019, 2020 by Stormfish Scientific Corporation
 * All Rights Reserved
 *
 * See LICENSE file for Terms of Use.
 * 
 * THERE IS NO WARRANTY FOR THE PROGRAM, TO THE EXTENT PERMITTED BY APPLICABLE
 * LAW. EXCEPT WHEN OTHERWISE STATED IN WRITING THE COPYRIGHT HOLDERS AND/OR
 * OTHER PARTIES PROVIDE THE PROGRAM “AS IS” WITHOUT WARRANTY OF ANY KIND,
 * EITHER EXPRESSED OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. THE
 * ENTIRE RISK AS TO THE QUALITY AND PERFORMANCE OF THE PROGRAM IS WITH YOU.
 * SHOULD THE PROGRAM PROVE DEFECTIVE, YOU ASSUME THE COST OF ALL NECESSARY
 * SERVICING, REPAIR OR CORRECTION. YOU ARE SOLELY RESPONSIBLE FOR DETERMINING
 * THE APPROPRIATENESS OF USING OR REDISTRIBUTING THE WORK AND ASSUME ANY
 * RISKS ASSOCIATED WITH YOUR EXERCISE OF PERMISSIONS UNDER THIS LICENSE.
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

using AURORA.Protobuf.ServerData;
using UnityEngine;

public class AuroraUnityLogger : AURORA.ClientAPI.AuroraLogger
{
    public class LogEntry
    {
        private LogMessage.Types.Severities m_severity;
        private string m_message;
        private string m_component;
        private string m_subcomponent;

        public LogEntry(
            LogMessage.Types.Severities severity,
            string message,
            string component,
            string subcomponent
        )
        {
            m_severity = severity;
            m_message = message;
            m_component = component;
            m_subcomponent = subcomponent;
        }

        public LogMessage.Types.Severities Severity { get => m_severity; set => m_severity = value; }
        public string Message { get => m_message; set => m_message = value; }
        public string Component { get => m_component; set => m_component = value; }
        public string Subcomponent { get => m_subcomponent; set => m_subcomponent = value; }
    }

    private ConcurrentQueue<LogEntry> m_logQueue;

    public ConcurrentQueue<LogEntry> LogQueue
    {
        get => m_logQueue;
    }

    public AuroraUnityLogger()
        : base()
    {
        m_logQueue = new ConcurrentQueue<LogEntry>();
    }

    protected override void HandleMessage(LogMessage.Types.Severities severity, string message, string component = "", string subcomponent = "")
    {
        base.HandleMessage(severity, message, component, subcomponent);

        LogEntry le = new LogEntry(severity, message, component, subcomponent);

        m_logQueue.Enqueue(le);
    }
}

public class AuroraUnityFileLogger : AuroraUnityLogger
{
    private System.Threading.Mutex m_fileMutex;
    private string m_filename;

    System.IO.StreamWriter m_fileStream;
    
    public AuroraUnityFileLogger(string filename)
        : base()
    {
        m_fileMutex = new System.Threading.Mutex();

        lock (m_fileMutex)
        {
            m_filename = filename;

            m_fileStream = new System.IO.StreamWriter(filename);
        }
    }

    public string Filename { get => m_filename; }

    protected override void HandleMessage(LogMessage.Types.Severities severity, string message, string component = "", string subcomponent = "")
    {
        base.HandleMessage(severity, message, component, subcomponent);

        string formatted_message = "";

        //if (component == "")
        //    component = "";
        //if (subcomponent == "")
        //    subcomponent = "";

        if (subcomponent != "")
        {
            formatted_message = string.Format("{0} [{1}/{2}]: {3}",
                severity.ToString(), component, subcomponent, message);
        }
        else if (component != "")
        {
            formatted_message = string.Format("{0} [{1}]: {2}",
                severity.ToString(), component, message);
        }
        else
        {
            formatted_message = string.Format("{0}: {1}", severity.ToString(), message);
        }

        formatted_message = System.DateTime.Now.ToLongTimeString() + " " + formatted_message;

        lock(m_fileMutex)
        {
            m_fileStream.WriteLine(formatted_message);
            m_fileStream.Flush();
        }

        LogEntry le = new LogEntry(severity, message, component, subcomponent);

        LogQueue.Enqueue(le);
    }
}

public class AURORA_Logger : MonoBehaviour
{
    //private AURORA.ClientAPI.AuroraLogger m_auroraLogger;

	//[SerializeField]
	//private AURORA_Manager m_auroraManager;

    [SerializeField]
    private string m_defaultComponent = "";

    [SerializeField]
    private string m_defaultSubcomponent = "";

    [Tooltip("If not empty writes logs to file.  Use %DATAPATH% to insert Unity DataPath")]
    [SerializeField]
    private string m_logFileName = "";

    private AuroraUnityLogger m_auroraUnityLogger;

    //private AuroraUnityFileLogger m_auroraUnityFileLogger;

    private void Reset()
    {
    }
	

    private void Awake()
    {
        
        if(m_logFileName.Trim().Length > 0)
        {
            string filename = m_logFileName.Replace("%DATAPATH%", Application.dataPath);

            m_auroraUnityLogger = new AuroraUnityFileLogger(filename);

            UnityEngine.Debug.Log("Logging auroraXR log events to: " + filename);

        }
        else
        {
            m_auroraUnityLogger = new AuroraUnityLogger();
        }
        //if(m_auroraManager == null)
        //{
        //	m_auroraManager = GetComponent<AURORA_Manager>();

        //	if(m_auroraManager == null)
        //	{
        //		UnityEngine.Debug.LogError("AURORA_Logger could AURORA Manager not set.");
        //	}
        //}
        m_auroraUnityLogger.Start();
    }

    public void InitializeLogger()
    {
    }

    public AURORA.ClientAPI.AuroraLogger AuroraLogger
    {
        get
        {
            return m_auroraUnityLogger;
        }
    }

    public string DefaultComponent { get => m_defaultComponent; set => m_defaultComponent = value; }
    public string DefaultSubcomponent { get => m_defaultSubcomponent; set => m_defaultSubcomponent = value; }

    public void Debug(string message, string component = "", string subcomponent = "")
    {
        if (component == "")
            component = DefaultComponent;
        if (subcomponent == "")
            subcomponent = DefaultSubcomponent;

        if (subcomponent != "")
        {
            UnityEngine.Debug.Log(string.Format("Debug [{0}/{1}]: {2}",
                component, subcomponent, message));
        }
        else if (component != "")
        {
            UnityEngine.Debug.Log(string.Format("Debug [{0}]: {1}",
                component, message));
        }
        else
        {
            UnityEngine.Debug.Log(string.Format("Debug: " + message));
        }
    }

    public void Info(string message, string component = "", string subcomponent = "")
    {
        if (component == "")
            component = DefaultComponent;
        if (subcomponent == "")
            subcomponent = DefaultSubcomponent;

        if (subcomponent != "")
        {
            UnityEngine.Debug.Log(string.Format("Info [{0}/{1}]: {2}",
                component, subcomponent, message));
        }
        else if (component != "")
        {
            UnityEngine.Debug.Log(string.Format("Info [{0}]: {1}",
                component, message));
        }
        else
        {
            UnityEngine.Debug.Log(string.Format("Info: " + message));
        }
    }

    public void Error(string message, string component = "", string subcomponent = "")
    {
        if (component == "")
            component = DefaultComponent;
        if (subcomponent == "")
            subcomponent = DefaultSubcomponent;

        if (subcomponent != "")
        {
            UnityEngine.Debug.LogError(string.Format("Debug [{0}/{1}]: {2}",
                component, subcomponent, message));
        }
        else if (component != "")
        {
            UnityEngine.Debug.LogError(string.Format("Debug [{0}]: {1}",
                component, message));
        }
        else
        {
            UnityEngine.Debug.LogError(string.Format("Debug: " + message));
        }
    }

    public void LogException(System.Exception exception, string message="", string component = "", string subcomponent = "")
    {
        Error("Exception in " + exception.TargetSite, component, subcomponent);
        Error(exception.StackTrace, component, subcomponent);
        if (message.Trim().Length > 0)
        {
            Error(message, component, subcomponent);
        }
    }

    public void Warning(string message, string component = "", string subcomponent = "")
    {
        if (component == "")
            component = DefaultComponent;
        if (subcomponent == "")
            subcomponent = DefaultSubcomponent;

        if (subcomponent != "")
        {
            UnityEngine.Debug.LogWarning(string.Format("Debug [{0}/{1}]: {2}",
                component, subcomponent, message));
        }
        else if (component != "")
        {
            UnityEngine.Debug.LogWarning(string.Format("Debug [{0}]: {1}",
                component, message));
        }
        else
        {
            UnityEngine.Debug.LogWarning(string.Format("Debug: " + message));
        }
    }

    internal void Stop()
    {
        if(m_auroraUnityLogger != null)
        {
            m_auroraUnityLogger.Stop();
        }
    }

    private void Update()
    {
        if(!m_auroraUnityLogger.LogQueue.IsEmpty)
        {
            while(m_auroraUnityLogger.LogQueue.TryDequeue(out AuroraUnityLogger.LogEntry result))
            {
                switch(result.Severity)
                {
                    case LogMessage.Types.Severities.Alert:
                        Error(result.Message, result.Component, result.Subcomponent);
                        break;

                    case LogMessage.Types.Severities.Critical:
                        Error(result.Message, result.Component, result.Subcomponent);
                        break;

                    case LogMessage.Types.Severities.Debug:
                        Debug(result.Message, result.Component, result.Subcomponent);
                        break;

                    case LogMessage.Types.Severities.Emergency:
                        Error(result.Message, result.Component, result.Subcomponent);
                        break;

                    case LogMessage.Types.Severities.Error:
                        Error(result.Message, result.Component, result.Subcomponent);
                        break;

                    case LogMessage.Types.Severities.Info:
                        Info(result.Message, result.Component, result.Subcomponent);
                        break;

                    case LogMessage.Types.Severities.Notice:
                        Info(result.Message, result.Component, result.Subcomponent);
                        break;

                    case LogMessage.Types.Severities.Warning:
                        Warning(result.Message, result.Component, result.Subcomponent);
                        break;

                    default:
                        Error(result.Message, result.Component, result.Subcomponent);
                        break;
                }
            }
        }
    }
}
