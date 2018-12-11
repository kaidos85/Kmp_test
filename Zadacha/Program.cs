using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadacha
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Example> init = new List<Example>();
            if(args.Length != 26)
            {
                init = Init();
            }
            else
            {
                init.Add(new Example(args));
            }
            FindMin(init);
            Print(init);
            Console.ReadKey();
        }

        static List<Example> Init()
        {
            var list = new List<Example>();
            list.Add(new Example { InputArray = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 2 } });
            list.Add(new Example { InputArray = new[] { 2, 2, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } });
            list.Add(new Example { InputArray = new[] { 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 } });
            return list;
        }

        static void FindMin(List<Example> list)
        {
            foreach (var item in list)
            {
                var res = Dtosym.GetList(item.InputArray);
                var abcList = GetResult(res);
                var minAbc = abcList.OrderBy(c => c.KMPSum).ThenBy(c=>c.Pattern).FirstOrDefault();
                if(minAbc != null)
                {
                    item.KMP = minAbc.KMP;
                    item.KMPSum = minAbc.KMPSum;
                    item.Result = minAbc.Pattern;
                }
            }
        }

        static void Print(List<Example> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine("--------------------");
                Console.WriteLine("input array: {0}", string.Join(" ", item.InputArray.Select(c=>c.ToString())));
                Console.WriteLine("word: {0}", item.Result);
                Console.WriteLine("kmp: {0}", string.Join(" ", item.KMP.Select(c=>c.ToString())));
                Console.WriteLine("min kmp sum: {0}", item.KMPSum);
                Console.WriteLine("--------------------");
            }
        }

        static List<Abc> GetResult(List<Dtosym> dto)
        {
            var list = new List<Abc>();
            string fPattern = string.Empty;
            foreach (var el in dto)
            {
                for (int i = 0; i < el.Count; i++)
                {
                    fPattern += el.Symbol;
                }
            }
            var chArr = fPattern.ToCharArray();
            do
            {
                list.Add(new Abc { Pattern =  new string(chArr) });
            } while (Narayana.NextPermutation(chArr));
            return list;
        }

    }

    public class Dtosym
    {
        public string Symbol { get; set; }
        public int Count { get; set; }

        public static List<Dtosym> GetList(int[] input)
        {
            var list = new List<Dtosym>();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] > 0)
                    list.Add(new Dtosym
                    {
                        Symbol = ((char)(97 + i)).ToString(),
                        Count = input[i]
                    });
            }
            return list;
        }
    }

    public class Abc
    {
        public string Pattern { get; set; }
        public int[] KMP
        {
            get
            {
                return GetPrefix(Pattern);
            }
        }

        public int KMPSum
        {
            get
            {
                return KMP.Sum(c => c);
            }
        }

        static int[] GetPrefix(string s)
        {
            int[] result = new int[s.Length];
            result[0] = 0;
            int index = 0;

            for (int i = 1; i < s.Length; i++)
            {
                while (index >= 0 && s[index] != s[i]) { index--; }
                index++;
                result[i] = index;
            }

            return result;
        }

    }

    public static class Narayana
    {

        public static bool NextPermutation(char[] sequence)
        {            
            var i = sequence.Length;
            do
            {
                if (i < 2)
                    return false; 
                --i;
            } while (!Compare(sequence[i - 1], sequence[i]));
            
            var j = sequence.Length;
            while (i < j && !Compare(sequence[i - 1], sequence[--j])) ;
                Swap(sequence, i - 1, j);
            j = sequence.Length;
            while (i < --j)
                Swap(sequence, i++, j);
            return true;
        }

        private static void Swap(char[] sequence, int index_0, int index_1)
        {
            var item = sequence[index_0];
            sequence[index_0] = sequence[index_1];
            sequence[index_1] = item;
        }

        private static bool Compare(char value_0, char value_1)
        {
            return value_0.CompareTo(value_1) < 0;
        }
    }

    public class Example
    {
        public int[] InputArray { get; set; }        
        public string Result { get; set; }
        public int[] KMP { get; set; }
        public int KMPSum { get; set; }

        public Example()
        {

        }

        public Example(int[] inputArray)
        {
            InputArray = inputArray;
        }

        public Example(string[] inputStr)
        {
            var intArr = inputStr.Select(c => TryParseInt(c)).Where(c => c != null).Cast<int>();
            if (inputStr.Length != 26 && intArr.Count() != 26)
                throw new Exception("need 26");
            InputArray = intArr.ToArray();
        }

        int? TryParseInt(string str)
        {
            int res = 0;
            if (int.TryParse(str, out res))
                return res;
            return null;
        }
    }
}
