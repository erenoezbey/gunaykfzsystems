using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Prism.Logging;

namespace GunayKfzSystems.Core
{
    public class CustomLogger : ILoggerFacade
    {
        private ILog log;

        public CustomLogger()
        {
            log4net.Config.XmlConfigurator.Configure();
            this.log = LogManager.GetCurrentLoggers().FirstOrDefault(p => p.Logger.Name == "MainLog");

        }

        #region Implementation of ILoggerFacade

        public void Log(string message, Category category, Priority priority)
        {
            switch (category)
            {
                case Category.Debug:
                    this.log.Debug(message);
                    break;
                case Category.Exception:
                    this.log.Error(message);
                    break;
                case Category.Info:
                    this.log.Info(message);
                    break;
                case Category.Warn:
                    this.log.Warn(message);
                    break;

            }
        }

        #endregion
    }
}
