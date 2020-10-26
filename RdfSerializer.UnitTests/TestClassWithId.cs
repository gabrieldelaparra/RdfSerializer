using System;
using System.Collections.Generic;

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
}