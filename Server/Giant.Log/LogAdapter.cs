using NLog;

namespace Giant.Logger
{
    public interface ILog
    {
        void Trace(object message);

        void Debug(object message);

        void Info(object message);

        void Warn(object message);

        void Error(object message);

        void Fatal(object message);
    }

    public class LogAdapter : ILog
    {
        private readonly NLog.Logger logger = LogManager.GetLogger("Logger");

        public void Debug(object message)
        {
            logger.Debug(message);
        }

        public void Error(object message)
        {
            logger.Error(message);
        }

        public void Fatal(object message)
        {
            logger.Fatal(message);
        }

        public void Info(object message)
        {
            logger.Info(message);
        }

        public void Trace(object message)
        {
            logger.Trace(message);
        }

        public void Warn(object message)
        {
            logger.Warn(message);
        }
    }
}