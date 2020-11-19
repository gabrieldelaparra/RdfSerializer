using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

using VDS.RDF;
using VDS.RDF.Parsing;

namespace RdfSerializer.Extensions
{
    public static class ValueExtensions
    {
        private static readonly NodeFactory NodeFactory = new NodeFactory();
        public static CultureInfo CultureInfo = CultureInfo.InvariantCulture;
        private static string defaultIri = @"http://example.rdf/";

        public static ILiteralNode ToLiteralNode(this bool value) => value.ToLiteral(NodeFactory);
        public static ILiteralNode ToLiteralNode(this byte value) => value.ToLiteral(NodeFactory);
        public static ILiteralNode ToLiteralNode(this char value) => value.ToString().ToLiteral(NodeFactory);
        public static ILiteralNode ToLiteralNode(this DateTime value) => value.ToLiteral(NodeFactory);
        public static ILiteralNode ToLiteralNode(this DateTimeOffset value) => value.ToLiteral(NodeFactory);
        public static ILiteralNode ToLiteralNode(this decimal value) => value.ToLiteral(NodeFactory);
        //TODO: Considers Infinity and NaN ?
        public static ILiteralNode ToLiteralNode(this double value) => value.ToLiteral(NodeFactory);
        //TODO: Considers Infinity and NaN ?
        public static ILiteralNode ToLiteralNode(this float value) => value.ToLiteral(NodeFactory);
        public static ILiteralNode ToLiteralNode(this Guid value) => value.ToString().ToLiteral(NodeFactory);
        public static ILiteralNode ToLiteralNode(this int value) => value.ToLiteral(NodeFactory);
        public static ILiteralNode ToLiteralNode(this long value) => value.ToLiteral(NodeFactory);
        public static ILiteralNode ToLiteralNode(this sbyte value) => value.ToLiteral(NodeFactory);
        public static ILiteralNode ToLiteralNode(this short value) => value.ToLiteral(NodeFactory);
        public static ILiteralNode ToLiteralNode(this string value) => value.ToLiteral(NodeFactory);
        public static ILiteralNode ToLiteralNode(this TimeSpan value) => value.ToLiteral(NodeFactory);
        public static ILiteralNode ToLiteralNode(this uint value) => NodeFactory.CreateLiteralNode(value.ToString(), UriFactory.Create(XmlSpecsHelper.XmlSchemaDataTypeUnsignedInt));
        public static ILiteralNode ToLiteralNode(this ulong value) => NodeFactory.CreateLiteralNode(value.ToString(), UriFactory.Create(XmlSpecsHelper.XmlSchemaDataTypeUnsignedLong));
        public static ILiteralNode ToLiteralNode(this ushort value) => NodeFactory.CreateLiteralNode(value.ToString(), UriFactory.Create(XmlSpecsHelper.XmlSchemaDataTypeUnsignedShort));
        internal static ILiteralNode ToLiteralNode(this object value)
        {
            return value switch
            {
                bool v => v.ToLiteralNode(),
                byte v => v.ToLiteralNode(),
                char v => v.ToLiteralNode(),
                DateTime v => v.ToLiteralNode(),
                DateTimeOffset v => v.ToLiteralNode(),
                decimal v => v.ToLiteralNode(),
                double v => v.ToLiteralNode(),
                float v => v.ToLiteralNode(),
                Guid v => v.ToString().ToLiteralNode(),
                int v => v.ToLiteralNode(),
                long v => v.ToLiteralNode(),
                sbyte v => v.ToLiteralNode(),
                short v => v.ToLiteralNode(),
                string v => v.ToLiteralNode(),
                TimeSpan v => v.ToLiteralNode(),
                uint v => v.ToLiteralNode(),
                ulong v => v.ToLiteralNode(),
                ushort v => v.ToLiteralNode(),
                _ => throw new ArgumentException($"Unsupported type for literal node: {value.GetType()}")
            };
        }

        public static IGraph AddTriple(this IGraph g, INode subject, INode predicate, INode ntObject)
        {
            g.Assert(subject, predicate, ntObject);
            return g;
        }

        public static Uri ToUri(this string value, string iri) => UriFactory.Create($"{iri}{value}");
        public static Uri ToUri(this string value) => UriFactory.Create(value);
        public static IUriNode ToUriNode(this string value) => new NodeFactory().CreateUriNode(value.ToUri());
        public static IUriNode ToUriNode(this string value, string iri) => new NodeFactory().CreateUriNode(value.ToUri(iri));

        public static Triple ToLiteralTriple(this object literalObject, INode nodeId, INode predicate)
        {
            return new Triple(nodeId, predicate, literalObject.ToLiteralNode());
        }

        public static IEnumerable<Triple> ToObjectTriples(this object obj)
        {
            var objProperties = obj.DictionaryFromType();
            var idNode = objProperties.GetIdNode();
            foreach (var objProperty in objProperties)
            {
                var propertyName = objProperty.Key;
                var propertyValue = objProperty.Value;
                if (propertyValue == null)
                    continue;
                if (propertyValue.IsLiteralType())
                    yield return ToLiteralTriple(propertyValue, idNode, propertyName.ToUriNode(defaultIri));
                else {
                    var newBlankNode = NodeFactory.CreateBlankNode();
                    yield return ToLiteralTriple(newBlankNode.ToString(), idNode, propertyName.ToUriNode(defaultIri));
                    foreach (var triple in ToTriples(propertyValue, newBlankNode, "Value".ToUriNode(defaultIri)))
                        yield return triple;
                }

            }
        }

        public static INode GetIdNode(this Dictionary<string, object> objDictionary)
        {
            var hasIdProperty = objDictionary.Keys.Any(x => x.ToLower().Equals("id"));
            if (hasIdProperty == true)
                return objDictionary.FirstOrDefault(x => x.Key.ToLower().Equals("id")).Value.ToString()
                        .ToUriNode(defaultIri);
            return NodeFactory.CreateBlankNode();
        }
        public static bool IsEnumerable(this object obj) => obj is IEnumerable;
        public static bool IsLiteralType(this object obj)
        {
            return obj switch
            {
                bool _ => true,
                byte _ => true,
                char _ => true,
                DateTime _ => true,
                DateTimeOffset _ => true,
                decimal _ => true,
                double _ => true,
                float _ => true,
                Guid _ => true,
                int _ => true,
                long _ => true,
                sbyte _ => true,
                short _ => true,
                string _ => true,
                TimeSpan _ => true,
                uint _ => true,
                ulong _ => true,
                ushort _ => true,
                _ => false
            };
        }

        public static Dictionary<string, object> DictionaryFromType(this object obj)
        {
            if (obj == null) return new Dictionary<string, object>();
            var t = obj.GetType();
            var props = t.GetProperties();
            var dict = new Dictionary<string, object>();
            foreach (var prp in props)
            {
                var value = prp.GetValue(obj, new object[] { });
                dict.Add(prp.Name, value);
            }
            return dict;
        }

        public static IEnumerable<Triple> ToTriples(this object obj, INode nodeId = null, INode predicateNode = null)
        {
            if(nodeId == null)
                nodeId = NodeFactory.CreateBlankNode();
            if (predicateNode == null)
                predicateNode = "predicateNode".ToUriNode(defaultIri);
            if (obj == null || obj is DBNull)
                return Array.Empty<Triple>();
            if (obj.IsLiteralType())
                return new[] { new Triple(nodeId, predicateNode, obj.ToLiteralNode()) };
            if (obj.IsEnumerable())
                foreach (var item in (IEnumerable)obj)
                    return item.ToTriples(nodeId, predicateNode);
            else
                return obj.ToObjectTriples();
            return Array.Empty<Triple>();
        }

        public static Triple ToTriple(this object obj)
        {
            if (obj == null || obj is DBNull)
                return null;

            if (obj.IsLiteralType())
            {
                return new Triple(NodeFactory.CreateBlankNode(), "Value".ToUriNode(defaultIri), obj.ToLiteralNode());
            }




            //obj.getId();

            //If object is value
            //If object is class
            //ifs others
            return new Triple(NodeFactory.CreateBlankNode(), "Value".ToUriNode(defaultIri), obj.ToLiteralNode());
        }
    }
}
