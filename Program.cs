using System;

namespace Examen
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Критический путь.");
            Console.WriteLine("Введите путь файла:");
            string path = Console.ReadLine();
            CriticalPath cr = new CriticalPath(path);
            Console.ReadKey();
        }
    }
}
