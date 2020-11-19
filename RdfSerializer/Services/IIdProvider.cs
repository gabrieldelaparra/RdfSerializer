using System;
using System.Collections.Generic;
using System.Text;
using RdfSerializer.Extensions;
using VDS.RDF;

namespace RdfSerializer.Services
{
    public interface IIriProvider
    {
        string GetIri(object value);
    }
    public interface IIdProvider
    {
        INode GetId(object value);
    }
    public class GuidIdProvider : IIdProvider
    {
        public string DefaultIri { get; set; } = @"http://example.rdf/";
        public INode GetId(object value) {
            return Guid.NewGuid().ToString().ToUriNode(DefaultIri);
        }
    }

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
