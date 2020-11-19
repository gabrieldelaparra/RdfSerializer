using System;
using RdfSerializer.Extensions;
using VDS.RDF;

namespace RdfSerializer.Services
{
    public class GuidIdProvider : IIdProvider
    {
        public string DefaultIri { get; set; } = @"http://example.rdf/";
        public INode GetId(object value) {
            return Guid.NewGuid().ToString().ToUriNode(DefaultIri);
        }
    }
}