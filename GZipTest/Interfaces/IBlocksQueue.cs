namespace GZipTest.Interfaces
{
    using GZipTest.BlockWorkers;

    /// <summary>
    /// Интерфейс очереди по работе с блоками.
    /// </summary>
    public interface IBlocksQueue
    {
        /// <summary>
        /// Добавляет блок в очередь.
        /// </summary>
        /// <param name="block">Добавляемый блок.</param>
        void AddBlock(Block block);

        /// <summary>
        /// Завершает добавление блоков в очередь.
        /// </summary>
        void CompleteAdding();

        /// <summary>
        /// Возвращает следующий блок из очереди.
        /// </summary>
        /// <returns>
        /// Следующий блок в очереди.
        /// Если добавление в очередь было прекращено и очередь пуста возвращается null.
        /// </returns>
        Block GetNextBlock();
    }
}