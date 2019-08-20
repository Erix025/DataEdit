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
    public class DataEditTests
    {
        [TestMethod()]
        public void SortTest()
        {
            int[] test = new int[] { 9, 4, 4, 7, 2, 6 };
            int[] output = DataEdit.Sort(test);
            Assert.AreEqual(test[0], 2);
            Assert.AreEqual(test[1], 4);
            Assert.AreEqual(test[2], 4);
            Assert.AreEqual(test[3], 6);
            Assert.AreEqual(test[4], 7);
            Assert.AreEqual(test[5], 9);
        }
    }
    [TestClass()]
    public class FunctionTests
    {
        [TestMethod()]
        public void FormatTest()
        {
            Assert.AreEqual(Function.ToDataString("hello\\ni\\nam\\nchina"),"hello\ni\nam\nchina");
        }

    }
}