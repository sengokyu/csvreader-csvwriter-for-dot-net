using System;

namespace Utils.CsvReaderWriter.Interfaces
{
    /// <summary>
    /// Write object to the stream as CSV
    /// </summary>
    public interface ICSVWriter<T> : IDisposable
    {
        /// <summary>
        /// Write a header line
        /// </summary>
        void WriteHeaderLine();

        /// <summary>
        /// Write a line
        /// </summary>
        /// <param name="record"></param>
        void WriteLine(T record);
    }
}
