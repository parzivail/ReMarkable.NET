using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace ReMarkable.NET.Util
{
    /// <summary>
    ///     Provides a source for creating loggers
    /// </summary>
    public class Lumberjack
    {
        static Lumberjack()
        {
            var config = new LoggingConfiguration();

            var format = "[${longdate}] [${threadname}/${level:uppercase=true}] [${logger}] ${message}";
            var layout = new SimpleLayout(format);

            var logfile = new FileTarget("logfile") {FileName = "output.log", Layout = layout};
            var logconsole = new ConsoleTarget("logconsole") {Layout = layout};

            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            LogManager.Configuration = config;
        }

        /// <summary>
        ///     Creates a logger with the specified header
        /// </summary>
        /// <param name="header">The "tag", usually the source, that is displayed with each message</param>
        /// <returns>A new logger that is configured with the library default formatting</returns>
        public static Logger CreateLogger(string header)
        {
            return LogManager.GetLogger(header);
        }
    }
}