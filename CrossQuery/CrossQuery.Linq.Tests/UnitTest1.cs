using System;
using System.Linq;
using CrossQuery.Linq.Collections;
using CrossQuery.Linq.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CrossQuery.Linq.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetStudentsCountForGroupFromEvent()
        {
            #region Mock.DB_1_Entities.Groups Initialize

            var group1 = new Mock.DB_1_Entities.Group()
            {
                ID = Guid.NewGuid(),
                Name = "Group1"
            };
            var group2 = new Mock.DB_1_Entities.Group()
            {
                ID = Guid.NewGuid(),
                Name = "Group2"
            };

            #endregion

            #region Mock.DB_1_Entities.Students Initialize

            var student1 = new Mock.DB_1_Entities.Student()
            {
                ID = Guid.NewGuid(),
                Name = "Student1",
                Group = group1
            };

            var student2 = new Mock.DB_1_Entities.Student()
            {
                ID = Guid.NewGuid(),
                Name = "Student2",
                Group = group1
            };

            var student3 = new Mock.DB_1_Entities.Student()
            {
                ID = Guid.NewGuid(),
                Name = "Student3",
                Group = group2
            };

            group1.Students.Add(student1);
            group1.Students.Add(student2);
            group2.Students.Add(student3);

            #endregion

            var db_1_Adapter = new DB_1_Adapter();
            db_1_Adapter.AddObjectToCollection<Mock.DB_1_Entities.Group>(group1);
            db_1_Adapter.AddObjectToCollection<Mock.DB_1_Entities.Group>(group2);
            db_1_Adapter.AddObjectToCollection<Mock.DB_1_Entities.Student>(student1);
            db_1_Adapter.AddObjectToCollection<Mock.DB_1_Entities.Student>(student2);
            db_1_Adapter.AddObjectToCollection<Mock.DB_1_Entities.Student>(student3);

            #region Mock.DB_2_Entities.Events Initialize

            var event1 = new Mock.DB_2_Entities.Event()
            {
                ID = Guid.NewGuid(),
                Name = "eventForGroup1",
                GroupName = group1.Name
            };

            var event2 = new Mock.DB_2_Entities.Event()
            {
                ID = Guid.NewGuid(),
                Name = "eventForGroup2",
                GroupName = group2.Name
            };

            #endregion

            var db_2_Adapter = new DB_2_Adapter();
            db_2_Adapter.AddObjectToCollection<Mock.DB_2_Entities.Event>(event1);
            db_2_Adapter.AddObjectToCollection<Mock.DB_2_Entities.Event>(event2);

            var cqProvider = new CQProvider(db_1_Adapter, db_2_Adapter);

            var result = (new CQSet<Mock.DomainModel.Event>(cqProvider)).Where(e => e.ID == event1.ID).SelectMany(e => e.Group.Students).Count();

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void GetEventByGroupID_GroupFromDB1_EventFromDB2()
        {
            #region Mock.DB_1_Entities.Groups Initialize

            var group1 = new Mock.DB_1_Entities.Group()
            {
                ID = Guid.NewGuid(),
                Name = "Group1"
            };
            var group2 = new Mock.DB_1_Entities.Group()
            {
                ID = Guid.NewGuid(),
                Name = "Group2"
            };

            #endregion

            #region Mock.DB_1_Entities.Students Initialize

            var student1 = new Mock.DB_1_Entities.Student()
            {
                ID = Guid.NewGuid(),
                Name = "Student1",
                Group = group1
            };

            var student2 = new Mock.DB_1_Entities.Student()
            {
                ID = Guid.NewGuid(),
                Name = "Student2",
                Group = group1
            };

            var student3 = new Mock.DB_1_Entities.Student()
            {
                ID = Guid.NewGuid(),
                Name = "Student3",
                Group = group2
            };

            group1.Students.Add(student1);
            group1.Students.Add(student2);
            group2.Students.Add(student3);

            #endregion

            var db_1_Adapter = new DB_1_Adapter();
            db_1_Adapter.AddObjectToCollection<Mock.DB_1_Entities.Group>(group1);
            db_1_Adapter.AddObjectToCollection<Mock.DB_1_Entities.Group>(group2);
            db_1_Adapter.AddObjectToCollection<Mock.DB_1_Entities.Student>(student1);
            db_1_Adapter.AddObjectToCollection<Mock.DB_1_Entities.Student>(student2);
            db_1_Adapter.AddObjectToCollection<Mock.DB_1_Entities.Student>(student3);

            #region Mock.DB_2_Entities.Events Initialize

            var event1 = new Mock.DB_2_Entities.Event()
            {
                ID = Guid.NewGuid(),
                Name = "eventForGroup1",
                GroupName = group1.Name
            };

            var event2 = new Mock.DB_2_Entities.Event()
            {
                ID = Guid.NewGuid(),
                Name = "eventForGroup2",
                GroupName = group2.Name
            };

            #endregion

            var db_2_Adapter = new DB_2_Adapter();
            db_2_Adapter.AddObjectToCollection<Mock.DB_2_Entities.Event>(event1);
            db_2_Adapter.AddObjectToCollection<Mock.DB_2_Entities.Event>(event2);

            var cqProvider = new CQProvider(db_1_Adapter, db_2_Adapter);

            var result = (new CQSet<Mock.DomainModel.Event>(cqProvider)).FirstOrDefault(e => e.Group.ID == group1.ID);

            Assert.IsNotNull(result, "Event not found");
            Assert.AreEqual(event1.ID, result.ID, "Wrong event");
        }

        [TestMethod]
        public void GetGroupByID_GroupFromDB1()
        {
            #region Mock.DB_1_Entities.Groups Initialize

            var group1 = new Mock.DB_1_Entities.Group()
            {
                ID = Guid.NewGuid(),
                Name = "Group1"
            };
            var group2 = new Mock.DB_1_Entities.Group()
            {
                ID = Guid.NewGuid(),
                Name = "Group2"
            };

            #endregion

            var db_1_Adapter = new DB_1_Adapter();
            db_1_Adapter.AddObjectToCollection<Mock.DB_1_Entities.Group>(group1);
            db_1_Adapter.AddObjectToCollection<Mock.DB_1_Entities.Group>(group2);

            var cqProvider = new CQProvider(db_1_Adapter);

            var result = (new CQSet<Mock.DomainModel.Group>(cqProvider)).FirstOrDefault(g => g.ID == group1.ID);

            Assert.IsNotNull(result, "Event not found");
            Assert.AreEqual(group1.ID, result.ID, "Wrong group");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetTeacherByID_AdapterAttributeIsNotAdded()
        {
            var db_1_Adapter = new DB_1_Adapter();
           
            var cqProvider = new CQProvider(db_1_Adapter);

            var result = (new CQSet<Mock.DomainModel.Teacher>(cqProvider)).First();
        }
    }
}
