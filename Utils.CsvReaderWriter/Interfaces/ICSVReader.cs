using System.Collections.Generic;

namespace Utils.CsvReaderWriter.Interfaces
{
    public interface ICSVReader
    {
        IEnumerable<string> ReadLine();
        bool EndOfStream { get; }
    }
}
