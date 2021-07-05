using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Examen;

namespace ExamUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            int rezult = 29;
            string it = "";
            using (StreamReader sr = new StreamReader(@"D:\Slava\Программы\Проекты\Examen\bin\Debug\netcoreapp3.1\Входные данные.csv"))
            {
                it = sr.ReadToEnd();
            }
            char[] chars = { '\r', '\n' };
            string[] str = it.Split(chars);
            Assert.AreEqual(rezult, Convert.ToInt32(str[str.Length-1]));
        }
    }
}
