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
            System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.LongTimePattern = "HH:mm:ss";
            System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.TimeSeparator = ":";
            System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat = System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat;

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return json.XmlOrJsonTextToJsonData().ToRDFGraph(defaultIti);
        }
    }
}
