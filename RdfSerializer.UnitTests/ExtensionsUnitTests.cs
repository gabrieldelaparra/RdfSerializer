using System;
using System.Collections.Generic;
using System.Text;
using RdfSerializer.Extensions;
using VDS.RDF;
using Xunit;

namespace RdfSerializer.UnitTests
{
    public class ExtensionsUnitTests
    {
        [Fact]
        public void TestToLiteralNodeInt()
        {
            const int i1 = 0;
            const int i2 = -43;
            const int i3 = int.MaxValue;

            var a1 = i1.ToLiteralNode();
            Assert.Equal(@"0^^http://www.w3.org/2001/XMLSchema#integer", a1.ToString());
            var a2 = i2.ToLiteralNode();
            Assert.Equal(@"-43^^http://www.w3.org/2001/XMLSchema#integer", a2.ToString());
            var a3 = i3.ToLiteralNode();
            Assert.Equal(@"2147483647^^http://www.w3.org/2001/XMLSchema#integer", a3.ToString());
        }


    }
}
