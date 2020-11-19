using System.Linq;
using RdfSerializer.Extensions;
using VDS.RDF;

namespace RdfSerializer.Services
{
    public class InternalIdProvider : IIdProvider
    {
        public string DefaultIri { get; set; } = @"http://example.rdf/";
        public INode GetId(object value) {

            var objDictionary = value.DictionaryFromType();

            var hasIdProperty = objDictionary.Keys.Any(x => x.ToLower().Equals("id"));
            if (hasIdProperty == true)
                return objDictionary.FirstOrDefault(x => x.Key.ToLower().Equals("id")).Value.ToString().ToUriNode(DefaultIri);
            return new NodeFactory().CreateBlankNode();
        }
    }
}