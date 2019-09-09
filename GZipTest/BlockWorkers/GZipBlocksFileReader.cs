namespace GZipTest.BlockWorkers
{
    using System;
    using System.IO;

    using GZipTest.Constants;
    using GZipTest.Interfaces;

    /// <summary>
    /// Поблочный считыватель gzip-файла.
    /// </summary>
    public class GZipBlocksFileReader : IReader
    {
        /// <summary>
        /// Путь до файла.
        /// </summary>
        private readonly string _filePath;

        /// <summary>
        /// Интерфейс очереди блоков на считывание.
        /// </summary>
        private readonly IBlocksQueue _readBlocksQueue;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="filePath">Путь до файла.</param>
        /// <param name="readBlocksQueue">Интерфейс очереди блоков на считывание.</param>
        public GZipBlocksFileReader(string filePath, IBlocksQueue readBlocksQueue)
        {
            _filePath = filePath;
            _readBlocksQueue = readBlocksQueue;
        }

        /// <summary>
        /// Считывает файл поблочно.
        /// <remarks>Считанные блоки записывает в очередь. Из блоков извлекается информация о длине исходного блока и сжатого.</remarks>
        /// </summary>
        public void Read()
        {
            try
            {
                using (var fileStream = new FileStream(_filePath, FileMode.Open))
                {
                    var currentIndex = 0;

                    while (fileStream.Position != fileStream.Length)
                    {
                        var blockLengthBuffer = new byte[BlockParameters.GZIP_BLOCK_HEADER_SIZE];
                        fileStream.Read(blockLengthBuffer, 0, blockLengthBuffer.Length);

                        var compressedBlockLength = BitConverter.ToInt32(blockLengthBuffer,
                            BlockParameters.GZIP_BLOCK_LENGTH_OFFSET);

                        var compressedBlockData = new byte[compressedBlockLength];
                        blockLengthBuffer.CopyTo(compressedBlockData, 0);
                        fileStream.Read(compressedBlockData, BlockParameters.GZIP_BLOCK_HEADER_SIZE,
                            compressedBlockLength - BlockParameters.GZIP_BLOCK_HEADER_SIZE);

                        var blockLength = BitConverter.ToInt32(compressedBlockData,
                            compressedBlockLength - BlockParameters.GZIP_BLOCK_LENGTH_OFFSET);

                        var block = new Block(compressedBlockData, blockLength, currentIndex);

                        _readBlocksQueue.AddBlock(block);

                        currentIndex++;
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error($"При считывании файла {_filePath} произошла ошибка: {exception.Message}", exception);
                throw;
            }
        }
    }
}