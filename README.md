# CSVReader/CSVWriter for .Net

## What's this?

This is simple CSV reading/writing implementation.

## Features

- Read and write Excel style CSV.
- Quoted value supported.
- Lines/Quotes/Commas in value supported.


## Usage


### CSV Reader

```Csharp:CSVReaderUsage.cs
using (var reader = new CSVReader(stream, new UTF8Encoding(false)))
{
    while (!reader.EndOfStream)
	{
		IEnumerable<string> line = reader.ReadLine();

		// do stuff
	}
}
```


### CSV Writer

Annotate the target bean with `CSVColumn` attribute.

```Csharp:SampleBean.cs
class SampleBean
{
        public string Ignored { get; set; }

        [CSVColumn(1)] // The number is column order
        public string Column1 { get; set; }

        [CSVColumn(2, Name = "Custom, \"Name")] // Customize header line
        public string Column2 { get; set; }

        [CSVColumn(3)]
        public int MyNumber { get; set; }
}
```

```Csharp:CSVWriterUsage.cs
var bean1 = new SampleBean()
{
	Column1 = "value1",                // Simple string value
	Column2 = "value\nvalue,value\"",  // String value can contain new lines and quotes.
	MyNumber = 1234,                   // Simple numeric value
};
   
using (var writer = new CSVWriter<SampleBean>(stream, new UTF8Encoding(false)))
{
   writer.WriteHeaderLine(); // Write the header line

   writer.WriteLine(bean1);
   // do more lines

   writer.Flush();
}
```
