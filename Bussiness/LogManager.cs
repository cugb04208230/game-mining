using log4net;
using System;

namespace Bussiness
{
    public class LogManager : BaseManager
    {
        private const string DefaultLoggerName = "DefaultLog";
        private const string DebugLoggerName = "DebugLog";
        private const string ErrorLoggerName = "ErrorLog";
        private const string FatalLoggerName = "FatalLog";

        public LogManager(MiddleTier middleTier)
            : base(middleTier)
        {

        }
        public ILog GetLogManager(string appenderName)
        {
            return log4net.LogManager.GetLogger(appenderName);
        }
        public ILog GetLogManager(Type t)
        {
            return log4net.LogManager.GetLogger(t);
        }

        public void Info(string message)
        {
            ILog log = log4net.LogManager.GetLogger(DefaultLoggerName);
            log.Info(message);
        }
        public void Info(string appenderName, string message)
        {
            ILog log = log4net.LogManager.GetLogger(appenderName);
            log.Info(message);
        }
        public void Info(Type t, string message)
        {
            ILog log = log4net.LogManager.GetLogger(t);
            log.Info(message);
        }
        public void Debug(string message)
        {
            ILog log = log4net.LogManager.GetLogger(DebugLoggerName);
            log.Debug(message);
        }
        public void Error(string message)
        {
            ILog log = log4net.LogManager.GetLogger(ErrorLoggerName);
            log.Error(message);
        }


	    public void Error(Exception e)
	    {
		    ILog log = log4net.LogManager.GetLogger(ErrorLoggerName);
		    log.Error(e);
	    }

		public void Fatal(string message)
        {
            ILog log = log4net.LogManager.GetLogger(FatalLoggerName);
            log.Fatal(message);
        }
    }
}
