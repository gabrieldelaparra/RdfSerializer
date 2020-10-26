using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using ModelToRdf;
using RdfSerializer.Extensions;
using VDS.RDF;


namespace RdfSerializer
{
    public static class RdfSerializer
    {
        public static string DefaultIri { get; set; } = @"http://example.rdf/";
        //private IGraph graph = new Graph();
        public static Graph SerializeRdf(this object obj) => obj.SerializeRdf(DefaultIri);
        public static Graph SerializeRdf(this object obj, string defaultIti)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return json.XmlOrJsonTextToJsonData().ToRDFGraph(defaultIti);
        }

        //private void AddObject(object obj) {
        //    if (obj == null || obj is DBNull) 
        //        ;
        //    else if (obj is string || obj is char)

        //}
//        private void WriteValue(object obj)
//        {
//            if (obj == null || obj is DBNull)
//                _output.Append("null");

//            else if (obj is string || obj is char)
//                WriteString(obj.ToString());

//            else if (obj is Guid)
//                WriteGuid((Guid)obj);

//            else if (obj is bool)
//                _output.Append(((bool)obj) ? "true" : "false"); // conform to standard

//            else if (
//obj is int || obj is long ||
//obj is decimal ||
//obj is byte || obj is short ||
//obj is sbyte || obj is ushort ||
//obj is uint || obj is ulong
//)
//                _output.Append(((IConvertible)obj).ToString(NumberFormatInfo.InvariantInfo));

//            else if (obj is double || obj is Double)
//            {
//                double d = (double)obj;
//                if (double.IsNaN(d))
//                    _output.Append("\"NaN\"");
//                else if (double.IsInfinity(d))
//                {
//                    _output.Append('\"');
//                    _output.Append(((IConvertible)obj).ToString(NumberFormatInfo.InvariantInfo));
//                    _output.Append('\"');
//                }
//                else
//                    _output.Append(((IConvertible)obj).ToString(NumberFormatInfo.InvariantInfo));
//            }
//            else if (obj is float || obj is Single)
//            {
//                float d = (float)obj;
//                if (float.IsNaN(d))
//                    _output.Append("\"NaN\"");
//                else if (float.IsInfinity(d))
//                {
//                    _output.Append('\"');
//                    _output.Append(((IConvertible)obj).ToString(NumberFormatInfo.InvariantInfo));
//                    _output.Append('\"');
//                }
//                else
//                    _output.Append(((IConvertible)obj).ToString(NumberFormatInfo.InvariantInfo));
//            }

//            else if (obj is DateTime)
//                WriteDateTime((DateTime)obj);

//            else if (obj is DateTimeOffset)
//                WriteDateTimeOffset((DateTimeOffset)obj);

//            else if (obj is TimeSpan)
//                _output.Append(((TimeSpan)obj).Ticks);

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
    }
}
