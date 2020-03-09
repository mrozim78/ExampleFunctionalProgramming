using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Examples.Immutable.After.Tests
{
    [TestClass]
    public class AuditManagerTests
    {
        [TestMethod]
        public void AddRecord_adds_a_record_to_an_existing_file_if_not_overflowed()
        {
            var manager = new AuditManager(10);
            var file = new FileContent("Audit_1.txt", new[]
            {
                "1;Peter Peterson;2016-04-06T16:30:00"
            });
            
            FileAction action = manager.AddRecord(file, "Jane Doe", new DateTime(2016, 4, 6, 17, 0, 0));

            Assert.AreEqual(ActionType.Update, action.Type);
            Assert.AreEqual("Audit_1.txt", action.FileName);
            Assert.IsTrue(ArrayStringIsEqual(new[]
            {
                "1;Peter Peterson;2016-04-06T16:30:00",
                "2;Jane Doe;2016-04-06T17:00:00"
            }, action.Content));
        }

        [TestMethod]
        public void AddRecord_adds_a_record_to_a_new_file_if_overflowed()
        {
            var manager = new AuditManager(3);
            var file = new FileContent("Audit_1.txt", new[]
            {
                "1;Peter Peterson;2016-04-06T16:30:00",
                "2;Jane Doe;2016-04-06T16:40:00",
                "3;Jack Rich;2016-04-06T17:00:00"
            });

            FileAction action = manager.AddRecord(file, "Tom Tomson", new DateTime(2016, 4, 6, 17, 30, 0));

            Assert.AreEqual(ActionType.Create, action.Type);
            Assert.AreEqual("Audit_2.txt", action.FileName);
            Assert.IsTrue(ArrayStringIsEqual(new[]
            {
                "1;Tom Tomson;2016-04-06T17:30:00"
            }, action.Content));
        }

        [TestMethod]
        public void RemoveMentionsAbout_removes_mentions_from_files_in_the_directory()
        {
            var manager = new AuditManager(10);
            var file = new FileContent("Audit_1.txt", new[]
            {
                "1;Peter Peterson;2016-04-06T16:30:00",
                "2;Jane Doe;2016-04-06T16:40:00",
                "3;Jack Rich;2016-04-06T17:00:00"
            });

            IReadOnlyList<FileAction> actions = manager.RemoveMentionsAbout("Peter Peterson", new[] { file });

            Assert.AreEqual(1, actions.Count);
            Assert.AreEqual("Audit_1.txt", actions[0].FileName);
            Assert.AreEqual(ActionType.Update, actions[0].Type);
            Assert.IsTrue( ArrayStringIsEqual(new[]
            {
                "1;Jane Doe;2016-04-06T16:40:00",
                "2;Jack Rich;2016-04-06T17:00:00"
            }, actions[0].Content));
        }

        [TestMethod]
        public void RemoveMentionsAbout_removes_whole_file_if_it_doesnt_contain_anything_else()
        {
            var manager = new AuditManager(10);
            var file = new FileContent("Audit_1.txt", new[]
            {
                "1;Peter Peterson;2016-04-06T16:30:00"
            });

            IReadOnlyList<FileAction> actions = manager.RemoveMentionsAbout("Peter Peterson", new[] { file });

            Assert.AreEqual(1, actions.Count);
            Assert.AreEqual("Audit_1.txt", actions[0].FileName);
            Assert.AreEqual(ActionType.Delete, actions[0].Type);
        }

        [TestMethod]
        public void RemoveMentionsAbout_does_not_do_anything_in_case_no_mentions_found()
        {
            var manager = new AuditManager(10);
            var file = new FileContent("Audit_1.txt", new[]
            {
                "1;Jane Smith;2016-04-06T16:30:00"
            });

            IReadOnlyList<FileAction> actions = manager.RemoveMentionsAbout("Peter Peterson", new[] {file});

            Assert.AreEqual(0, actions.Count);
        }

        private bool ArrayStringIsEqual(string[] array1, string[] array2)
        {
            if (array1 is null || array2 is null) return false;
            if (array1.Length != array2.Length) return false;
            for (int i=0;i<array1.Length;i++)
                if (array1[i] != array2[i])
                    return false;
            return true;

        }

    }
}