﻿using System;

namespace Utils.CsvReaderWriter.Attributes
{
    /// <summary>
    /// CSV column definition
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CSVColumnAttribute : System.Attribute
    {
        private readonly int _order;

        public CSVColumnAttribute(int order)
        {
            _order = order;
        }

        public int Order { get { return _order; } }
        public string Name { get; set; }
    }
}
