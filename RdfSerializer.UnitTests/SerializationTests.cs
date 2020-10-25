using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;
using VDS.RDF.Writing.Formatting;
using Xunit;

namespace RdfSerializer.UnitTests
{
    public class TestClassWithId
    {
        public int Id { get; set; }
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
        public double DoubleProperty { get; set; }
        public DateTime DateTime { get; set; }
        public bool BoolProperty { get; set; }
        public IEnumerable<bool> BoolEnumerable { get; set; }
        public List<bool> BoolList { get; set; }
        public bool[] BoolArray { get; set; }
        public TestEnum1 TestEnum1 { get; set; }
        public string NullString { get; set; }
    }

    public enum TestEnum1
    {
        First,
        Second,
    }
    public class SerializationTests
    {
        [Fact]
        public void TestSerializeClassWithId() {
            var testObject = new TestClassWithId() {
                Id = 123,
                BoolArray = new bool[0],
                BoolEnumerable = new List<bool>() { true, false },
                BoolList = new List<bool>() { true },
                BoolProperty = false,
                DateTime = new DateTime(2020,10,25,13,24,25),
                DoubleProperty = -1.1,
                IntProperty = 0,
                StringProperty = "string value",
                TestEnum1 = TestEnum1.First,
            };
            var graph = testObject.SerializeRdf();
            Assert.NotNull(graph);
            var nTriplesFormatter = new NTriplesFormatter(NTriplesSyntax.Rdf11);
            var actual = graph.Triples.Select(x => x.ToString(nTriplesFormatter)).ToList();

            var expected = new List<string>() {
                "<http://example.rdf/123> <http://example.rdf/Id> \"123\" .",
                "<http://example.rdf/123> <http://example.rdf/StringProperty> \"string value\" .",
                "<http://example.rdf/123> <http://example.rdf/DoubleProperty> \"-1.1\" .",
                "<http://example.rdf/123> <http://example.rdf/DateTime> \"2020-10-25 13:24:25\" .",
                "<http://example.rdf/123> <http://example.rdf/BoolProperty> \"False\" .",
                "<http://example.rdf/123> <http://example.rdf/BoolEnumerable> \"True\" .",
                "<http://example.rdf/123> <http://example.rdf/BoolEnumerable> \"False\" .",
                //"<http://example.rdf/123> <http://example.rdf/BoolEnumerable> \"True\" .",
                "<http://example.rdf/123> <http://example.rdf/BoolList> \"True\" .",
            };

            //TODO: What about the Enum?
            //TODO: What about the NullString?
            //TODO: Fails when a collection (e.g.:BoolEnumerable) has 2 times true; It only shows once.
            Assert.Equal(expected.Count, actual.Count);
            Assert.False(expected.Except(actual).Any());
        }
    }
}
