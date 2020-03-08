using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MontagneRusse
{
    public static class InputData
    {

        public static Func<List<string>> GetStage(int i)
        {
            if (i == 1)
                return Stage1;
            else if (i == 2)
                return Stage2;
            else if (i == 3)
                return Stage3;
            else if (i == 4)
                return Stage4;
            else if (i == 5)
                return Stage5;
            else if (i == 6)
                return Stage6;
            else
                return null;
        }

        public static string GetInput(int i)
        {
            if (i == 1)
                return "3 3 4";
            else if (i == 2)
                return "5 3 4";
            else if (i == 3)
                return "10 100 1";
            else if (i == 4)
                return "10000 10 5";
            else if (i == 5)
                return "100000 50000 1000";
            else if (i == 6)
                return "10000000 9000000 1000";
            else
                return null;
        }

        public static int GetResult(int i)
        {
            if (i == 1)
                return 7;
            else if (i == 2)
                return 14;
            else if (i == 3)
                return 100;
            else if (i == 4)
                return 15000;
            else
                return -1;
        }

        private static List<string> Stage1()
        {
            var r = new List<string> { "3", "1", "1", "2" };
            return r;
        }
        private static List<string> Stage2()
        {
            var r = new List<string> { "2", "3", "5", "4" };
            return r;
        }
        private static List<string> Stage3()
        {
            var r = new List<string> { "1" };
            return r;
        }
        private static List<string> Stage4()
        {
            var r = new List<string> { "100", "200", "300", "400", "500" };
            return r;
        }
        private static List<string> Stage5()
        {
            var r = Enumerable.Range(0, 1000)
                .Select(i => i % 2 == 0 ? "100000" : "99999")
                .ToList();
            return r;
        }

        private static List<string> Stage6()
        {
            var rnd = new Random();
            var r = Enumerable.Range(0, 1000)
                .Select(i => rnd.Next(10000, 100000).ToString())
                .ToList();
            return r;
        }
    }
}
