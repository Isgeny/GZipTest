namespace GZipTest.BlockWorkers
{
    using System;
    using System.IO;

    using GZipTest.Constants;
    using GZipTest.Interfaces;

    /// <summary>
    /// Записыватель блоков в gzip-файл.
    /// </summary>
    public class GZipBlocksFileWriter : IWriter
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
        public GZipBlocksFileWriter(string filePath, IBlocksQueue writeBlocksQueue)
        {
            _filePath = filePath;
            _writeBlocksQueue = writeBlocksQueue;
        }

        /// <summary>
        /// Записывает блоки в файл.
        /// <remarks>Берет блоки из очереди и записывает их. В блоки помещает длину сжатых данных.</remarks>
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

                        var compressedData = block.CompressedData;
                        var compressedBlockLength = compressedData.Length;

                        var compressedBlockLengthBytes = BitConverter.GetBytes(compressedBlockLength);
                        compressedBlockLengthBytes.CopyTo(compressedData, BlockParameters.GZIP_BLOCK_LENGTH_OFFSET);

                        fileStream.Write(compressedData, 0, compressedBlockLength);
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