using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CrossQuery.Mapper.Extensions;
using CrossQuery.Mapper.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CrossQuery.Mapper.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void MapSourceToDestination_AllOk()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource, MockDestination>()
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

            var result = mapper.Map<MockSource, MockDestination>(source);

            Assert.AreEqual(source.GuidProp, result.GuidProp, "GuidProp error.");
            Assert.AreEqual(source.StringProp, result.StringProp, "StringProp error.");
            Assert.AreEqual(source.IntProp, result.IntProp, "IntProp error.");
            Assert.AreEqual(source.DateTimeProp, result.DateTimeProp, "DateTimeProp error.");
            Assert.AreEqual(source.DoubleProp, result.DoubleProp, "DoubleProp error.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void MapperForSourceIsNotImplemented_Exception()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource, MockDestination>()
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

            var result = mapper.Map<MockSource1, MockDestination>(source);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void MapperForDestinationIsNotImplemented_Exception()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource, MockDestination>()
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

            var result = mapper.Map<MockSource, MockDestination1>(source);
        }

        [TestMethod]
        public void MapperForPropertyIsImplemented_PropertyHasDefultValue()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource, MockDestination>()
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

            var result = mapper.Map<MockSource, MockDestination>(source);

            Assert.AreEqual(default(double), result.DoubleProp);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MapperInputReadOnlyProperty()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource, MockDestination>()
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

            var result = mapper.Map<MockSource, MockDestination>(source);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MapperInputMethod()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource, MockDestination>()
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

            var result = mapper.Map<MockSource, MockDestination>(source);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MapperForPropertyMissingCast()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource, MockDestination>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.IntProp, d => d.StringProp);

            var source = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                IntProp = 1234
            };

            var result = mapper.Map<MockSource, MockDestination>(source);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void MapReferenceProperty_MapperIsNotImplemented()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource, MockDestination>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp)
                .AddMap(s => s.DateTimeProp, d => d.DateTimeProp)
                .AddMap(s => s.DoubleProp, d => d.DoubleProp)
                .AddMap(s => s.ReferenceProperty, d => d.ReferenceProperty);

            var source = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456,
                ReferenceProperty = new MockSource1()
                {
                    GuidProp = Guid.NewGuid(),
                    StringProp = "anyRefString",
                    IntProp = 456
                }
            };

            var result = mapper.Map<MockSource, MockDestination>(source);
        }

        [TestMethod]
        public void MapReferenceProperty_MapperImplemented_AllOk()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource1, MockDestination1>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp);

            mapper.CreateConfiguration<MockSource, MockDestination>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp)
                .AddMap(s => s.DateTimeProp, d => d.DateTimeProp)
                .AddMap(s => s.DoubleProp, d => d.DoubleProp)
                .AddMap(s => s.ReferenceProperty, d => d.ReferenceProperty);       

            var source = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456,
                ReferenceProperty = new MockSource1()
                {
                    GuidProp = Guid.NewGuid(),
                    StringProp = "anyRefString",
                    IntProp = 456
                }
            };

            var result = mapper.Map<MockSource, MockDestination>(source);

            Assert.AreEqual(source.GuidProp, result.GuidProp, "GuidProp error.");
            Assert.AreEqual(source.StringProp, result.StringProp, "StringProp error.");
            Assert.AreEqual(source.IntProp, result.IntProp, "IntProp error.");
            Assert.AreEqual(source.DateTimeProp, result.DateTimeProp, "DateTimeProp error.");
            Assert.AreEqual(source.DoubleProp, result.DoubleProp, "DoubleProp error.");

            Assert.AreEqual(source.ReferenceProperty.GuidProp, result.ReferenceProperty.GuidProp, "ReferenceProperty.GuidProp error.");
            Assert.AreEqual(source.ReferenceProperty.StringProp, result.ReferenceProperty.StringProp, "ReferenceProperty.StringProp error.");
            Assert.AreEqual(source.ReferenceProperty.IntProp, result.ReferenceProperty.IntProp, "ReferenceProperty.IntProp error.");    
        }

        [TestMethod]
        public void MapCollectionWithReferenceProperty_MapperImplemented_AllOk()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource1, MockDestination1>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp);

            mapper.CreateConfiguration<MockSource, MockDestination>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp)
                .AddMap(s => s.DateTimeProp, d => d.DateTimeProp)
                .AddMap(s => s.DoubleProp, d => d.DoubleProp)
                .AddMap(s => s.ReferenceProperty, d => d.ReferenceProperty);

            var source1 = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456,
                ReferenceProperty = new MockSource1()
                {
                    GuidProp = Guid.NewGuid(),
                    StringProp = "anyRefString",
                    IntProp = 456
                }
            };

            var source2 = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456,
                ReferenceProperty = new MockSource1()
                {
                    GuidProp = Guid.NewGuid(),
                    StringProp = "anyRefString",
                    IntProp = 456
                }
            };

            var result = mapper.Map<MockSource, MockDestination>((new List<MockSource>() { source1, source2 }).AsQueryable());

            Assert.AreEqual(2, result.Count(), "Count error.");

            var destination = result.FirstOrDefault(d => d.GuidProp == source1.GuidProp);

            Assert.IsNotNull(destination, "destination1 is null.");
            Assert.AreEqual(source1.StringProp, destination.StringProp, "source1 StringProp error.");
            Assert.AreEqual(source1.IntProp, destination.IntProp, "source1 IntProp error.");
            Assert.AreEqual(source1.DateTimeProp, destination.DateTimeProp, "source1 DateTimeProp error.");
            Assert.AreEqual(source1.DoubleProp, destination.DoubleProp, "source1 DoubleProp error.");

            Assert.AreEqual(source1.ReferenceProperty.GuidProp, destination.ReferenceProperty.GuidProp, "source1 ReferenceProperty.GuidProp error.");
            Assert.AreEqual(source1.ReferenceProperty.StringProp, destination.ReferenceProperty.StringProp, "source1 ReferenceProperty.StringProp error.");
            Assert.AreEqual(source1.ReferenceProperty.IntProp, destination.ReferenceProperty.IntProp, "source1 ReferenceProperty.IntProp error.");

            destination = result.FirstOrDefault(d => d.GuidProp == source2.GuidProp);

            Assert.IsNotNull(destination, "destination2 is null.");
            Assert.AreEqual(source2.StringProp, destination.StringProp, "source2 StringProp error.");
            Assert.AreEqual(source2.IntProp, destination.IntProp, "source2 IntProp error.");
            Assert.AreEqual(source2.DateTimeProp, destination.DateTimeProp, "source2 DateTimeProp error.");
            Assert.AreEqual(source2.DoubleProp, destination.DoubleProp, "source2 DoubleProp error.");

            Assert.AreEqual(source2.ReferenceProperty.GuidProp, destination.ReferenceProperty.GuidProp, "source2 ReferenceProperty.GuidProp error.");
            Assert.AreEqual(source2.ReferenceProperty.StringProp, destination.ReferenceProperty.StringProp, "source2 ReferenceProperty.StringProp error.");
            Assert.AreEqual(source2.ReferenceProperty.IntProp, destination.ReferenceProperty.IntProp, "source2 ReferenceProperty.IntProp error.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void MapCollectionWithReferenceProperty_MapperIsNotImplemented()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource, MockDestination>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp)
                .AddMap(s => s.DateTimeProp, d => d.DateTimeProp)
                .AddMap(s => s.DoubleProp, d => d.DoubleProp)
                .AddMap(s => s.ReferenceProperty, d => d.ReferenceProperty);

            var source1 = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456,
                ReferenceProperty = new MockSource1()
                {
                    GuidProp = Guid.NewGuid(),
                    StringProp = "anyRefString",
                    IntProp = 456
                }
            };

            var source2 = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456,
                ReferenceProperty = new MockSource1()
                {
                    GuidProp = Guid.NewGuid(),
                    StringProp = "anyRefString",
                    IntProp = 456
                }
            };

            var result = mapper.Map<MockSource, MockDestination>((new List<MockSource>() { source1, source2 }).AsQueryable());
        }

        [TestMethod]
        public void MapSourceWithCollectionAndReferenceProperty_MapperImplemented_AllOk()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource1, MockDestination1>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp);

            mapper.CreateConfiguration<MockSource, MockDestination>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp)
                .AddMap(s => s.DateTimeProp, d => d.DateTimeProp)
                .AddMap(s => s.DoubleProp, d => d.DoubleProp)
                .AddMap(s => s.ReferenceProperty, d => d.ReferenceProperty)
                .AddMap(s => s.CollectionOfReferenceProperties, d => d.CollectionOfReferenceProperties);

            var sourceRef1 = new MockSource1()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anySourceRef1",
                IntProp = 12
            };

            var sourceRef2 = new MockSource1()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anySourceRef2",
                IntProp = 23
            };

            var source = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456,
                ReferenceProperty = new MockSource1()
                {
                    GuidProp = Guid.NewGuid(),
                    StringProp = "anyRefString",
                    IntProp = 456
                },
                CollectionOfReferenceProperties = new List<MockSource1>() { sourceRef1, sourceRef2 }
            };

            var result = mapper.Map<MockSource, MockDestination>((new List<MockSource>() { source }).AsQueryable());           

            var destination = result.FirstOrDefault(d => d.GuidProp == source.GuidProp);

            Assert.IsNotNull(destination, "destination is null.");
            Assert.AreEqual(source.StringProp, destination.StringProp, "source StringProp error.");
            Assert.AreEqual(source.IntProp, destination.IntProp, "source IntProp error.");
            Assert.AreEqual(source.DateTimeProp, destination.DateTimeProp, "source DateTimeProp error.");
            Assert.AreEqual(source.DoubleProp, destination.DoubleProp, "source DoubleProp error.");

            Assert.AreEqual(source.ReferenceProperty.GuidProp, destination.ReferenceProperty.GuidProp, "source ReferenceProperty.GuidProp error.");
            Assert.AreEqual(source.ReferenceProperty.StringProp, destination.ReferenceProperty.StringProp, "source ReferenceProperty.StringProp error.");
            Assert.AreEqual(source.ReferenceProperty.IntProp, destination.ReferenceProperty.IntProp, "source ReferenceProperty.IntProp error.");

            var destinationCollectionElement = destination.CollectionOfReferenceProperties.FirstOrDefault(d => d.GuidProp == sourceRef1.GuidProp);

            Assert.IsNotNull(destinationCollectionElement, "sourceRef1 destination is null.");
            Assert.AreEqual(sourceRef1.StringProp, destinationCollectionElement.StringProp, "sourceRef1 ReferenceProperty.StringProp error.");
            Assert.AreEqual(sourceRef1.IntProp, destinationCollectionElement.IntProp, "sourceRef1 ReferenceProperty.IntProp error.");

            destinationCollectionElement = destination.CollectionOfReferenceProperties.FirstOrDefault(d => d.GuidProp == sourceRef2.GuidProp);

            Assert.IsNotNull(destinationCollectionElement, "sourceRef2 destination is null.");
            Assert.AreEqual(sourceRef2.StringProp, destinationCollectionElement.StringProp, "sourceRef2 ReferenceProperty.StringProp error.");
            Assert.AreEqual(sourceRef2.IntProp, destinationCollectionElement.IntProp, "sourceRef2 ReferenceProperty.IntProp error.");
        }

        [TestMethod]
        public void MapSourceWithCollectionAndReferenceProperty_MapperForCollectionIsNotImplemented()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource1, MockDestination1>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp);

            mapper.CreateConfiguration<MockSource, MockDestination>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp)
                .AddMap(s => s.DateTimeProp, d => d.DateTimeProp)
                .AddMap(s => s.DoubleProp, d => d.DoubleProp)
                .AddMap(s => s.ReferenceProperty, d => d.ReferenceProperty);

            var sourceRef1 = new MockSource1()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anySourceRef1",
                IntProp = 12
            };

            var sourceRef2 = new MockSource1()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anySourceRef2",
                IntProp = 23
            };

            var source = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456,
                ReferenceProperty = new MockSource1()
                {
                    GuidProp = Guid.NewGuid(),
                    StringProp = "anyRefString",
                    IntProp = 456
                },
                CollectionOfReferenceProperties = new List<MockSource1>() { sourceRef1, sourceRef2 }
            };

            var result = mapper.Map<MockSource, MockDestination>((new List<MockSource>() { source }).AsQueryable());

            var destination = result.FirstOrDefault(d => d.GuidProp == source.GuidProp);

            Assert.IsNotNull(destination, "destination is null.");
            Assert.AreEqual(source.StringProp, destination.StringProp, "source StringProp error.");
            Assert.AreEqual(source.IntProp, destination.IntProp, "source IntProp error.");
            Assert.AreEqual(source.DateTimeProp, destination.DateTimeProp, "source DateTimeProp error.");
            Assert.AreEqual(source.DoubleProp, destination.DoubleProp, "source DoubleProp error.");

            Assert.AreEqual(source.ReferenceProperty.GuidProp, destination.ReferenceProperty.GuidProp, "source ReferenceProperty.GuidProp error.");
            Assert.AreEqual(source.ReferenceProperty.StringProp, destination.ReferenceProperty.StringProp, "source ReferenceProperty.StringProp error.");
            Assert.AreEqual(source.ReferenceProperty.IntProp, destination.ReferenceProperty.IntProp, "source ReferenceProperty.IntProp error.");

            Assert.IsNull(destination.CollectionOfReferenceProperties, "CollectionOfReferenceProperties is not null.");
        }

        [TestMethod]
        public void MapSourceToDestination_NonGenericMetod_AllOk()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource, MockDestination>()
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

            var result = (MockDestination)mapper.Map(typeof(MockSource), typeof(MockDestination), source);

            Assert.AreEqual(source.GuidProp, result.GuidProp, "GuidProp error.");
            Assert.AreEqual(source.StringProp, result.StringProp, "StringProp error.");
            Assert.AreEqual(source.IntProp, result.IntProp, "IntProp error.");
            Assert.AreEqual(source.DateTimeProp, result.DateTimeProp, "DateTimeProp error.");
            Assert.AreEqual(source.DoubleProp, result.DoubleProp, "DoubleProp error.");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void MapSourceToDestination_NonGenericMetod_TSourceIsNull()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource, MockDestination>()
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

            var result = (MockDestination)mapper.Map(null, typeof(MockDestination), source);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void MapSourceToDestination_NonGenericMetod_TDestIsNull()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource, MockDestination>()
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

            var result = (MockDestination)mapper.Map(typeof(MockSource), null, source);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MapSourceToDestination_NonGenericMetod_TSourceIsNotClass()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource, MockDestination>()
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

            var result = (MockDestination)mapper.Map(typeof(int), typeof(MockDestination), source);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MapSourceToDestination_NonGenericMetod_TDestIsNotClass()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource, MockDestination>()
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

            var result = (int)mapper.Map(typeof(MockSource), typeof(int), source);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MapSourceToDestination_NonGenericMetod_TDestIsNotImplementDefaulConstructor()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource, MockDestination1>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp);

            var source = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234
            };

            var result = (MockDestinationWithoutDefaultConstructor)mapper.Map(typeof(MockSource), typeof(MockDestinationWithoutDefaultConstructor), source);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void MapSourceToDestination_NonGenericMetod_SourceObjIsNull()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource, MockDestination>()
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

            var result = (MockDestination)mapper.Map(typeof(MockSource), typeof(MockDestination), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MapSourceToDestination_NonGenericMetod_TSourceAndSourceObjHaveDifferentTypes()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource, MockDestination>()
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

            var result = (MockDestination)mapper.Map(typeof(MockSource), typeof(MockDestination), source);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MapSourceToDestination_NonGenericMetod_TSourceIsArrayNonGenericType()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource1, MockDestination1>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp);

            mapper.CreateConfiguration<MockSource, MockDestination>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp)
                .AddMap(s => s.DateTimeProp, d => d.DateTimeProp)
                .AddMap(s => s.DoubleProp, d => d.DoubleProp)
                .AddMap(s => s.ReferenceProperty, d => d.ReferenceProperty);

            var source1 = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456,
                ReferenceProperty = new MockSource1()
                {
                    GuidProp = Guid.NewGuid(),
                    StringProp = "anyRefString",
                    IntProp = 456
                }
            };

            var source2 = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456,
                ReferenceProperty = new MockSource1()
                {
                    GuidProp = Guid.NewGuid(),
                    StringProp = "anyRefString",
                    IntProp = 456
                }
            };

            var result = (IEnumerable)mapper.Map(
                typeof(IEnumerable),
                typeof(IEnumerable),
                (new List<MockSource>() { source1, source2 }).AsQueryable());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MapSourceToDestination_NonGenericMetod_TSourceIsArrayDestinatiniIsNotArray()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource1, MockDestination1>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp);

            mapper.CreateConfiguration<MockSource, MockDestination>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp)
                .AddMap(s => s.DateTimeProp, d => d.DateTimeProp)
                .AddMap(s => s.DoubleProp, d => d.DoubleProp)
                .AddMap(s => s.ReferenceProperty, d => d.ReferenceProperty);

            var source1 = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456,
                ReferenceProperty = new MockSource1()
                {
                    GuidProp = Guid.NewGuid(),
                    StringProp = "anyRefString",
                    IntProp = 456
                }
            };

            var source2 = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456,
                ReferenceProperty = new MockSource1()
                {
                    GuidProp = Guid.NewGuid(),
                    StringProp = "anyRefString",
                    IntProp = 456
                }
            };

            var result = (IEnumerable)mapper.Map(
                typeof(IEnumerable),
                typeof(MockDestination),
                (new List<MockSource>() { source1, source2 }).AsQueryable());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MapSourceToDestination_NonGenericMetod_TSourceIsNotArrayDestinatiniIsArray()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource1, MockDestination1>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp);

            mapper.CreateConfiguration<MockSource, MockDestination>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp)
                .AddMap(s => s.DateTimeProp, d => d.DateTimeProp)
                .AddMap(s => s.DoubleProp, d => d.DoubleProp)
                .AddMap(s => s.ReferenceProperty, d => d.ReferenceProperty);

            var source1 = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456,
                ReferenceProperty = new MockSource1()
                {
                    GuidProp = Guid.NewGuid(),
                    StringProp = "anyRefString",
                    IntProp = 456
                }
            };

            var source2 = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456,
                ReferenceProperty = new MockSource1()
                {
                    GuidProp = Guid.NewGuid(),
                    StringProp = "anyRefString",
                    IntProp = 456
                }
            };

            var result = (IEnumerable)mapper.Map(
                typeof(MockSource),
                typeof(IEnumerable),
                (new List<MockSource>() { source1, source2 }).AsQueryable());
        }

        [TestMethod]
        public void MapSourceToDestination_NonGenericMetod_TSourceIsArray_AllOk()
        {
            var mapper = new Mapper();

            mapper.CreateConfiguration<MockSource1, MockDestination1>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp);

            mapper.CreateConfiguration<MockSource, MockDestination>()
                .AddMap(s => s.GuidProp, d => d.GuidProp)
                .AddMap(s => s.StringProp, d => d.StringProp)
                .AddMap(s => s.IntProp, d => d.IntProp)
                .AddMap(s => s.DateTimeProp, d => d.DateTimeProp)
                .AddMap(s => s.DoubleProp, d => d.DoubleProp)
                .AddMap(s => s.ReferenceProperty, d => d.ReferenceProperty);

            var source1 = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456,
                ReferenceProperty = new MockSource1()
                {
                    GuidProp = Guid.NewGuid(),
                    StringProp = "anyRefString",
                    IntProp = 456
                }
            };

            var source2 = new MockSource()
            {
                GuidProp = Guid.NewGuid(),
                StringProp = "anyString",
                IntProp = 1234,
                DateTimeProp = DateTime.Now,
                DoubleProp = 123.456,
                ReferenceProperty = new MockSource1()
                {
                    GuidProp = Guid.NewGuid(),
                    StringProp = "anyRefString",
                    IntProp = 456
                }
            };

            var result = (IEnumerable<MockDestination>)mapper.Map(
                typeof(MockSource), 
                typeof(MockDestination), 
                (new List<MockSource>() { source1, source2 }).AsQueryable());

            Assert.AreEqual(2, result.Count(), "Count error.");

            var destination = result.FirstOrDefault(d => d.GuidProp == source1.GuidProp);

            Assert.IsNotNull(destination, "destination1 is null.");
            Assert.AreEqual(source1.StringProp, destination.StringProp, "source1 StringProp error.");
            Assert.AreEqual(source1.IntProp, destination.IntProp, "source1 IntProp error.");
            Assert.AreEqual(source1.DateTimeProp, destination.DateTimeProp, "source1 DateTimeProp error.");
            Assert.AreEqual(source1.DoubleProp, destination.DoubleProp, "source1 DoubleProp error.");

            Assert.AreEqual(source1.ReferenceProperty.GuidProp, destination.ReferenceProperty.GuidProp, "source1 ReferenceProperty.GuidProp error.");
            Assert.AreEqual(source1.ReferenceProperty.StringProp, destination.ReferenceProperty.StringProp, "source1 ReferenceProperty.StringProp error.");
            Assert.AreEqual(source1.ReferenceProperty.IntProp, destination.ReferenceProperty.IntProp, "source1 ReferenceProperty.IntProp error.");

            destination = result.FirstOrDefault(d => d.GuidProp == source2.GuidProp);

            Assert.IsNotNull(destination, "destination2 is null.");
            Assert.AreEqual(source2.StringProp, destination.StringProp, "source2 StringProp error.");
            Assert.AreEqual(source2.IntProp, destination.IntProp, "source2 IntProp error.");
            Assert.AreEqual(source2.DateTimeProp, destination.DateTimeProp, "source2 DateTimeProp error.");
            Assert.AreEqual(source2.DoubleProp, destination.DoubleProp, "source2 DoubleProp error.");

            Assert.AreEqual(source2.ReferenceProperty.GuidProp, destination.ReferenceProperty.GuidProp, "source2 ReferenceProperty.GuidProp error.");
            Assert.AreEqual(source2.ReferenceProperty.StringProp, destination.ReferenceProperty.StringProp, "source2 ReferenceProperty.StringProp error.");
            Assert.AreEqual(source2.ReferenceProperty.IntProp, destination.ReferenceProperty.IntProp, "source2 ReferenceProperty.IntProp error.");
        }

        [TestMethod]
        public void GetSingleMapper_ReturnMethod()
        {
            var singleMapper = Mapper.GetSingleMapper();

            Assert.IsNotNull(singleMapper, "SingleMapper is null");
        }

        [TestMethod]
        public void GetSingleMapper_AllReferencesToSingleObject()
        {
            var singleMapper1 = Mapper.GetSingleMapper();
            var singleMapper2 = Mapper.GetSingleMapper();

            Assert.AreEqual(singleMapper1, singleMapper1, "References to different objects");
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ClearSingleMapperConfiguration_ClearConfiguration()
        {
            var singleMapper = Mapper.GetSingleMapper();

            singleMapper.CreateConfiguration<MockSource, MockDestination>()
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

            Mapper.ClearSingleMapperConfiguration();

            singleMapper.Map<MockSource, MockDestination>(source);
        }
    }
}
