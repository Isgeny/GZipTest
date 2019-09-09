namespace GZipTest.BlockWorkers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using GZipTest.Interfaces;

    /// <summary>
    /// Очередь блоков.
    /// </summary>
    public class BlocksQueue : IBlocksQueue
    {
        /// <summary>
        /// Очередь блоков.
        /// </summary>
        private readonly Queue<Block> _blocks;

        /// <summary>
        /// Объект-синхронизатор потоков.
        /// </summary>
        private readonly object _locker;

        /// <summary>
        /// Максимальный размер очереди.
        /// </summary>
        private readonly int _maxQueueSize;

        /// <summary>
        /// Флаг окончания добавления в очередь.
        /// </summary>
        private bool _addingCompleted;

        /// <summary>
        /// Индекс текущего блока.
        /// </summary>
        private int _currentBlockIndex;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="maxQueueSize">Максимальный размер очереди.</param>
        public BlocksQueue(int maxQueueSize)
        {
            _blocks = new Queue<Block>();
            _locker = new object();
            _maxQueueSize = maxQueueSize;
        }

        /// <summary>
        /// Добавляет блок в очередь.
        /// </summary>
        /// <param name="block">Добавляемый блок.</param>
        public void AddBlock(Block block)
        {
            lock (_locker)
            {
                while (block.Index != _currentBlockIndex || _blocks.Count == _maxQueueSize)
                    Monitor.Wait(_locker);

                _blocks.Enqueue(block);

                _currentBlockIndex++;

                Monitor.PulseAll(_locker);
            }
        }

        /// <summary>
        /// Завершает добавление блоков в очередь.
        /// </summary>
        public void CompleteAdding()
        {
            lock (_locker)
            {
                _addingCompleted = true;
                Monitor.PulseAll(_locker);
            }
        }

        /// <summary>
        /// Возвращает следующий блок из очереди.
        /// </summary>
        /// <returns>
        /// Следующий блок в очереди.
        /// Если добавление в очередь было прекращено и очередь пуста возвращается null.
        /// </returns>
        public Block GetNextBlock()
        {
            lock (_locker)
            {
                while (!_blocks.Any() && !_addingCompleted)
                    Monitor.Wait(_locker);

                if (!_blocks.Any())
                    return null;

                Monitor.Pulse(_locker);
                return _blocks.Dequeue();
            }
        }
    }
}