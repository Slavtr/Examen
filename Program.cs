using System;
using System.Diagnostics;

namespace Examen
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new TextWriterTraceListener("Отладка.txt"));
            Trace.AutoFlush = true;
            Trace.WriteLine("Выполнение начато.");
            Console.WriteLine("Критический путь.");
            Console.WriteLine("Введите путь файла:");
            string path = Console.ReadLine();
            CriticalPath cr = new CriticalPath(path);
            Console.ReadKey();
            Trace.WriteLine("Выполнение завершено.");
        }
    }
}
