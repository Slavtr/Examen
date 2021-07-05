using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Examen;
using System.Collections.Generic;

namespace ExamTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            int exp = 29;
            string it = "";
            using (StreamReader sr = new StreamReader(@"D:\Slava\Программы\Проекты\Examen\bin\Debug\netcoreapp3.1\Итог.csv"))
            {
                it = sr.ReadToEnd();
            }
            char[] ch = { '\r', '\n' };
            string[] str1 = it.Split(ch);
            List<string> ls = new List<string>();
            for(int i = str1.Length-1; i>=0; i--)
            {
                if(str1[i]!="")
                {
                    ls.Add(str1[i]);
                }
            }
            Assert.IsTrue(Convert.ToInt32(ls[0]) == exp);
        }
    }
}
