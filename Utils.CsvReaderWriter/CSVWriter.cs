using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Utils.CsvReaderWriter.Attributes;
using Utils.CsvReaderWriter.Interfaces;

namespace Utils.CsvReaderWriter
{
    public class CSVWriter<T> : ICSVWriter<T>, IDisposable
    {
        private static readonly string DELIMITER = ",";
        private readonly StreamWriter _writer;
        private List<BindingProperty> _bindingProperties;

        public CSVWriter(Stream stream, Encoding encoding)
        {
            _writer = new StreamWriter(stream, encoding);
            _bindingProperties = extractBindingProperties();
        }

        private List<BindingProperty> extractBindingProperties()
        {
            var targetType = GetType().GetGenericArguments()[0];

            return targetType
                .GetProperties()
                .Select(i => new BindingProperty()
                {
                    Property = i,
                    CSVColumn = i.GetCustomAttributes(typeof(CSVColumnAttribute), false).FirstOrDefault() as CSVColumnAttribute
                })
                .Where(i => i.CSVColumn != null)
                .OrderBy(i => i.CSVColumn.Order)
                .ToList();
        }


        public void WriteLine(T record)
        {
            var values = _bindingProperties
                .Select(i => i.Property.GetValue(record))
                .Select(i => Quote(i))
                .ToArray();

            _writer.WriteLine(string.Join(DELIMITER, values));
        }

        public void WriteHeaderLine()
        {
            var headers = _bindingProperties
                            .Select(i => i.CSVColumn.Name ?? CamelCase2Title(i.Property.Name))
                            .Select(i => Quote(i))
                            .ToArray();

            _writer.WriteLine(string.Join(DELIMITER, headers));
        }

        public void Flush()
        {
            _writer.Flush();
        }

        public void Close()
        {
            _writer.Close();
        }

        private string CamelCase2Title(string src)
        {
            return Regex.Replace(src, "(?<!^)([A-Z])(?![A-Z])", " ${1}");
        }


        private string Quote(object src)
        {
            string ssrc = src != null ? src.ToString() : "";

            // via http://www.din.or.jp/~ohzaki/perl.htm#CSVfromValues
            // join ',', map {(s/"/""/g or /[\r\n,]/) ? qq("$_") : $_} @values;

            if (Regex.Match(ssrc, "[\"\\r\\n,]").Success)
            {
                return "\"" + ssrc.Replace("\"", "\"\"") + "\"";
            }
            else
            {
                return ssrc;
            }
        }

        private class BindingProperty
        {
            internal PropertyInfo Property;
            internal CSVColumnAttribute CSVColumn;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _writer.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~CSVWriter() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
