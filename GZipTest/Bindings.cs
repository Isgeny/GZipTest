namespace GZipTest
{
    using System.IO.Compression;

    using GZipTest.BlockWorkers;
    using GZipTest.Constants;
    using GZipTest.Interfaces;

    using Ninject;
    using Ninject.Modules;

    /// <summary>
    /// Класс для настройки зависимостей приложения.
    /// </summary>
    public class Bindings : NinjectModule
    {
        /// <summary>
        /// Аргументы командной строки.
        /// </summary>
        private readonly string[] _args;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        public Bindings(string[] args)
        {
            _args = args;
        }

        /// <summary>
        /// Настройка контейнера зависимостей.
        /// </summary>
        public override void Load()
        {
            Bind<IProgramArguments>().To<ProgramArguments>()
                .WithConstructorArgument(BindingsParameters.ARGS_NAME, _args);

            var arguments = Kernel.Get<IProgramArguments>();

            Bind<IBlocksQueue>().To<BlocksQueue>();

            Bind<IReader>().To<BlocksFileReader>()
                .When(r => arguments.CompressionMode == CompressionMode.Compress)
                .WithConstructorArgument(BindingsParameters.FILE_PATH_NAME, arguments.InputFilePath);

            Bind<IReader>().To<GZipBlocksFileReader>()
                .When(r => arguments.CompressionMode == CompressionMode.Decompress)
                .WithConstructorArgument(BindingsParameters.FILE_PATH_NAME, arguments.InputFilePath);

            Bind<IWriter>().To<BlocksFileWriter>()
                .When(r => arguments.CompressionMode == CompressionMode.Decompress)
                .WithConstructorArgument(BindingsParameters.FILE_PATH_NAME, arguments.OutputFilePath);

            Bind<IWriter>().To<GZipBlocksFileWriter>()
                .When(r => arguments.CompressionMode == CompressionMode.Compress)
                .WithConstructorArgument(BindingsParameters.FILE_PATH_NAME, arguments.OutputFilePath);

            Bind<ICompressor>().To<BlocksCompressor>()
                .When(r => arguments.CompressionMode == CompressionMode.Compress);

            Bind<ICompressor>().To<BlocksDecompressor>()
                .When(r => arguments.CompressionMode == CompressionMode.Decompress);
        }
    }
}