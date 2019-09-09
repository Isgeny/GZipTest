namespace GZipTest.BlockWorkers
{
    using System;
    using System.IO;
    using System.IO.Compression;

    using GZipTest.Interfaces;

    /// <summary>
    /// Сжиматель блоков.
    /// </summary>
    public class BlocksCompressor : ICompressor
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
        public BlocksCompressor(IBlocksQueue readBlocksQueue, IBlocksQueue writeBlocksQueue)
        {
            _readBlocksQueue = readBlocksQueue;
            _writeBlocksQueue = writeBlocksQueue;
        }

        /// <summary>
        /// Запускает работу сжимателя блоков.
        /// <remarks>Берет очередной блок из очереди на считывании, сжимает его и добавляет в очередь блоков на запись.</remarks>
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

                    using (var outMemoryStream = new MemoryStream())
                    {
                        using (var gzipStream = new GZipStream(outMemoryStream, CompressionMode.Compress))
                        {
                            gzipStream.Write(block.Data, 0, block.Data.Length);
                        }

                        block.CompressedData = outMemoryStream.ToArray();

                        _writeBlocksQueue.AddBlock(block);
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error($"При сжатии блока возникла ошибка: {exception.Message}", exception);
                throw;
            }
        }
    }
}