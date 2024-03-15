using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;

namespace Moblers.InterviewAssesment.Providers;

public class LowLatencyCompressorProvider : ICompressionProvider
{
    public Stream CreateStream(Stream outputStream)
    {
        return new LowLatencyStreamCompressor(outputStream, CompressionMode.Compress);
    }

    public string EncodingName  => "LowLatency";
    public bool SupportsFlush => true;
}

public class LowLatencyStreamCompressor : Stream
{
    private readonly Stream _compressionStream;

    public LowLatencyStreamCompressor(Stream outputStream, CompressionMode compressionMode)
    {
        _compressionStream = new DeflateStream(outputStream, compressionMode);
    }
    
    public override void Flush()
    {
        throw new NotImplementedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotImplementedException();
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _compressionStream.Write(buffer, offset, count);
    }

    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanWrite { get; }
    public override long Length { get; }
    public override long Position { get; set; }
}