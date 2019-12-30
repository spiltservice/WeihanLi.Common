﻿using System;
using System.Collections.Generic;

namespace WeihanLi.Common.Logging
{
    /// <summary>
    /// LogHelperExtensions
    /// </summary>
    public static class LogHelperExtensions
    {
        public static void Log(this ILogHelperLogger logger, LogHelperLevel loggerLevel, string msg) => logger.Log(loggerLevel, null, msg);

        #region Info

        public static void Info(this ILogHelperLogger logger, string msg, params object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                logger.Log(LogHelperLevel.Info, msg);
            }
            else
            {
                logger.Log(LogHelperLevel.Info, null, msg, parameters);
            }
        }

        public static void Info(this ILogHelperLogger logger, Exception ex, string msg) => logger.Log(LogHelperLevel.Info, ex, msg);

        public static void Info(this ILogHelperLogger logger, Exception ex) => logger.Log(LogHelperLevel.Info, ex, ex?.Message);

        #endregion Info

        #region Trace

        public static void Trace(this ILogHelperLogger logger, string msg, params object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                logger.Log(LogHelperLevel.Trace, msg);
            }
            else
            {
                logger.Log(LogHelperLevel.Trace, null, msg, parameters);
            }
        }

        public static void Trace(this ILogHelperLogger logger, Exception ex, string msg) => logger.Log(LogHelperLevel.Trace, ex, msg);

        public static void Trace(this ILogHelperLogger logger, Exception ex) => logger.Log(LogHelperLevel.Trace, ex, ex?.Message);

        #endregion Trace

        #region Debug

        public static void Debug(this ILogHelperLogger logger, string msg, params object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                logger.Log(LogHelperLevel.Debug, msg);
            }
            else
            {
                logger.Log(LogHelperLevel.Debug, null, msg, parameters);
            }
        }

        public static void Debug(this ILogHelperLogger logger, Exception ex, string msg) => logger.Log(LogHelperLevel.Debug, ex, msg);

        public static void Debug(this ILogHelperLogger logger, Exception ex) => logger.Log(LogHelperLevel.Debug, ex, ex?.Message);

        #endregion Debug

        #region Warn

        public static void Warn(this ILogHelperLogger logger, string msg, params object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                logger.Log(LogHelperLevel.Warn, null, msg);
            }
            else
            {
                logger.Log(LogHelperLevel.Warn, null, msg, parameters);
            }
        }

        public static void Warn(this ILogHelperLogger logger, Exception ex, string msg) => logger.Log(LogHelperLevel.Warn, ex, msg);

        public static void Warn(this ILogHelperLogger logger, Exception ex) => logger.Log(LogHelperLevel.Warn, ex, ex?.Message);

        #endregion Warn

        #region Error

        public static void Error(this ILogHelperLogger logger, string msg, params object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                logger.Log(LogHelperLevel.Error, null, msg);
            }
            else
            {
                logger.Log(LogHelperLevel.Error, null, msg, parameters);
            }
        }

        public static void Error(this ILogHelperLogger logger, Exception ex, string msg) => logger.Log(LogHelperLevel.Error, ex, msg);

        public static void Error(this ILogHelperLogger logger, Exception ex) => logger.Log(LogHelperLevel.Error, ex, ex?.Message);

        #endregion Error

        #region Fatal

        public static void Fatal(this ILogHelperLogger logger, string msg, params object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                logger.Log(LogHelperLevel.Fatal, null, msg);
            }
            else
            {
                logger.Log(LogHelperLevel.Fatal, null, msg, parameters);
            }
        }

        public static void Fatal(this ILogHelperLogger logger, Exception ex, string msg) => logger.Log(LogHelperLevel.Fatal, ex, msg);

        public static void Fatal(this ILogHelperLogger logger, Exception ex) => logger.Log(LogHelperLevel.Fatal, ex, ex?.Message);

        #endregion Fatal

        #region LogHelperFactory

        public static ILogHelperFactory WithMinimumLevel(this ILogHelperFactory logHelperFactory, LogHelperLevel logLevel)
        {
            return logHelperFactory.WithFilter(level => level >= logLevel);
        }

        public static ILogHelperFactory WithFilter(this ILogHelperFactory logHelperFactory, Func<LogHelperLevel, bool> filterFunc)
        {
            logHelperFactory.AddFilter((type, categoryName, logLevel, exception) => filterFunc.Invoke(logLevel));
            return logHelperFactory;
        }

        public static ILogHelperFactory WithFilter(this ILogHelperFactory logHelperFactory, Func<string, LogHelperLevel, bool> filterFunc)
        {
            logHelperFactory.AddFilter((type, categoryName, logLevel, exception) => filterFunc.Invoke(categoryName, logLevel));
            return logHelperFactory;
        }

        public static ILogHelperFactory WithFilter(this ILogHelperFactory logHelperFactory, Func<Type, string, LogHelperLevel, bool> filterFunc)
        {
            logHelperFactory.AddFilter((type, categoryName, logLevel, exception) => filterFunc.Invoke(type, categoryName, logLevel));
            return logHelperFactory;
        }

        public static ILogHelperFactory WithFilter(this ILogHelperFactory logHelperFactory, Func<Type, string, LogHelperLevel, Exception, bool> filterFunc)
        {
            logHelperFactory.AddFilter(filterFunc);
            return logHelperFactory;
        }

        public static ILogHelperFactory WithProvider(this ILogHelperFactory logHelperFactory, ILogHelperProvider logHelperProvider)
        {
            logHelperFactory.AddProvider(logHelperProvider);
            return logHelperFactory;
        }

        public static ILogHelperFactory WithEnricher<TEnricher>(this ILogHelperFactory logHelperFactory,
            TEnricher enricher) where TEnricher : ILogHelperLoggingEnricher
        {
            logHelperFactory.AddEnricher(enricher);
            return logHelperFactory;
        }

        public static ILogHelperFactory WithEnricher<TEnricher>(this ILogHelperFactory logHelperFactory) where TEnricher : ILogHelperLoggingEnricher, new()
        {
            logHelperFactory.AddEnricher(new TEnricher());
            return logHelperFactory;
        }

        public static ILogHelperFactory EnrichWithProperty(this ILogHelperFactory logHelperFactory, string propertyName, object value, bool overwrite = false)
        {
            logHelperFactory.AddEnricher(new PropertyLoggingEnricher(propertyName, value, overwrite));
            return logHelperFactory;
        }

        public static ILogHelperFactory EnrichWithProperty(this ILogHelperFactory logHelperFactory, string propertyName, Func<LogHelperLoggingEvent> valueFactory, bool overwrite = false)
        {
            logHelperFactory.AddEnricher(new PropertyLoggingEnricher(propertyName, valueFactory, overwrite));
            return logHelperFactory;
        }

        public static ILogHelperFactory EnrichWithProperty(this ILogHelperFactory logHelperFactory, string propertyName, Func<LogHelperLoggingEvent, object> valueFactory, Func<LogHelperLoggingEvent, bool> predict, bool overwrite = false)
        {
            logHelperFactory.AddEnricher(new PropertyLoggingEnricher(propertyName, valueFactory, predict, overwrite));
            return logHelperFactory;
        }

        #endregion LogHelperFactory

        #region LoggingEnricher

        public static void AddProperty(this LogHelperLoggingEvent loggingEvent, string propertyName,
            object propertyValue, bool overwrite = false)
        {
            if (null == loggingEvent)
            {
                throw new ArgumentNullException(nameof(loggingEvent));
            }

            if (loggingEvent.Properties == null)
            {
                loggingEvent.Properties = new Dictionary<string, object>();
            }
            if (loggingEvent.Properties.ContainsKey(propertyName) && overwrite == false)
            {
                return;
            }

            loggingEvent.Properties[propertyName] = propertyValue;
        }

        public static void AddProperty(this LogHelperLoggingEvent loggingEvent, string propertyName,
            Func<LogHelperLoggingEvent, object> propertyValueFactory, bool overwrite = false)
        {
            if (null == loggingEvent)
            {
                throw new ArgumentNullException(nameof(loggingEvent));
            }

            if (loggingEvent.Properties == null)
            {
                loggingEvent.Properties = new Dictionary<string, object>();
            }

            if (loggingEvent.Properties.ContainsKey(propertyName) && overwrite == false)
            {
                return;
            }

            loggingEvent.Properties[propertyName] = propertyValueFactory?.Invoke(loggingEvent);
        }

        #endregion LoggingEnricher
    }
}
