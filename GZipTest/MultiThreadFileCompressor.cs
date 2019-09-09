namespace GZipTest
{
    using System;
    using System.Threading;

    using GZipTest.Constants;
    using GZipTest.Interfaces;

    using Ninject;
    using Ninject.Parameters;

    /// <summary>
    /// Многопоточный сжиматель файла.
    /// </summary>
    public class MultiThreadFileCompressor
    {
        /// <summary>
        /// Ядро зависимостей приложения.
        /// </summary>
        private readonly IKernel _kernel;

        /// <summary>
        /// Массив потоков.
        /// <remarks>Индексация: 0 - поток чтения, 1 - поток записи, остальные - сжатие/распаковка.</remarks>
        /// </summary>
        private readonly Thread[] _threads;

        /// <summary>
        /// Количество потоков.
        /// </summary>
        private readonly int _threadsCount;

        /// <summary>
        /// Интерфейс очереди блоков на считывание.
        /// </summary>
        private IBlocksQueue _readBlocksQueue;

        /// <summary>
        /// Интерфейс очереди блоков на запись.
        /// </summary>
        private IBlocksQueue _writeBlocksQueue;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="kernel">Ядро зависимостей.</param>
        public MultiThreadFileCompressor(IKernel kernel)
        {
            _kernel = kernel;
            _threadsCount = Environment.ProcessorCount;
            _threads = new Thread[_threadsCount];
        }

        /// <summary>
        /// Запускает сжиматель.
        /// </summary>
        public void Start()
        {
            InitializeThreads();

            foreach (var thread in _threads)
            {
                thread?.Start();
            }

            _threads[0].Join();

            _readBlocksQueue.CompleteAdding();

            for (var i = 2; i < _threadsCount; i++)
                _threads[i].Join();

            _writeBlocksQueue.CompleteAdding();

            _threads[1].Join();
        }

        /// <summary>
        /// Инициализирует потоки.
        /// </summary>
        private void InitializeThreads()
        {
            var processorCount = Environment.ProcessorCount;
            var queueSizeArgument = new ConstructorArgument(BindingsParameters.MAX_QUEUE_SIZE_NAME, processorCount);
            _readBlocksQueue = _kernel.Get<IBlocksQueue>(queueSizeArgument);
            _writeBlocksQueue = _kernel.Get<IBlocksQueue>(queueSizeArgument);

            var readBlockQueueArgument = new ConstructorArgument(
                BindingsParameters.READ_BLOCKS_QUEUE_NAME, _readBlocksQueue);

            var blocksFileReader = _kernel.Get<IReader>(readBlockQueueArgument);

            var writeBlockQueueArgument = new ConstructorArgument(
                BindingsParameters.WRITE_BLOCKS_QUEUE_NAME, _writeBlocksQueue);

            var blocksFileWriter = _kernel.Get<IWriter>(writeBlockQueueArgument);

            _threads[0] = new Thread(blocksFileReader.Read);
            _threads[1] = new Thread(blocksFileWriter.Write);

            for (var i = 2; i < _threadsCount; i++)
            {
                var blocksCompressor = _kernel.Get<ICompressor>(readBlockQueueArgument, writeBlockQueueArgument);
                _threads[i] = new Thread(blocksCompressor.Start);
            }
        }
    }
}