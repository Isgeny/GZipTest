namespace GZipTest.Interfaces
{
    using System.IO.Compression;

    /// <summary>
    /// Интерфейс аргументов приложения.
    /// </summary>
    public interface IProgramArguments
    {
        /// <summary>
        /// Тип операции - сжатие/распаковка.
        /// </summary>
        CompressionMode CompressionMode { get; }

        /// <summary>
        /// Путь до исходного файла.
        /// </summary>
        string InputFilePath { get; }

        /// <summary>
        /// Путь до результирующего файла.
        /// </summary>
        string OutputFilePath { get; }
    }
}