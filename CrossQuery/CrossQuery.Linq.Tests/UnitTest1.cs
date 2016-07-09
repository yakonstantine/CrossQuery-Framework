using System;
using System.Linq;
using CrossQuery.Linq.Collections;
using CrossQuery.Linq.Tests.Mock.DB1_Context;
using CrossQuery.Linq.Tests.Mock.DB2_Context;
using CrossQuery.Mapper.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CrossQuery.Linq.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Initialize()
        {
            var db1Adapter = new DB1Adapter();
            db1Adapter.ExecuteSqlCommand("delete from [Student]");
            db1Adapter.ExecuteSqlCommand("delete from [Group]");            

            var db2Adapter = new DB2Adapter();
            db2Adapter.ExecuteSqlCommand("delete from [Event]");
        }

        [TestMethod]
        public void GetStudentsCountForGroupFromEvent()
        {
            #region Mock.DB_1_Entities.Groups Initialize

            var group1 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group1"
            };
            var group2 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group2"
            };

            #endregion

            #region Mock.DB_1_Entities.Students Initialize

            var student1 = new Mock.DB1_Context.Student()
            {
                Id = Guid.NewGuid(),
                Name = "Student1",
                Group = group1
            };

            var student2 = new Mock.DB1_Context.Student()
            {
                Id = Guid.NewGuid(),
                Name = "Student2",
                Group = group1
            };

            var student3 = new Mock.DB1_Context.Student()
            {
                Id = Guid.NewGuid(),
                Name = "Student3",
                Group = group2
            };

            #endregion

            var db1Adapter = new DB1Adapter();
            db1Adapter.AddEntity<Group>(group1);
            db1Adapter.AddEntity<Group>(group2);
            db1Adapter.AddEntity<Student>(student1);
            db1Adapter.AddEntity<Student>(student2);
            db1Adapter.AddEntity<Student>(student3);
            db1Adapter.SaveChanges();

            #region Mock.DB_2_Entities.Events Initialize

            var event1 = new Mock.DB2_Context.Event()
            {
                Id = Guid.NewGuid(),
                Name = "eventForGroup1",
                GroupName = group1.Name
            };

            var event2 = new Mock.DB2_Context.Event()
            {
                Id = Guid.NewGuid(),
                Name = "eventForGroup2",
                GroupName = group2.Name
            };

            #endregion

            var db2Adapter = new DB2Adapter();
            db2Adapter.AddEntity<Event>(event1);
            db2Adapter.AddEntity<Event>(event2);
            db2Adapter.SaveChanges();

            var cqContext = new CQContext(null, db1Adapter, db2Adapter);

            var result = cqContext.GetEntities<Mock.DomainModel.Event>().Where(e => e.ID == event1.Id).SelectMany(e => e.Group.Students).Count();

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void GetEventByGroupID_GroupFromDB1_EventFromDB2()
        {
            #region Mock.DB_1_Entities.Groups Initialize

            var group1 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group1"
            };
            var group2 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group2"
            };

            #endregion

            var db1Adapter = new DB1Adapter();
            db1Adapter.AddEntity<Group>(group1);
            db1Adapter.AddEntity<Group>(group2);
            db1Adapter.SaveChanges();

            #region Mock.DB_2_Entities.Events Initialize

            var event1 = new Mock.DB2_Context.Event()
            {
                Id = Guid.NewGuid(),
                Name = "eventForGroup1",
                GroupName = group1.Name
            };

            var event2 = new Mock.DB2_Context.Event()
            {
                Id = Guid.NewGuid(),
                Name = "eventForGroup2",
                GroupName = group2.Name
            };

            #endregion

            var db2Adapter = new DB2Adapter();
            db2Adapter.AddEntity<Event>(event1);
            db2Adapter.AddEntity<Event>(event2);
            db2Adapter.SaveChanges();

            var mapper = new Mapper.Mapper();

            mapper.CreateConfiguration<Mock.DB1_Context.Group, Mock.DomainModel.Group>()
                .AddMap(s => s.Id, d => d.ID)
                .AddMap(s => s.Name, d => d.Name);

            mapper.CreateConfiguration<Mock.DB2_Context.Event, Mock.DomainModel.Event>()
                .AddMap(s => s.Id, d => d.ID)
                .AddMap(s => s.Name, d => d.Name);

            var cqContext = new CQContext(mapper, db1Adapter, db2Adapter);

            var result = cqContext.GetEntities<Mock.DomainModel.Event>().FirstOrDefault(e => e.Group.ID == group1.Id);

            Assert.IsNotNull(result, "Event not found");
            Assert.AreEqual(event1.Id, result.ID, "Wrong event");
        }

        [TestMethod]
        public void GetGroupByID_GroupFromDB1()
        {
            #region Mock.DB_1_Entities.Groups Initialize

            var group1 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group1"
            };
            var group2 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group2"
            };

            #endregion

            var db1Adapter = new DB1Adapter();
            db1Adapter.AddEntity<Group>(group1);
            db1Adapter.AddEntity<Group>(group2);
            db1Adapter.SaveChanges();

            var mapper = new Mapper.Mapper();

            mapper.CreateConfiguration<Mock.DB1_Context.Group, Mock.DomainModel.Group>()
                .AddMap(s => s.Id, d => d.ID)
                .AddMap(s => s.Name, d => d.Name);

            var cqContext = new CQContext(mapper, db1Adapter);

            var result = cqContext.GetEntities<Mock.DomainModel.Group>().FirstOrDefault(g => g.ID == group1.Id);

            Assert.IsNotNull(result, "Event not found");
            Assert.AreEqual(group1.Id, result.ID, "Wrong group");
        }

        [TestMethod]
        public void GetGroupsWhereNumberEqual1_GroupFromDB1()
        {
            #region Mock.DB_1_Entities.Groups Initialize

            var group1 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group1",
                Number = 1
            };
            var group2 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group2",
                Number = 1
            };
            var group3 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group3",
                Number = 2
            };

            #endregion

            var db1Adapter = new DB1Adapter();
            db1Adapter.AddEntity<Group>(group1);
            db1Adapter.AddEntity<Group>(group2);
            db1Adapter.AddEntity<Group>(group3);
            db1Adapter.SaveChanges();

            var mapper = new Mapper.Mapper();

            mapper.CreateConfiguration<Mock.DB1_Context.Group, Mock.DomainModel.Group>()
                .AddMap(s => s.Id, d => d.ID)
                .AddMap(s => s.Name, d => d.Name)
                .AddMap(s => s.Number, d => d.Number);

            var cqContext = new CQContext(mapper, db1Adapter);

            var result = cqContext.GetEntities<Mock.DomainModel.Group>().Where(g => g.Number == 1).ToList();

            Assert.AreEqual(2, result.Count(), "Wrong groups count");
            Assert.IsFalse(result.Any(g => g.Number != 1), "Wrong group returned");
        }

        [TestMethod]
        public void GetGroupsWhereNumberEqual1CountWithExpression_GroupFromDB1()
        {
            #region Mock.DB_1_Entities.Groups Initialize

            var group1 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group1",
                Number = 1
            };
            var group2 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group2",
                Number = 1
            };
            var group3 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group3",
                Number = 2
            };

            #endregion

            var db1Adapter = new DB1Adapter();
            db1Adapter.AddEntity<Group>(group1);
            db1Adapter.AddEntity<Group>(group2);
            db1Adapter.AddEntity<Group>(group3);
            db1Adapter.SaveChanges();

            var mapper = new Mapper.Mapper();

            mapper.CreateConfiguration<Mock.DB1_Context.Group, Mock.DomainModel.Group>()
                .AddMap(s => s.Id, d => d.ID)
                .AddMap(s => s.Name, d => d.Name)
                .AddMap(s => s.Number, d => d.Number);

            var cqContext = new CQContext(mapper, db1Adapter);

            var result = cqContext.GetEntities<Mock.DomainModel.Group>().Count(g => g.Number == 1);

            Assert.AreEqual(2, result, "Wrong groups count");
        }

        [TestMethod]
        public void GetGroupsWhereNumberEqual1CountWithoutExpression_GroupFromDB1()
        {
            #region Mock.DB_1_Entities.Groups Initialize

            var group1 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group1",
                Number = 1
            };
            var group2 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group2",
                Number = 1
            };
            var group3 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group3",
                Number = 2
            };

            #endregion

            var db1Adapter = new DB1Adapter();
            db1Adapter.AddEntity<Group>(group1);
            db1Adapter.AddEntity<Group>(group2);
            db1Adapter.AddEntity<Group>(group3);
            db1Adapter.SaveChanges();

            var mapper = new Mapper.Mapper();

            mapper.CreateConfiguration<Mock.DB1_Context.Group, Mock.DomainModel.Group>()
                .AddMap(s => s.Id, d => d.ID)
                .AddMap(s => s.Name, d => d.Name)
                .AddMap(s => s.Number, d => d.Number);

            var cqContext = new CQContext(mapper, db1Adapter);

            var result = cqContext.GetEntities<Mock.DomainModel.Group>().Where(g => g.Number == 1).Count();

            Assert.AreEqual(2, result, "Wrong groups count");
        }

        [TestMethod]
        public void GetGroupsByNumberAndIdWhereWithAnd_GroupFromDB1()
        {
            #region Mock.DB_1_Entities.Groups Initialize

            var group1 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group1",
                Number = 1
            };
            var group2 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group2",
                Number = 1
            };
            var group3 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group3",
                Number = 2
            };

            #endregion

            var db1Adapter = new DB1Adapter();
            db1Adapter.AddEntity<Group>(group1);
            db1Adapter.AddEntity<Group>(group2);
            db1Adapter.AddEntity<Group>(group3);
            db1Adapter.SaveChanges();

            var mapper = new Mapper.Mapper();

            mapper.CreateConfiguration<Mock.DB1_Context.Group, Mock.DomainModel.Group>()
                .AddMap(s => s.Id, d => d.ID)
                .AddMap(s => s.Name, d => d.Name)
                .AddMap(s => s.Number, d => d.Number);

            var cqContext = new CQContext(mapper, db1Adapter);

            var result = cqContext.GetEntities<Mock.DomainModel.Group>()
                .Where(g => g.Number == group1.Number && g.ID == group1.Id).First();

            Assert.AreEqual(group1.Id, result.ID, "Wrong group returned");
        }

        [TestMethod]
        public void GetGroupsByNumberAndIdDoubleWhere_GroupFromDB1()
        {
            #region Mock.DB_1_Entities.Groups Initialize

            var group1 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group1",
                Number = 1
            };
            var group2 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group2",
                Number = 1
            };
            var group3 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group3",
                Number = 2
            };

            #endregion

            var db1Adapter = new DB1Adapter();
            db1Adapter.AddEntity<Group>(group1);
            db1Adapter.AddEntity<Group>(group2);
            db1Adapter.AddEntity<Group>(group3);
            db1Adapter.SaveChanges();

            var mapper = new Mapper.Mapper();

            mapper.CreateConfiguration<Mock.DB1_Context.Group, Mock.DomainModel.Group>()
                .AddMap(s => s.Id, d => d.ID)
                .AddMap(s => s.Name, d => d.Name)
                .AddMap(s => s.Number, d => d.Number);

            var cqContext = new CQContext(mapper, db1Adapter);

            var result = cqContext.GetEntities<Mock.DomainModel.Group>()
                .Where(g => g.Number == group1.Number)
                .Where(g => g.ID == group1.Id)
                .First();

            Assert.AreEqual(group1.Id, result.ID, "Wrong groups count");
        }

        [TestMethod]
        public void GetGroupsByNumberWhereUnaryNot_GroupFromDB1()
        {
            #region Mock.DB_1_Entities.Groups Initialize

            var group1 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group1",
                Number = 1
            };
            var group2 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group2",
                Number = 1
            };
            var group3 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group3",
                Number = 2
            };

            #endregion

            var db1Adapter = new DB1Adapter();
            db1Adapter.AddEntity<Group>(group1);
            db1Adapter.AddEntity<Group>(group2);
            db1Adapter.AddEntity<Group>(group3);
            db1Adapter.SaveChanges();

            var mapper = new Mapper.Mapper();

            mapper.CreateConfiguration<Mock.DB1_Context.Group, Mock.DomainModel.Group>()
                .AddMap(s => s.Id, d => d.ID)
                .AddMap(s => s.Name, d => d.Name)
                .AddMap(s => s.Number, d => d.Number);

            var cqContext = new CQContext(mapper, db1Adapter);

            var result = cqContext.GetEntities<Mock.DomainModel.Group>()
                .Where(g => !(g.Number == 1))
                .First();

            Assert.AreEqual(group3.Id, result.ID, "Wrong groups count");
        }

        [TestMethod]
        public void GetGroupsSelectID_GroupFromDB1()
        {
            #region Mock.DB_1_Entities.Groups Initialize

            var group1 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group1",
                Number = 1
            };
            var group2 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group2",
                Number = 1
            };
            var group3 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group3",
                Number = 2
            };

            #endregion

            var db1Adapter = new DB1Adapter();
            db1Adapter.AddEntity<Group>(group1);
            db1Adapter.AddEntity<Group>(group2);
            db1Adapter.AddEntity<Group>(group3);
            db1Adapter.SaveChanges();

            var mapper = new Mapper.Mapper();

            mapper.CreateConfiguration<Mock.DB1_Context.Group, Mock.DomainModel.Group>()
                .AddMap(s => s.Id, d => d.ID)
                .AddMap(s => s.Name, d => d.Name)
                .AddMap(s => s.Number, d => d.Number);

            var cqContext = new CQContext(mapper, db1Adapter);

            var result = cqContext.GetEntities<Mock.DomainModel.Group>().Select(g => g.ID).ToList();

            Assert.IsTrue(result.Any(r => r == group1.Id), "group1 is not found");
            Assert.IsTrue(result.Any(r => r == group2.Id), "group2 is not found");
            Assert.IsTrue(result.Any(r => r == group3.Id), "group3 is not found");
        }


        [TestMethod]
        public void GetGroupsSelectNewEntity_GroupFromDB1()
        {
            #region Mock.DB_1_Entities.Groups Initialize

            var group1 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group1",
                Number = 1
            };
            var group2 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group2",
                Number = 1
            };
            var group3 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group3",
                Number = 2
            };

            #endregion

            var db1Adapter = new DB1Adapter();
            db1Adapter.AddEntity<Group>(group1);
            db1Adapter.AddEntity<Group>(group2);
            db1Adapter.AddEntity<Group>(group3);
            db1Adapter.SaveChanges();

            var mapper = new Mapper.Mapper();

            mapper.CreateConfiguration<Mock.DB1_Context.Group, Mock.DomainModel.Group>()
                .AddMap(s => s.Id, d => d.ID)
                .AddMap(s => s.Name, d => d.Name)
                .AddMap(s => s.Number, d => d.Number);

            var cqContext = new CQContext(mapper, db1Adapter);

            var result = cqContext.GetEntities<Mock.DomainModel.Group>()
                .Select(g => new { g.ID, g.Number })
                .ToList();

            Assert.IsTrue(result.Any(r => r.ID == group1.Id), "group1 is not found");
            Assert.IsTrue(result.Any(r => r.ID == group2.Id), "group2 is not found");
            Assert.IsTrue(result.Any(r => r.ID == group3.Id), "group3 is not found");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetTeacherByID_AdapterAttributeIsNotAdded()
        {
            var db1Adapter = new DB1Adapter();

            var cqContext = new CQContext(null, db1Adapter);

            var result = cqContext.GetEntities<Mock.DomainModel.Teacher>().First();
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetGroup_GroupFromDB1_GroupWithAttributeWithoutSourceClass()
        {
            #region Mock.DB_1_Entities.Groups Initialize

            var group1 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group1"
            };
            var group2 = new Mock.DB1_Context.Group()
            {
                Id = Guid.NewGuid(),
                Name = "Group2"
            };

            #endregion

            var db1Adapter = new DB1Adapter();
            db1Adapter.AddEntity<Group>(group1);
            db1Adapter.AddEntity<Group>(group2);
            db1Adapter.SaveChanges();

            var mapper = new Mapper.Mapper();

            mapper.CreateConfiguration<Mock.DB1_Context.Group, Mock.DomainModel.GroupWitoutSourceClass>()
                .AddMap(s => s.Id, d => d.ID)
                .AddMap(s => s.Name, d => d.Name);

            var cqContext = new CQContext(mapper, db1Adapter);

            var result = cqContext.GetEntities<Mock.DomainModel.GroupWitoutSourceClass>().FirstOrDefault();
        }
    }
}
