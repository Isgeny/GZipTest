namespace GZipTest.BlockWorkers
{
    /// <summary>
    /// Блок байтов.
    /// </summary>
    public class Block
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="data">Массив байтов.</param>
        /// <param name="index">Индекс блока.</param>
        public Block(byte[] data, int index)
        {
            Data = data;
            Index = index;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="compressedData">Массив сжатых байтов.</param>
        /// <param name="dataLength">Длина не сжатых данных</param>
        /// <param name="index">Индекс блока.</param>
        public Block(byte[] compressedData, int dataLength, int index)
        {
            CompressedData = compressedData;
            Data = new byte[dataLength];
            Index = index;
        }

        /// <summary>
        /// Сжатые данные блока.
        /// </summary>
        public byte[] CompressedData { get; set; }

        /// <summary>
        /// Данные блока.
        /// </summary>
        public byte[] Data { get; }

        /// <summary>
        /// Индекс блока.
        /// </summary>
        public int Index { get; }
    }
}