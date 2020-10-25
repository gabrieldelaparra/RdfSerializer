using ModelToRdf;
using VDS.RDF;


namespace RdfSerializer
{
    public static class RdfSerializer
    {
        public static string DefaultIri { get; set; } = @"http://example.rdf/";

        public static Graph SerializeRdf(this object obj) => obj.SerializeRdf(DefaultIri);
        public static Graph SerializeRdf(this object obj, string defaultIti)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return json.XmlOrJsonTextToJsonData().ToRDFGraph(defaultIti);
        }
    }
}
