namespace GZipTest.Constants
{
    /// <summary>
    /// Параметры для работы с блоками.
    /// </summary>
    public static class BlockParameters
    {
        /// <summary>
        /// Размер блока (в байтах). Задан 1МБ.
        /// </summary>
        public const int BLOCK_SIZE = 1024 * 1024;

        /// <summary>
        /// Длина заголовка в блоке gzip.
        /// </summary>
        public const int GZIP_BLOCK_HEADER_SIZE = 8;

        /// <summary>
        /// Смещение позиции длины блока gzip.
        /// </summary>
        public const int GZIP_BLOCK_LENGTH_OFFSET = 4;
    }
}