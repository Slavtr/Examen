using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Examen
{
    /// <summary>
    /// Класс для поиска критического пути с применением рекурсивного алгоритма.
    /// </summary>
    public class CriticalPath
    {
        //Строка, в которую пути записываются из рекурсии. Нужна из-за особенностей ветвления в рекурсиях.
        string s = "";
        /// <summary>
        /// Конструктор, принимающий путь к файлу и активирующий методы поиска критического пути
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public CriticalPath(string path)
        {
            try
            {
                Trace.WriteLine("Запущен класс критического пути.");
                List<Rbt> ret;
                List<Rbt> ls = Flrd(path);
                //Список из рёбер, выходящих из начальной точки графа.
                ret = ls.FindAll(x => x.point1 == ls[Minel(ls)].point1);
                //Список путей.
                List<List<Rbt>> fnlcn = new List<List<Rbt>>();
                Trace.WriteLine("Запущен поиск критических путей");
                foreach (Rbt rb in ret)
                {
                    //Запускается рекурсия для поиска путей из начальной точки, указанной ранее. Цикл нужен, чтобы проверить все начальные точки.
                    Mv(ls, rb);
                    //В список путей добавляется длиннейший путь из полученных.
                    fnlcn.Add(RtPrs(ls, s));
                    //Строка обновляется для записи новых путей.
                    s = "";
                }
                Trace.WriteLine("Запущен поиск пути с максимальной длиной");
                //Поиск пути с максимальной длиной.
                int max = fnlcn[0][0].length, maxind = 0;
                List<int> maxindl = new List<int>();
                for (int i = 0; i < ret.Count; i++)
                {
                    if (FnlMv(fnlcn[i]) > max)
                    {
                        max = FnlMv(fnlcn[i]);
                        maxind = i;
                    }
                }
                //Поиск одинаковых критических путей.
                maxindl.Add(maxind);
                for (int i = 0; i < ret.Count; i++)
                {
                    if (FnlMv(fnlcn[i]) == max && i != maxind)
                    {
                        maxindl.Add(i);
                    }
                }
                Trace.WriteLine("Запущен процесс вывода результатов поиска.");
                //Диалог с пользователем.
                Console.WriteLine("Сохранить итог?(Y/N)");
                string itog = Console.ReadLine();
                if (itog == "Y")
                {
                    //Запись критического пути в файл.
                    using (StreamWriter sr = new StreamWriter("Итог.csv"))
                    {
                        foreach (int mx in maxindl)
                        {
                            foreach (Rbt rb in fnlcn[mx])
                            {
                                sr.WriteLine(rb.point1 + " - " + rb.point2);
                            }
                            sr.WriteLine(max);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Итог не был сохранён.");
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("Выполнение прервано с ошибкой " + e.Message);
            }
        }
        /// <summary>
        /// Структура, хранящая рёбра графа
        /// </summary>
        struct Rbt
        {
            public int point1;
            public int point2;
            public int length;
            public bool Equals(Rbt obj)
            {
                if (this.point1 == obj.point1 && this.point2 == obj.point2 && this.length == obj.length) return true;
                else return false;
            }
            public override string ToString()
            {
                return point1.ToString() + " - " + point2.ToString() + " " + length.ToString();
            }
        }
        /// <summary>
        /// Метод поиска минимального элемента
        /// </summary>
        /// <param name="ls">Список структур, в котором производится поиск</param>
        /// <returns>Индекс минимального элемента в списке</returns>
        int Minel(List<Rbt> ls)
        {
            int min = ls[0].point1, minind = 0;
            foreach (Rbt rb in ls)
            {
                if (rb.point1 <= min)
                {
                    min = rb.point1;
                    minind = ls.IndexOf(rb);
                }
            }
            return minind;
        }
        /// <summary>
        /// Метод поиска максимального элемента списка
        /// </summary>
        /// <param name="ls">Список структур, где производится поиск</param>
        /// <returns>Индекс максимального элемента</returns>
        int Maxel(List<Rbt> ls)
        {
            int min = ls[0].point2, maxind = 0;
            foreach (Rbt rb in ls)
            {
                if (rb.point2 >= min)
                {
                    min = rb.point1;
                    maxind = ls.IndexOf(rb);
                }
            }
            return maxind;
        }
        /// <summary>
        /// Рекурсивный метод, проходящий все пути в графе и записывающий их в строку
        /// </summary>
        /// <param name="ls">Список структур, представляющий рёбра графа</param>
        /// <param name="minel">Элемент, с которого начинается поиск путей</param>
        /// <returns>Возвращает длину пройденного пути</returns>
        int Mv(List<Rbt> ls, Rbt minel)
        {
            int ret = 0;
            Rbt rb = ls.Find(x => x.point1 == minel.point1 && x.point2 == minel.point2);
            s += rb.point1.ToString() + "-" + rb.point2.ToString();
            if (rb.point2 == ls[Maxel(ls)].point2)
            {
                s += ";";
                return rb.length;
            }
            else
            {
                for (int i = 0; i < ls.Count; i++)
                {
                    if (ls[i].point1 == rb.point2)
                    {
                        s += ",";
                        ret = Mv(ls, ls[i]) + rb.length;
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// Метод чтения файла и заполнения списка структур для последующего использования
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Список структур, представляющий рёбра графа</returns>
        List<Rbt> Flrd(string path)
        {
            List<Rbt> ret = new List<Rbt>();
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.EndOfStream != true)
                {
                    string[] str1 = sr.ReadLine().Split(';');
                    string[] str2 = str1[0].Split('-');
                    ret.Add(new Rbt { point1 = Convert.ToInt32(str2[0]), point2 = Convert.ToInt32(str2[1]), length = Convert.ToInt32(str1[1]) });
                }
            }
            return ret;
        }
        /// <summary>
        /// Метод, разбивающий строку со всеми путями в графе на отдельные пути и возвращающий длиннейший из них.
        /// </summary>
        /// <param name="ls">Список рёбер графа</param>
        /// <param name="s">Строка со всеми рекурсивно найденными путями в графе</param>
        /// <returns></returns>
        List<Rbt> RtPrs(List<Rbt> ls, string s)
        {
            List<List<Rbt>> ret = new List<List<Rbt>>();
            string[] str1 = s.Split(';');
            foreach (string st1 in str1)
            {
                if (st1 != "")
                {
                    ret.Add(new List<Rbt>());
                    string[] str2 = st1.Split(',');
                    foreach (string st2 in str2)
                    {
                        if (st2 != "")
                        {
                            string[] str3 = st2.Split('-');
                            ret[ret.Count - 1].Add(ls.Find(x => x.point1 == Convert.ToInt32(str3[0]) && x.point2 == Convert.ToInt32(str3[1])));
                        }
                    }
                }
            }
            for (int i = 0; i < ret.Count; i++)
            {
                if (i > 0)
                {
                    if (ret[i][0].point1 != ret[i][ret[i].Count - 1].point2)
                    {
                        ret[i].InsertRange(0, ret[i - 1].FindAll(x => ret[i - 1].IndexOf(x) <= ret[i - 1].FindIndex(y => y.point2 == ret[i][0].point1)));
                    }
                }
            }
            int max = ret[0][0].length, maxind = 0;
            for (int i = 0; i < ret.Count; i++)
            {
                if (FnlMv(ret[i]) >= max)
                {
                    max = FnlMv(ret[i]);
                    maxind = i;
                }
            }
            return ret[maxind];
        }
        /// <summary>
        /// Метод, ищущий длину пути.
        /// </summary>
        /// <param name="ls">Список рёбер, представляющий путь.</param>
        /// <returns>Длина пути по списку.</returns>
        int FnlMv(List<Rbt> ls)
        {
            int ret = 0;
            foreach (Rbt rb in ls)
            {
                ret += rb.length;
            }
            return ret;
        }
    }
}
