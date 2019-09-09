namespace GZipTest
{
    using System;

    using NLog;

    /// <summary>
    /// Обертка над NLog.
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Инстанс логгера.
        /// </summary>
        private static readonly Logger Logger;

        /// <summary>
        /// Статический конструктор. Инициализирует NLog.
        /// </summary>
        static Log()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Отображает лог об ошибке.
        /// </summary>
        /// <param name="message">Сообщение ошибки.</param>
        /// <param name="exception">Возникшее исключение.</param>
        public static void Error(string message, Exception exception = null)
        {
            if (exception == null)
                Logger.Error(message);
            else
                Logger.Error(exception, message);
        }
    }
}