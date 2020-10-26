using System;
using System.Collections.Generic;
using System.Globalization;
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

        public static Triple ToTriple(this object obj)
        {
            if (obj == null || obj is DBNull) return null;
            return new Triple(NodeFactory.CreateBlankNode(), "Value".ToUriNode(defaultIri), obj.ToLiteralNode());
        }

        //}
        //        private void WriteValue(object obj)
        //        {


        //#if NET4
        //            else if (_params.KVStyleStringDictionary == false &&
        //                obj is IEnumerable<KeyValuePair<string, object>>)

        //                WriteStringDictionary((IEnumerable<KeyValuePair<string, object>>)obj);
        //#endif

        //            else if (_params.KVStyleStringDictionary == false && obj is IDictionary &&
        //                obj.GetType().IsGenericType && Reflection.Instance.GetGenericArguments(obj.GetType())[0] == typeof(string))

        //                WriteStringDictionary((IDictionary)obj);
        //            else if (obj is IDictionary)
        //                WriteDictionary((IDictionary)obj);
        //#if !SILVERLIGHT
        //            else if (obj is DataSet)
        //                WriteDataset((DataSet)obj);

        //            else if (obj is DataTable)
        //                this.WriteDataTable((DataTable)obj);
        //#endif
        //            else if (obj is byte[])
        //                WriteBytes((byte[])obj);

        //            else if (obj is StringDictionary)
        //                WriteSD((StringDictionary)obj);

        //            else if (obj is NameValueCollection)
        //                WriteNV((NameValueCollection)obj);

        //            else if (obj is Array)
        //                WriteArrayRanked((Array)obj);

        //            else if (obj is IEnumerable)
        //                WriteArray((IEnumerable)obj);

        //            else if (obj is Enum)
        //                WriteEnum((Enum)obj);

        //            //else if (Reflection.Instance.IsTypeRegistered(obj.GetType()))
        //            //    WriteCustom(obj);

        //            else
        //                WriteObject(obj);
        //        }

        //public static void 

        //public static Uri ToUri(this string value, string iri) => UriFactory.Create($"{iri}{value}");
        //public static Uri ToUri(this int value, string iri) => UriFactory.Create($"{iri}{value}");
        //public static Uri ToUri(this string value) => UriFactory.Create(value);
        //public static Uri ToUri(this int value) => UriFactory.Create(value.ToString());
    }
}
