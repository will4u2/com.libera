using System.Data;

namespace com.libera.core
{
    public interface IRequestObject
    {
        long RequestorId { get; set; }
        string SqlStatement { get; set; }
        CommandType CommandType { get; set; }
        IDbTransaction Transaction { get; set; }
        int? Timeout { get; set; }
        bool Buffered { get; set; }
        object Data { get; set; }
    }
}
