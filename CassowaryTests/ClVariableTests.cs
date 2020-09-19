using Cassowary_moddiff;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cassowary_moddiffTests
{
    [TestClass]
    public class ClVariableTests
    {
        [TestMethod]
        public void PassingNameIntoConstructor_SetsNameProperty()
        {
            ClVariable variable = new ClVariable("name");

            Assert.AreEqual("name", variable.Name);
        }

        [TestMethod]
        public void PassingNameAndValueIntoConstructor_SetsNamePropertyAndSetsValueProperty()
        {
            ClVariable variable = new ClVariable("name", 111.1);

            Assert.AreEqual("name", variable.Name);
            Assert.AreEqual(111.1, variable.Value);
        }

        [TestMethod]
        public void PassingValueIntoConstructor_SetsValueProperty()
        {
            ClVariable variable = new ClVariable(111.1);

            Assert.AreEqual(111.1, variable.Value);
        }

        [TestMethod]
        public void PassingIdAndPrefixIntoConstructor_SetsNameProperty()
        {
            ClVariable variable = new ClVariable(1, "prefix");

            Assert.IsNotNull(variable.Name);
            Assert.IsTrue(variable.Name.Contains("prefix"));
            Assert.IsTrue(variable.Name.Contains("1"));
        }

        [TestMethod]
        public void ToStringRepresentation_IncludesNameAndValue()
        {
            ClVariable variable = new ClVariable("name", 111.1);

            Assert.IsTrue(variable.ToString().Contains("name"));
            Assert.IsTrue(variable.ToString().Contains("111.1"));
        }

        [TestMethod]
        public void IsDummy_IsFalse()
        {
            ClVariable variable = new ClVariable("a");

            Assert.IsFalse(variable.IsDummy);
        }

        [TestMethod]
        public void IsExternal_IsTrue()
        {
            ClVariable variable = new ClVariable("a");

            Assert.IsTrue(variable.IsExternal);
        }

        [TestMethod]
        public void IsPivotable_IsFalse()
        {
            ClVariable variable = new ClVariable("a");

            Assert.IsFalse(variable.IsPivotable);
        }

        [TestMethod]
        public void IsRestricted_IsFalse()
        {
            ClVariable variable = new ClVariable("a");

            Assert.IsFalse(variable.IsRestricted);
        }

    }
}
