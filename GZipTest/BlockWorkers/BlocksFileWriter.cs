namespace GZipTest.BlockWorkers
{
    using System;
    using System.IO;

    using GZipTest.Interfaces;

    /// <summary>
    /// Записыватель блоков в файл.
    /// </summary>
    public class BlocksFileWriter : IWriter
    {
        /// <summary>
        /// Путь до файла.
        /// </summary>
        private readonly string _filePath;

        /// <summary>
        /// Интерфейс очереди блоков на запись.
        /// </summary>
        private readonly IBlocksQueue _writeBlocksQueue;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="filePath">Путь до файла.</param>
        /// <param name="writeBlocksQueue">Интерфейс очереди блоков на запись.</param>
        public BlocksFileWriter(string filePath, IBlocksQueue writeBlocksQueue)
        {
            _filePath = filePath;
            _writeBlocksQueue = writeBlocksQueue;
        }

        /// <summary>
        /// Записывает блоки в файл.
        /// <remarks>Берет блоки из очереди и записывает их.</remarks>
        /// </summary>
        public void Write()
        {
            try
            {
                using (var fileStream = File.Create(_filePath))
                {
                    while (true)
                    {
                        var block = _writeBlocksQueue.GetNextBlock();
                        if (block == null)
                            return;

                        fileStream.Write(block.Data, 0, block.Data.Length);
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error($"При записи в файл {_filePath} произошла ошибка: {exception.Message}", exception);
                throw;
            }
        }
    }
}