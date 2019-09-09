namespace GZipTest
{
    using System;

    using GZipTest.Constants;

    using Ninject;

    /// <summary>
    /// Класс приложения.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Точка входа в программу.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        /// <returns>Статусный код.</returns>
        private static int Main(string[] args)
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += (s, e) => Environment.Exit(StatusCodes.ERROR_CODE);

                var kernel = new StandardKernel(new Bindings(args));
                var compressor = new MultiThreadFileCompressor(kernel);
                compressor.Start();

                return StatusCodes.SUCCESS_CODE;
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message, exception);
                return StatusCodes.ERROR_CODE;
            }
        }
    }
}