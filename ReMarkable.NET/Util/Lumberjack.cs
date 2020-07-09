using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace ReMarkable.NET.Util
{
    public class Lumberjack
    {
        static Lumberjack()
        {
            var config = new LoggingConfiguration();

            var format = "[${longdate}] [${threadname}/${level:uppercase=true}] [${logger}] ${message}";
            var layout = new SimpleLayout(format);

            var logfile = new FileTarget("logfile") { FileName = "output.log", Layout = layout };
            var logconsole = new ConsoleTarget("logconsole") { Layout = layout };

            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            LogManager.Configuration = config;
        }

        public static Logger CreateLogger(string header)
        {
            return LogManager.GetLogger(header);
        }
    }
}
