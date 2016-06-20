using System;
using CrossQuery.Mapper.Extensions;
using CrossQuery.Mapper.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CrossQuery.Mapper.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Initialize()
        {
            Mapper.ClearConfiguration();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void MapperClearConfiguration()
        {
            Mapper.CreateConfiguration<MockSource, MockDestination>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp)
                .AddMap(s => s.DateTimeProp, d => d.DateTimeProp)
                .AddMap(s => s.DoubleProp, d => d.DoubleProp);

            var source = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456
            };

            Mapper.ClearConfiguration();

            var result = Mapper.Map<MockSource, MockDestination>(source);
        }

        [TestMethod]
        public void MapSourceToDestination_AllOk()
        {
            Mapper.CreateConfiguration<MockSource, MockDestination>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp)
                .AddMap(s => s.DateTimeProp, d => d.DateTimeProp)
                .AddMap(s => s.DoubleProp, d => d.DoubleProp);

            var source = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456
            };

            var result = Mapper.Map<MockSource, MockDestination>(source);

            Assert.AreEqual(source.GuidProp, result.GuidProp, "GuidProp error.");
            Assert.AreEqual(source.StringProp, result.StringProp, "StringProp error.");
            Assert.AreEqual(source.IntProp, result.IntProp, "IntProp error.");
            Assert.AreEqual(source.DateTimeProp, result.DateTimeProp, "DateTimeProp error.");
            Assert.AreEqual(source.DoubleProp, result.DoubleProp, "DoubleProp error.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void MapperForSourceIsImplemented_Exception()
        {
            Mapper.CreateConfiguration<MockSource, MockDestination>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp)
                .AddMap(s => s.DateTimeProp, d => d.DateTimeProp)
                .AddMap(s => s.DoubleProp, d => d.DoubleProp);

            var source = new MockSource1()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234
            };

            var result = Mapper.Map<MockSource1, MockDestination>(source);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void MapperForDestinationIsImplemented_Exception()
        {
            Mapper.CreateConfiguration<MockSource, MockDestination>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp)
                .AddMap(s => s.DateTimeProp, d => d.DateTimeProp)
                .AddMap(s => s.DoubleProp, d => d.DoubleProp);

            var source = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456
            };

            var result = Mapper.Map<MockSource, MockDestination1>(source);
        }

        [TestMethod]
        public void MapperForPropertyIsImplemented_PropertyHasDefultValue()
        {
            Mapper.CreateConfiguration<MockSource, MockDestination>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp)
                .AddMap(s => s.DateTimeProp, d => d.DateTimeProp);

            var source = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456
            };

            var result = Mapper.Map<MockSource, MockDestination>(source);

            Assert.AreEqual(default(double), result.DoubleProp);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MapperInputReadOnlyProperty()
        {
            Mapper.CreateConfiguration<MockSource, MockDestination>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp)
                .AddMap(s => s.DateTimeProp, d => d.DateTimeProp)
                .AddMap(s => s.DoubleProp, d => d.DoubleProp)
                .AddMap(s => s.StringProp1, d => d.ReadOnlyProp);

            var source = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456,
                StringProp1 = "forReadOnly"
            };

            var result = Mapper.Map<MockSource, MockDestination>(source);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MapperInputMethod()
        {
            Mapper.CreateConfiguration<MockSource, MockDestination>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp)
                .AddMap(s => s.DateTimeProp, d => d.DateTimeProp)
                .AddMap(s => s.DoubleProp, d => d.DoubleProp)
                .AddMap(s => s.StringProp1, d => d.GetString());

            var source = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456,
                StringProp1 = "forReadOnly"
            };

            var result = Mapper.Map<MockSource, MockDestination>(source);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MapperForPropertyMissingCast()
        {
            Mapper.CreateConfiguration<MockSource, MockDestination>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.IntProp, d => d.StringProp);

            var source = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                IntProp = 1234
            };

            var result = Mapper.Map<MockSource, MockDestination>(source);
        }
    }
}
