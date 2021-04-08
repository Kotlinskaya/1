using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    abstract class Triad
    {
        public int a = 0, b = 0, c = 0;

        public Triad(int a, int b, int c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        virtual public void IncA()
        {
            a++;
        }

        virtual public void IncB()
        {
            b++;
        }

        virtual public void IncC()
        {
            c++;
        }
    }

    class Date : Triad, IComparable
    {
        public Date(int a, int b, int c) : base(a, b, c) { }

        override public string ToString()
        {
            return $"{a:d2}/{b:d2}/{c:d4}"; //дд:мм:гггг
        }

        public int CompareTo(object o)
        {
            Date p = o as Date;
            if (p != null)
                return new DateTime(c, b, a).CompareTo(new DateTime(p.c, p.b, p.a));
            else
                throw new Exception("Невозможно сравнить два объекта");
        }

    }

    class DateComparer : IComparer<Date>
    {
        public int Compare(Date p1, Date p2)
        {
            return new DateTime(p1.c, p1.b, p1.a).CompareTo(new DateTime(p2.c, p2.b, p2.a));
        }
    }

    class Time : Triad
    {
        public Time(int a, int b, int c) : base(a, b, c) { }

        override public string ToString()
        {
            return $"{a:d2}:{b:d2}:{c:d2}"; //чч:мм:сс
        }

        public int CompareTo(object o)
        {
            Time p = o as Time;
            if (p != null)
                return new DateTime(0, 0, 0, a, b, c).CompareTo(new DateTime(0, 0, 0, p.a, p.b, p.c));
            else
                throw new Exception("Невозможно сравнить два объекта");
        }
    }

    class TimeComparer : IComparer<Time>
    {
        public int Compare(Time p1, Time p2)
        {
            return new DateTime(0, 0, 0, p1.a, p1.b, p1.c).CompareTo(new DateTime(0, 0, 0, p2.a, p2.b, p2.c));
        }
    }

    class Memories
    {
        public object[,] memo = new Memories[0, 3];

        public Memories(int size = 0)
        {
            if (size > 0)
            {
                ResizeArray(ref memo, 3, size);
            }
        }

        public void FillMemo()
        {
            for (int i = 0; i < memo.GetLength(0); i++)
            {
                memo[i, 0] = new Date(int.Parse(Console.ReadLine()),
                int.Parse(Console.ReadLine()),
                int.Parse(Console.ReadLine()));
                memo[i, 1] = new Time(int.Parse(Console.ReadLine()),
                int.Parse(Console.ReadLine()),
                int.Parse(Console.ReadLine()));
                memo[i, 2] = Console.ReadLine();
            }
        }

        public void AddMemo(int day, int month, int year, int hour, int min, int sec, string note)
        {
            ResizeArray(ref memo, 3, memo.GetLength(0) + 1);
            memo[memo.GetLength(0) - 1, 0] = new Date(day, month, year);
            memo[memo.GetLength(0) - 1, 1] = new Time(hour, min, sec);
            memo[memo.GetLength(0) - 1, 2] = note;
        }

        public void PrintAllMemo()
        {
            Console.WriteLine("\nВоспоминания:");
            for (int i = 0; i < memo.GetLength(0); i++)
            {
                Console.WriteLine($"{memo[i, 0].ToString()} {memo[i, 1].ToString()} {memo[i, 2]}");
            }
        }

        public string Latest()
        {
            int t = 0;
            for (int i = 1; i < memo.GetLength(0); i++)
            {
                if ((memo[t, 0] as Date).CompareTo((memo[i, 0] as Date)) == -1)
                {
                    t = i;
                    continue;
                }
                if ((memo[t, 0] as Date).CompareTo((memo[i, 0] as Date)) == 0)
                {
                    if ((memo[t, 1] as Time).CompareTo((memo[i, 1] as Time)) == -1)
                    {
                        t = i;
                    }
                }
            }
            return $"{memo[t, 0].ToString()} {memo[t, 1].ToString()} {memo[t, 2]}";
        }

        public string Earliest()
        {
            int t = 0;
            for (int i = 1; i < memo.GetLength(0); i++)
            {
                if ((memo[t, 0] as Date).CompareTo((memo[i, 0] as Date)) == 1)
                {
                    t = i;
                    continue;
                }
                if ((memo[t, 0] as Date).CompareTo((memo[i, 0] as Date)) == 0)
                {
                    if ((memo[t, 1] as Time).CompareTo((memo[i, 1] as Time)) == 1)
                    {
                        t = i;
                    }
                }
            }
            return $"{memo[t, 0].ToString()} {memo[t, 1].ToString()} {memo[t, 2]}";
        }

        public void ResizeArray(ref object[,] original, int cols, int rows)
        {
            object[,] newArray = new object[rows, cols];
            Array.Copy(original, newArray, original.Length);
            original = newArray;
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите размер массива воспоминаний: ");
            Memories m = new Memories(int.Parse(Console.ReadLine()));
            Console.WriteLine("Заполните массив воспоминаний. День, месяц, год, час, минута, секунда, запись.");
            m.FillMemo();
            m.PrintAllMemo();
            Console.WriteLine("\nДобавить одно воспоминание.");
            m.AddMemo(int.Parse(Console.ReadLine()), int.Parse(Console.ReadLine()),
            int.Parse(Console.ReadLine()), int.Parse(Console.ReadLine()),
            int.Parse(Console.ReadLine()),
            int.Parse(Console.ReadLine()),
            Console.ReadLine());
            m.PrintAllMemo();
            Console.WriteLine($"\nСамая ранняя запись: {m.Earliest()}");
            Console.WriteLine($"\nСамая поздняя запись: {m.Latest()}\n");
            Console.WriteLine("Сортировка дат (с использованием IComparer):");
            Date[] date = new Date[m.memo.GetLength(0)];
            for (int i = 0; i < date.Length; i++)
            {
                date[i] = m.memo[i, 0] as Date;
            }
            Array.Sort(date, new DateComparer());
            foreach (Date d in date)
            {
                Console.WriteLine(d.ToString());
            }
            Console.ReadKey();
        }
    }
}