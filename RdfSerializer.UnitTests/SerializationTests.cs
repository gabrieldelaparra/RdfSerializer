using System;
using System.Collections.Generic;
using System.Linq;
using ModelToRdf;
using RdfSerializer.Extensions;
using RdfSerializer.Services;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;
using VDS.RDF.Writing.Formatting;
using Wororo.Utilities;
using Xunit;

namespace RdfSerializer.UnitTests
{
    public class SerializationTests
    {
        public SerializationTests() {
            Extensions.ValueExtensions._idProvider = new IntIdProvider();
        }
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

            var actual = triples.ToNTriples11().ToList();

            var expected = new List<string>() {
                "<http://example.rdf/123> <http://example.rdf/TestEnum1> \"0\" .",
                "<http://example.rdf/123> <http://example.rdf/DateTime> \"2020-10-25 13:24:25\" .",
                "<http://example.rdf/123> <http://example.rdf/BoolProperty> \"False\" .",
                "<http://example.rdf/123> <http://example.rdf/BoolEnumerable> \"True\" .",
                "<http://example.rdf/123> <http://example.rdf/BoolEnumerable> \"False\" .",
                "<http://example.rdf/123> <http://example.rdf/BoolList> \"True\" .",
                "<http://example.rdf/123> <http://example.rdf/NullString> \"\" .",

                "<http://example.rdf/1> <http://example.rdf/Id> \"123\"^^<http://www.w3.org/2001/XMLSchema#integer> .",
                "<http://example.rdf/1> <http://example.rdf/StringProperty> \"string value\"^^<http://www.w3.org/2001/XMLSchema#string> .",
                "<http://example.rdf/1> <http://example.rdf/IntProperty> \"0\"^^<http://www.w3.org/2001/XMLSchema#integer> .",
                "<http://example.rdf/1> <http://example.rdf/DoubleProperty> \"-1.1\"^^<http://www.w3.org/2001/XMLSchema#double> .",
                "<http://example.rdf/1> <http://example.rdf/DateTime> \"2020-10-25T13:24:25.000000\"^^<http://www.w3.org/2001/XMLSchema#dateTime> .",
                "<http://example.rdf/1> <http://example.rdf/BoolProperty> \"false\"^^<http://www.w3.org/2001/XMLSchema#boolean> .",
                "<http://example.rdf/1> <http://example.rdf/BoolEnumerable> <http://example.rdf/2> .",
                "<http://example.rdf/1> <http://example.rdf/BoolEnumerable> <http://example.rdf/3> .",
                "<http://example.rdf/1> <http://example.rdf/BoolList> <http://example.rdf/4> .",
                "<http://example.rdf/1> <http://example.rdf/TestEnum1> <http://example.rdf/5> .",

            };

            foreach (var item in expected)
            {
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

        [Fact]
        public void TestNewSerializeOuterClassWithIdAndInnerClassNull()
        {
            var testObject = new OuterClassWithIdAndInnerClass()
            {
                Id = 123,
                InnerClassItem = null
            };
            var triples = testObject.ToTriples().ToList();
            Assert.NotNull(triples);
            Assert.NotEmpty(triples);

            var actual = triples.ToNTriples11().ToList();

            var expected = new List<string>() {
                "<http://example.rdf/1> <http://example.rdf/Id> \"123\"^^<http://www.w3.org/2001/XMLSchema#integer> .",
            };

            foreach (var item in expected)
            {
                Assert.Contains(item, actual);
            }

            foreach (var item in actual)
            {
                Assert.Contains(item, expected);
            }

            Assert.Equal(expected.Count, actual.Count());
            Assert.Empty(expected.Except(actual));
        }

        [Fact]
        public void TestNewSerializeOuterClassWithIdAndInnerClassValue()
        {
            var testObject = new OuterClassWithIdAndInnerClass()
            {
                Id = 123,
                InnerClassItem = new InnerClassWithId() {
                    Id = 456,
                    BoolValue = false,
                }
            };
            var triples = testObject.ToTriples().ToList();
            Assert.NotNull(triples);
            Assert.NotEmpty(triples);

            var actual = triples.ToNTriples11().ToList();

            var expected = new List<string>() {
                "<http://example.rdf/1> <http://example.rdf/Id> \"123\"^^<http://www.w3.org/2001/XMLSchema#integer> .",
                "<http://example.rdf/1> <http://example.rdf/InnerClassItem> <http://example.rdf/2> .",
                "<http://example.rdf/2> <http://example.rdf/Id> \"456\"^^<http://www.w3.org/2001/XMLSchema#integer> .",
                "<http://example.rdf/2> <http://example.rdf/BoolValue> \"false\"^^<http://www.w3.org/2001/XMLSchema#boolean> .",
            };

            foreach (var item in expected)
            {
                Assert.Contains(item, actual);
            }

            foreach (var item in actual)
            {
                Assert.Contains(item, expected);
            }

            Assert.Equal(expected.Count, actual.Count());
            Assert.Empty(expected.Except(actual));
        }

        [Fact]
        public void TestNewSerializeOuterClassWithIdAndInnerClassListNull()
        {
            var testObject = new OuterClassWithIdAndInnerClassList()
            {
                Id = 123,
                InnerClassList = null,
            };
            var triples = testObject.ToTriples().ToList();
            Assert.NotNull(triples);
            Assert.NotEmpty(triples);

            var actual = triples.ToNTriples11().ToList();

            var expected = new List<string>() {
                "<http://example.rdf/1> <http://example.rdf/Id> \"123\"^^<http://www.w3.org/2001/XMLSchema#integer> .",
            };

            foreach (var item in expected)
            {
                Assert.Contains(item, actual);
            }

            foreach (var item in actual)
            {
                Assert.Contains(item, expected);
            }

            Assert.Equal(expected.Count, actual.Count());
            Assert.Empty(expected.Except(actual));
        }

        [Fact]
        public void TestNewSerializeOuterClassWithIdAndInnerClassListValueEmpty()
        {
            var testObject = new OuterClassWithIdAndInnerClassList()
            {
                Id = 123,
                InnerClassList = new List<InnerClassWithId>(),
            };
            var triples = testObject.ToTriples().ToList();
            Assert.NotNull(triples);
            Assert.NotEmpty(triples);

            var actual = triples.ToNTriples11().ToList();

            var expected = new List<string>() {
                "<http://example.rdf/1> <http://example.rdf/Id> \"123\"^^<http://www.w3.org/2001/XMLSchema#integer> .",
            };

            foreach (var item in expected)
            {
                Assert.Contains(item, actual);
            }

            foreach (var item in actual)
            {
                Assert.Contains(item, expected);
            }

            Assert.Equal(expected.Count, actual.Count());
            Assert.Empty(expected.Except(actual));
        }

        [Fact]
        public void TestNewSerializeOuterClassWithIdAndInnerClassListValueOne()
        {
            var testObject = new OuterClassWithIdAndInnerClassList()
            {
                Id = 123,
                InnerClassList = new List<InnerClassWithId>() {
                    new InnerClassWithId() {
                        Id = 1,
                        BoolValue = false,
                    }
                },
            };
            var triples = testObject.ToTriples().ToList();
            Assert.NotNull(triples);
            Assert.NotEmpty(triples);

            var actual = triples.ToNTriples11().ToList();

            var expected = new List<string>() {
                "<http://example.rdf/1> <http://example.rdf/Id> \"123\"^^<http://www.w3.org/2001/XMLSchema#integer> .",
                "<http://example.rdf/1> <http://example.rdf/InnerClassList> <http://example.rdf/2> .",
                "<http://example.rdf/2> <http://example.rdf/Id> \"1\"^^<http://www.w3.org/2001/XMLSchema#integer> .",
                "<http://example.rdf/2> <http://example.rdf/BoolValue> \"false\"^^<http://www.w3.org/2001/XMLSchema#boolean> .",
            };

            foreach (var item in expected)
            {
                Assert.Contains(item, actual);
            }

            foreach (var item in actual)
            {
                Assert.Contains(item, expected);
            }

            Assert.Equal(expected.Count, actual.Count());
            Assert.Empty(expected.Except(actual));
        }

        [Fact]
        public void TestNewSerializeOuterClassWithIdAndInnerClassListValueTwo()
        {
            var testObject = new OuterClassWithIdAndInnerClassList()
            {
                Id = 123,
                InnerClassList = new List<InnerClassWithId>() {
                    new InnerClassWithId() {
                        Id = 1,
                        BoolValue = false,
                    },
                    new InnerClassWithId() {
                        Id = 2,
                        BoolValue = true,
                    }
                },
            };
            var triples = testObject.ToTriples().ToList();
            Assert.NotNull(triples);
            Assert.NotEmpty(triples);

            var actual = triples.ToNTriples11().ToList();

            var expected = new List<string>() {
                "<http://example.rdf/1> <http://example.rdf/Id> \"123\"^^<http://www.w3.org/2001/XMLSchema#integer> .",
                "<http://example.rdf/1> <http://example.rdf/InnerClassList> <http://example.rdf/2> .",
                "<http://example.rdf/1> <http://example.rdf/InnerClassList> <http://example.rdf/3> .",
                "<http://example.rdf/2> <http://example.rdf/Id> \"1\"^^<http://www.w3.org/2001/XMLSchema#integer> .",
                "<http://example.rdf/2> <http://example.rdf/BoolValue> \"false\"^^<http://www.w3.org/2001/XMLSchema#boolean> .",
                "<http://example.rdf/3> <http://example.rdf/Id> \"2\"^^<http://www.w3.org/2001/XMLSchema#integer> .",
                "<http://example.rdf/3> <http://example.rdf/BoolValue> \"true\"^^<http://www.w3.org/2001/XMLSchema#boolean> .",
            };

            foreach (var item in expected)
            {
                Assert.Contains(item, actual);
            }

            foreach (var item in actual)
            {
                Assert.Contains(item, expected);
            }

            Assert.Equal(expected.Count, actual.Count());
            Assert.Empty(expected.Except(actual));
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
            var actual = graph.ToNTriples11();

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
