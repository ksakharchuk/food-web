using System;
using System.Web;
using System.Diagnostics;

namespace Support
{
    public class Logger
    {
        public enum LogTypes { All = 0xFFFF, Generic = 1, SQL = 2, Exception = 4, Stack = 8, Error = 16 };

        static string LogPath;
        static string LogFileName;
        static bool useDefaultFolder = false;	//is log files will be save in the the default folder

        public static void Initialize()
        {
            LogPath = ProjectSettings.GetValue("trace.FilePath");
            LogFileName = ProjectSettings.GetValue("trace.LogFile");
            if (LogPath == string.Empty || LogFileName == string.Empty)
                System.Diagnostics.Trace.WriteLine("Please add keys trace.FilePath and trace.LogFile to the configuration file!");
        }

        /// <summary>
        /// Is logging enabled according web.config
        /// </summary>
        public static bool GlobalTraceFlag
        {
            get
            {
                string traceEnable = ProjectSettings.GetValue("trace.Enable");
                if (traceEnable != string.Empty && traceEnable == "true")
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Formating the name of the global trace file
        /// </summary>
        static string GlobalLogFilename
        {
            get
            {
                if (!string.IsNullOrEmpty(LogFileName))
                {
                    string logDirectory = LogFileName;
                    logDirectory = logDirectory.Replace("{day}", DateTime.Now.ToString("dd"));
                    logDirectory = logDirectory.Replace("{month}", DateTime.Now.ToString("MM"));
                    logDirectory = logDirectory.Replace("{year}", DateTime.Now.ToString("yyyy"));
                    return logDirectory;
                }
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// Formating the name of the global trace listiner.
        /// </summary>
        static string GlobalTraceName
        {
            get
            {
                return string.Format("GlobalTrace_{0}", DateTime.Now.ToString("yyyy-MM-dd"));
            }
        }

        private static string _FileID = string.Empty;
        /// <summary>
        /// Unique ID of the non global trace file
        /// Set or Get
        /// </summary>
        public static string FileID
        {
            get
            {
                try
                {
                    return HttpContext.Current.Session["LoggerFileID"].ToString();
                }
                catch (Exception ex)
                {
                    return _FileID;
                }
            }
            set
            {
                try
                {
                    HttpContext.Current.Session["LoggerFileID"] = value;
                }
                catch
                {
                    _FileID = value;
                }
            }
        }

        /// <summary>
        /// Returns full directory of log files
        /// </summary>
        public static string LogDirectory
        {
            get
            {
                if (useDefaultFolder)
                    return LogPath;
                else
                {
                    string logDirectory = System.IO.Path.GetFileNameWithoutExtension(GlobalLogFilename);
                    if (!string.IsNullOrEmpty(System.IO.Path.GetDirectoryName(GlobalLogFilename)))
                        logDirectory = string.Format(@"{0}\{1}", System.IO.Path.GetDirectoryName(GlobalLogFilename), logDirectory);
                    string logPath = string.Format("{0}/{1}", LogPath, logDirectory);
                    if (!System.IO.Directory.Exists(logPath))
                    {
                        System.IO.Directory.CreateDirectory(logPath);
                    }
                    return logPath;
                }
            }
        }

        /// <summary>
        /// Initializing global trace listiner
        /// </summary>
        static void InitializeGlobalTrace()
        {
            lock (typeof(Logger))
            {
                if (System.Diagnostics.Trace.Listeners[GlobalTraceName] == null)
                {
                    if (!string.IsNullOrEmpty(GlobalLogFilename))
                    {
                        try
                        {
                            System.Diagnostics.Trace.AutoFlush = true;

                            string logFilename = string.Format("{0}/{1}", LogDirectory, System.IO.Path.GetFileName(GlobalLogFilename));
                            TextWriterTraceListener globalListener = new TextWriterTraceListener(logFilename, GlobalTraceName);
                            System.Diagnostics.Trace.Listeners.Add(globalListener);
                        }
                        catch (Exception ex)
                        {
                            System.Web.HttpContext.Current.Trace.Write(ex.Message);
                            System.Web.HttpContext.Current.Trace.Write(ex.StackTrace);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Format message for the trace
        /// </summary>		
        private static string GetFormatedMessage(LogTypes logType, string message, bool isGlobalTrace)
        {
            string LogType = string.Empty;
            switch (logType)
            {
                case LogTypes.Generic: LogType = " "; break;
                case LogTypes.SQL: LogType = "S"; break;
                case LogTypes.Stack: LogType = "#"; break;
                case LogTypes.Exception: LogType = "X"; break;
                case LogTypes.Error: LogType = "E"; break;
                default: LogType = " "; break;
            }

            //fileID part
            string fileID = string.Empty;
            //need to add when current trace is global and FileID isn't empty
            if (isGlobalTrace && !string.IsNullOrEmpty(FileID))
                fileID = string.Format("{0,-4} |", FileID);

            return string.Format("{0}{1,-10}|{2}{3}{4,-2}|{5}", fileID, DateTime.Now.ToString("HH:mm:ss"), "", "", LogType, message);
        }

        // <summary>
        /// Write log line to the listener
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        /// <param name="FileID"></param>
        static void WriteLine(string message, LogTypes logType, string FileID)
        {
            lock (typeof(Logger))
            {
                try
                {
                    if (!GlobalTraceFlag) return;

                    string formatedMessage = GetFormatedMessage(logType, message, true);
                    string traceName = GlobalTraceName;

                    if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Trace != null)
                    {
                        System.Web.HttpContext.Current.Trace.Write(formatedMessage);
                    }

                    //initialize trace listiner
                    if (System.Diagnostics.Trace.Listeners[traceName] == null)
                    {
                        if (traceName.StartsWith("GlobalTrace")) InitializeGlobalTrace();
                    }
                    if (System.Diagnostics.Trace.Listeners[traceName] == null)
                    {
                        System.Diagnostics.Trace.WriteLine(formatedMessage);
                        return;
                    }

                    //correct formmatedMessage for non-global trace
                    if (traceName.StartsWith("AgentTrace"))
                        formatedMessage = GetFormatedMessage(logType, message, false);
                    System.Diagnostics.Trace.Listeners[traceName].WriteLine(formatedMessage);
                    System.Diagnostics.Trace.Listeners[traceName].Flush();
                    System.Diagnostics.Trace.Listeners[traceName].Close();
                    System.Diagnostics.Trace.Listeners.Remove(traceName);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message);
                    System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// Write exception to the global trace file.
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteTrace(Exception ex)
        {
            if (ex == null) return;
            if (ex is System.Threading.ThreadAbortException) return;

            //we should use non-global trace instead of global			
            WriteLine(ex.Message, LogTypes.Exception, null);
            WriteLine(ex.StackTrace, LogTypes.Stack, null);
        }

        /// <summary>
        /// Write message to the global trace file.
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteTrace(string msg)
        {
            //we should use non-global trace instead of global
            WriteLine(msg, LogTypes.Generic, null);
        }

        public static void WriteTrace(string msg, LogTypes logType)
        {
            //we should use non-global trace instead of global			
            if (FileID != "")
                WriteLine(msg, logType, FileID);
            else
                WriteLine(msg, logType, null);
        }

        public static void WriteTrace(string msg, LogTypes logType, string FileID)
        {
            //we should use non-global trace instead of global			
            if (FileID != "")
                WriteLine(msg, logType, FileID);
            else
                WriteLine(msg, logType, null);
        }

        /// <summary>
        /// Write error to the global trace file.
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteError(string msg)
        {
           WriteLine(msg, LogTypes.Error, null);
        }

    }
}
