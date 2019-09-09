namespace GZipTest.BlockWorkers
{
    using System;
    using System.IO;

    using GZipTest.Constants;
    using GZipTest.Interfaces;

    /// <summary>
    /// Поблочный считыватель файла.
    /// </summary>
    public class BlocksFileReader : IReader
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
        public BlocksFileReader(string filePath, IBlocksQueue readBlocksQueue)
        {
            _filePath = filePath;
            _readBlocksQueue = readBlocksQueue;
        }

        /// <summary>
        /// Считывает файл поблочно.
        /// <remarks>Считанные блоки записывает в очередь.</remarks>
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
                        var blockLength = Math.Min(BlockParameters.BLOCK_SIZE, fileStream.Length - fileStream.Position);
                        var blockData = new byte[blockLength];
                        fileStream.Read(blockData, 0, blockData.Length);

                        var block = new Block(blockData, currentIndex);
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