﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using System.IO;
using System.Text;

namespace Utils.CsvReaderWriter.Tests
{
    [TestClass]
    public class CSVReaderTests
    {
        [TestMethod]
        public void TestReadLineReturnsSimpleResult()
        {
            var csvsample = "a,\"b\",c\r\nd,e,\r\n";

            using (var instance = CreateInstance(csvsample))
            {
                var result = instance.ReadLine();

                Check.That(result).HasSize(3);
                Check.That(result).ContainsExactly("a", "b", "c");

                result = instance.ReadLine();

                Check.That(result).HasSize(3);
                Check.That(result).ContainsExactly("d", "e", "");
            }
        }

        [TestMethod]
        public void TestReadLineTreatMultilieCsv()
        {
            var csvsample = "a,b,\"c\n\nc\"\r\n";

            using (var instance = CreateInstance(csvsample))
            {
                var result = instance.ReadLine();

                Check.That(result).HasSize(3);
                Check.That(result).ContainsExactly("a", "b", "c\n\nc");
            }

        }

        [TestMethod]
        public void TestReadLineUnescapeQuote()
        {
            var csvsample = "\"a,a\",\"b,\"\"c\"\r\n";

            using (var instance = CreateInstance(csvsample))
            {
                var result = instance.ReadLine();

                Check.That(result).HasSize(2);
                Check.That(result).ContainsExactly("a,a", "b,\"c");
            }
        }


        [TestMethod]
        public void TestEndOfStream()
        {
            var csvsample = "a,b,c\r\na,b,c\r\n";

            using (var instance = CreateInstance(csvsample))
            {
                Check.That(instance.EndOfStream).IsFalse();

                instance.ReadLine();

                Check.That(instance.EndOfStream).IsFalse();

                instance.ReadLine();

                Check.That(instance.EndOfStream).IsTrue();
            }

        }

        private CSVReader CreateInstance(string src)
        {
            return new CSVReader(CreateStream(src), new UTF8Encoding(false));
        }

        private Stream CreateStream(string src)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(src));
        }
    }
}
