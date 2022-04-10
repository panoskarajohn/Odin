namespace Shared.Mongo;

public interface IMongoDocument
{
    public DateTime LastModified { get; set; }
    public int? ModifiedBy { get; set; }
    public long Version { get; set; }
}