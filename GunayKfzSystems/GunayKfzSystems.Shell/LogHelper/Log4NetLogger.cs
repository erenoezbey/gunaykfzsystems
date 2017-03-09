using System.Linq;
using log4net;
using Prism.Logging;

namespace GunayKfzSystems.Shell.LogHelper
{
   public class Log4NetLogger : ILoggerFacade
    {




        #region Fields

        /* Note that the ILog interface and
         * the LogManager object are Log4Net members.
         * They are used to instantiate the Log4Net 
         * instance to which we will log. */

        // Member variables
       private readonly ILog m_Logger =
           LogManager.GetCurrentLoggers().FirstOrDefault(p => p.Logger.Name == "MainLog");

        #endregion

        #region ILoggerFacade Members

        /// <summary>
        /// Writes a log message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="category">The message category.</param>
        /// <param name="priority">Not used by Log4Net; pass Priority.None.</param>
        public void Log(string message, Category category, Priority priority)
        {
            switch (category)
            {
                case Category.Debug:
                    m_Logger.Debug(message);
                    break;
                case Category.Warn:
                    m_Logger.Warn(message);
                    break;
                case Category.Exception:
                    m_Logger.Error(message);
                    break;
                case Category.Info:
                    m_Logger.Info(message);
                    break;
            }
        }

        #endregion
    }
}
