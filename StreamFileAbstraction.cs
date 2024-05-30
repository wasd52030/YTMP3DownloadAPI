public class StreamFileAbstraction : TagLib.File.IFileAbstraction
{
    public StreamFileAbstraction(string name, Stream stream)
    {
        WriteStream = stream;
        ReadStream = stream;
        Name = name;
    }

    public string Name { get; private set; }

    public Stream ReadStream { get; private set; }

    public Stream WriteStream { get; private set; }

    public void CloseStream(Stream stream)
    {
        stream.Position = 0;
    }
}