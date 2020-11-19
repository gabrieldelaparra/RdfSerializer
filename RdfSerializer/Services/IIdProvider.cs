using VDS.RDF;

namespace RdfSerializer.Services
{
    public interface IIdProvider
    {
        INode GetId(object value);
    }
}
