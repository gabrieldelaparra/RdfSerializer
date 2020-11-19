using RdfSerializer.Extensions;
using VDS.RDF;

namespace RdfSerializer.Services
{
    public class IntIdProvider : IIdProvider
    {
        public string DefaultIri { get; set; } = @"http://example.rdf/";
        private int _id;
        public INode GetId(object value)
        {
            return (++_id).ToString().ToUriNode(DefaultIri);
        }
    }
}