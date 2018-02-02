using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using System.IO;
using System.Text;
using Utils.CsvReaderWriter.Attributes;

namespace Utils.CsvReaderWriter.Tests
{
    [TestClass]
    public class CSVWriterTests
    {
        [TestMethod]
        public void TestWriterHeaderLine()
        {
            var encoding = new UTF8Encoding(false); // UTF-8 BOM must die !!

            using (var stream = new MemoryStream())
            using (var instance = new CSVWriter<SampleBean>(stream, encoding))
            {
                instance.WriteHeaderLine();

                instance.Flush();

                var actual = encoding.GetString(stream.ToArray());

                var expected = "Column1,\"Original, \"\"Name\",My Number\r\n";

                Check.That(actual).IsEqualTo(expected);
            }
        }

        [TestMethod]
        public void TestWriteLine()
        {
            var encoding = new UTF8Encoding(false); // UTF-8 BOM must die !!

            var bean = new SampleBean()
            {
                Column1 = "value1",
                Column2 = "value\nvalue,value\"",
                MyNumber = 1234
            };

            using (var stream = new MemoryStream())
            using (var instance = new CSVWriter<SampleBean>(stream, encoding))
            {
                instance.WriteLine(bean);

                instance.Flush();

                var result = encoding.GetString(stream.ToArray());

                var expected = "value1,\"value\nvalue,value\"\"\",1234\r\n";

                Check.That(result).IsEqualTo(expected);
            }

        }

        private class SampleBean
        {

            public string Ignored { get; set; }

            [CSVColumn(1)]
            public string Column1 { get; set; }

            [CSVColumn(2, Name = @"Original, ""Name")]
            public string Column2 { get; set; }

            [CSVColumn(3)]
            public int MyNumber { get; set; }
        }

    }
}
