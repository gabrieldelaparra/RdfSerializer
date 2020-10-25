# RDFSerializer

Library is a WIP. It converts an object to json and uses the functionality of https://github.com/gabrieldelaparra/ModelToRdf to convert it to an RDF graph. 
Depends on dotnetRDDF for all that has to do with RDF.
I has several drawbacks, mostly the ones lister in ModelToRdf. There is use case example in the tests: 

## Usage
Given a class such as:
``` csharp 
public class TestClassWithId
{
    public int Id { get; set; }
    public string StringProperty { get; set; }
    public bool BoolProperty { get; set; }
    public int IntProperty { get; set; }
    public double DoubleProperty { get; set; }
    public DateTime DateTimeProperty { get; set; }
    public IEnumerable<int> IntEnumerable { get; set; }      
}
```

The instance can be converted like this:
``` csharp
var testObject = new TestClassWithId() {
    //instantiate object
};
var graph = testObject.SerializeRdf(@"http://example.rdf/");
//The contents of the graph can be enumerated like this:
var nTriplesFormatter = new NTriplesFormatter();
var triples = graph.Triples.Select(x => x.ToString(nTriplesFormatter))

//The RDF graph can be saved like this:
var writer = new NTriplesWriter() { SortTriples = true };
writer.Save(graph, @"graph.nt");
```

The result is something like this:
``` rdf
<http://example.rdf/123> <http://example.rdf/Id> "123" .
<http://example.rdf/123> <http://example.rdf/BoolProperty> "False" .
<http://example.rdf/123> <http://example.rdf/DateTimeProperty> "2020-10-25 13:24:25" .
<http://example.rdf/123> <http://example.rdf/DoubleProperty> "-1.1" .
<http://example.rdf/123> <http://example.rdf/IntEnumerable> "1" .
<http://example.rdf/123> <http://example.rdf/IntEnumerable> "2" .
<http://example.rdf/123> <http://example.rdf/IntProperty> "3" .
<http://example.rdf/123> <http://example.rdf/StringProperty> "string value" .
```

## Limitations
- Data types are not handled correctly, they are all converted to strings.
- If no Id is given, a new GUID is generated as the Id. It would be nice to be able to specify an attribute for the serialization (also a custom serializer would be good to have).
- Many more, I have just developed some basic functionality for my needs.
