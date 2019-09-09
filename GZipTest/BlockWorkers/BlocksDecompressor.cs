namespace GZipTest.BlockWorkers
{
    using System;
    using System.IO;
    using System.IO.Compression;

    using GZipTest.Interfaces;

    /// <summary>
    /// Распаковщик блоков.
    /// </summary>
    public class BlocksDecompressor : ICompressor
    {
        /// <summary>
        /// Интерфейс очереди блоков на считывание.
        /// </summary>
        private readonly IBlocksQueue _readBlocksQueue;

        /// <summary>
        /// Интерфейс очереди блоков на запись.
        /// </summary>
        private readonly IBlocksQueue _writeBlocksQueue;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="readBlocksQueue">Интерфейс очереди блоков на считывание.</param>
        /// <param name="writeBlocksQueue">Интерфейс очереди блоков на запись.</param>
        public BlocksDecompressor(IBlocksQueue readBlocksQueue, IBlocksQueue writeBlocksQueue)
        {
            _readBlocksQueue = readBlocksQueue;
            _writeBlocksQueue = writeBlocksQueue;
        }

        /// <summary>
        /// Запускает работу распаковщика блоков.
        /// <remarks>Берет очередной блок из очереди на считывании, распаковывает его и добавляет в очередь блоков на запись.</remarks>
        /// </summary>
        public void Start()
        {
            try
            {
                while (true)
                {
                    var block = _readBlocksQueue.GetNextBlock();
                    if (block == null)
                        return;

                    using (var outMemoryStream = new MemoryStream(block.CompressedData))
                    {
                        using (var gzipStream = new GZipStream(outMemoryStream, CompressionMode.Decompress))
                        {
                            gzipStream.Read(block.Data, 0, block.Data.Length);
                        }

                        _writeBlocksQueue.AddBlock(block);
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error($"При распаковке блока возникла ошибка: {exception.Message}", exception);
                throw;
            }
        }
    }
}