namespace GZipTest
{
    using System;
    using System.IO;
    using System.IO.Compression;

    using GZipTest.Interfaces;

    /// <summary>
    /// Аргументы приложения.
    /// </summary>
    public class ProgramArguments : IProgramArguments
    {
        /// <summary>
        /// Список аргументов в строковом формате.
        /// </summary>
        private readonly string[] _args;

        /// <summary>
        /// Тип операции - сжатие/распаковка.
        /// </summary>
        private CompressionMode? _compressionMode;

        /// <summary>
        /// Путь до исходного файла.
        /// </summary>
        private string _inputFilePath;

        /// <summary>
        /// Путь до результирующего файла.
        /// </summary>
        private string _outputFilePath;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        public ProgramArguments(string[] args)
        {
            _args = args;
        }

        /// <summary>
        /// Тип операции - сжатие/распаковка.
        /// </summary>
        public CompressionMode CompressionMode
        {
            get
            {
                if (_compressionMode == null)
                    ParseArguments();

                return _compressionMode.Value;
            }
        }

        /// <summary>
        /// Путь до исходного файла.
        /// </summary>
        public string InputFilePath
        {
            get
            {
                if (_inputFilePath == null)
                    ParseArguments();

                return _inputFilePath;
            }
        }

        /// <summary>
        /// Путь до результирующего файла.
        /// </summary>
        public string OutputFilePath
        {
            get
            {
                if (_outputFilePath == null)
                    ParseArguments();

                return _outputFilePath;
            }
        }

        /// <summary>
        /// Парсит аргументы командной строки.
        /// </summary>
        private void ParseArguments()
        {
            if (_args == null || _args.Length != 3)
                throw new ArgumentException(
                    "Недостаточно входных аргументов. Формат аргументов: \"compress/decompress [имя исходного файла] [имя результирующего файла]\".");

            var compressionMode = _args[0];
            var inputFile = _args[1];
            var outputFile = _args[2];

            if (!Enum.TryParse(compressionMode, true, out CompressionMode mode))
                throw new ArgumentException(
                    $"Некорректный тип операции: {compressionMode}. Формат: compress/decompress.");

            if (!File.Exists(inputFile))
                throw new ArgumentException($"Файл {inputFile} не существует.");

            if (inputFile == outputFile)
                throw new ArgumentException(
                    $"Исходный и результирующий файл не могут иметь одинаковый путь {inputFile}.");

            _compressionMode = mode;
            _inputFilePath = inputFile;
            _outputFilePath = outputFile;
        }
    }
}