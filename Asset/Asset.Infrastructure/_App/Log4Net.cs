using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Repository.Hierarchy;

namespace Asset.Infrastructure._App
{
    public interface ILog4Net {
        void Error(object msg);
        void Error(object msg, Exception ex);
        void Error(Exception ex);
        void Info(object msg, string loggerName = "");
    }
    public class Log4Net:ILog4Net
    {
        private ILog Log { get; }

        public Log4Net()
        {
            Log = LogManager.GetLogger(typeof(Logger));
        }

        public void Error(object msg)
        {
            Log.Error(msg);
        }

        public void Error(object msg, Exception ex)
        {
            Log.Error(msg, ex);
        }

        public void Error(Exception ex)
        {
            Log.Error(ex.Message, ex);
        }


        public void Info(object msg, string loggerName = "")
        {
            if (string.IsNullOrEmpty(loggerName))
            {
                Log.Info(msg);
            }

            try
            {
                ILog log = LogManager.GetLogger(loggerName);
                log.Info(msg);
            }
            catch { }
        }
    }
}
