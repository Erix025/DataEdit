using Microsoft.VisualStudio.TestTools.UnitTesting;
using Index.DataEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Index.DataEdit.Tests
{
    [TestClass()]
    public class EditFilesTests
    {
        [TestMethod()]
        public void DeleteLinesTest()
        {
            Assert.AreEqual("OK", EditFiles.DeleteLines(Environment.CurrentDirectory + "/Test.txt",2,4));
        }
    }
}