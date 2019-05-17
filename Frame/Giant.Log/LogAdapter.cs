using NLog;

namespace Giant.Log
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
            this.logger.Debug(message);
        }

        public void Error(object message)
        {
            this.logger.Error(message);
        }

        public void Fatal(object message)
        {
            this.logger.Fatal(message);
        }

        public void Info(object message)
        {
            this.logger.Info(message);
        }

        public void Trace(object message)
        {
            this.logger.Trace(message);
        }

        public void Warn(object message)
        {
            this.logger.Warn(message);
        }
    }
}
