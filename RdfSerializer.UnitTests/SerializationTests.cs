using System;
using System.Collections.Generic;
using System.Linq;
using ModelToRdf;
using RdfSerializer.Extensions;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;
using VDS.RDF.Writing.Formatting;
using Wororo.Utilities;
using Xunit;

namespace RdfSerializer.UnitTests
{
    public class SerializationTests
    {
        [Fact]
        public void TestSerializeValueInt() {
            int i1 = 2;
            var t1 = i1.ToTriple();
            //var g1 = i1.SerializeRdf();
            Assert.NotNull(t1);
            //Assert.NotEmpty(g1.Triples);
            //Assert.Single(g1.Triples);
            //var triple1 = g1.Triples.FirstOrDefault();
            //Assert.NotNull(triple1);
            //Assert.NotNull(triple1.Subject);
            //Assert.NotNull(triple1.Predicate);
            //Assert.NotNull(triple1.Object);
        }

        [Fact]
        public void TestNewSerializeClassWithId() {
            var testObject = new TestClassWithId()
            {
                Id = 123,
                BoolArray = new bool[0],
                BoolEnumerable = new List<bool>() { true, false },
                BoolList = new List<bool>() { true },
                BoolProperty = false,
                DateTime = new DateTime(2020, 10, 25, 13, 24, 25),
                DoubleProperty = -1.1,
                IntProperty = 0,
                StringProperty = "string value",
                TestEnum1 = TestEnum1.First,
            };
            var triples = testObject.ToTriples().ToList();
            Assert.NotNull(triples);
            Assert.NotEmpty(triples);
            
        }

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
