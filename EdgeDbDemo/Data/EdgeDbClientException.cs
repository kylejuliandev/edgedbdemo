using System.Runtime.Serialization;

namespace EdgeDbDemo.Data;

[Serializable]
internal class EdgeDbClientException : Exception
{
    public EdgeDbClientException()
    {
    }

    public EdgeDbClientException(string? message) : base(message)
    {
    }

    public EdgeDbClientException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected EdgeDbClientException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
