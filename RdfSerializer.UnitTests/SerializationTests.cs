using System;
using System.Collections.Generic;
using System.Linq;
using ModelToRdf;
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
            var actual = graph.GraphToNTriples();

            var expected = new List<string>() {
                "<http://example.rdf/123> <http://example.rdf/Id> \"123\" .",
                "<http://example.rdf/123> <http://example.rdf/StringProperty> \"string value\" .",
                "<http://example.rdf/123> <http://example.rdf/DoubleProperty> \"-1.1\" .",
                "<http://example.rdf/123> <http://example.rdf/IntProperty> \"0\" .",
                "<http://example.rdf/123> <http://example.rdf/TestEnum1> \"0\" .",
                "<http://example.rdf/123> <http://example.rdf/DateTime> \"2020-10-25 13:24:25\" .",
                "<http://example.rdf/123> <http://example.rdf/BoolProperty> \"False\" .",
                "<http://example.rdf/123> <http://example.rdf/BoolEnumerable> \"True\" .",
                "<http://example.rdf/123> <http://example.rdf/BoolEnumerable> \"False\" .",
                "<http://example.rdf/123> <http://example.rdf/BoolList> \"True\" .",
                "<http://example.rdf/123> <http://example.rdf/NullString> \"\" .",
            };

            foreach (var item in expected) {
                Assert.Contains(item, actual);
            }

            foreach (var item in actual)
            {
                Assert.Contains(item, expected);
            }

            //TODO: What about the Enum?
            //TODO: What about the NullString?
            //TODO: Fails when a collection (e.g.:BoolEnumerable) has 2 times true; It only shows one
            //TODO: DateTime ToString is different in Windows and Linux.
            Assert.Equal(expected.Count, actual.Count());
            Assert.Empty(expected.Except(actual));
        }
    }
}
