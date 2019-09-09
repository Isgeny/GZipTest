namespace GZipTest.Constants
{
    /// <summary>
    /// Параметры по работы с завивимостями.
    /// </summary>
    public class BindingsParameters
    {
        /// <summary>
        /// Наименование параметры аргументов командной строки.
        /// </summary>
        public const string ARGS_NAME = "args";

        /// <summary>
        /// Наименование параметра пути до файла.
        /// </summary>
        public const string FILE_PATH_NAME = "filePath";

        /// <summary>
        /// Наименование параметра максимального размера очереди.
        /// </summary>
        public const string MAX_QUEUE_SIZE_NAME = "maxQueueSize";

        /// <summary>
        /// Наименование параметра очереди блоков на считывание.
        /// </summary>
        public const string READ_BLOCKS_QUEUE_NAME = "readBlocksQueue";

        /// <summary>
        /// Наименование параметра очереди блоков на запись.
        /// </summary>
        public const string WRITE_BLOCKS_QUEUE_NAME = "writeBlocksQueue";
    }
}