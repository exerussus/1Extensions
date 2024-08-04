using System;

namespace Exerussus._1Extensions
{
    public static class ProjectTask
    {
        public static void AddCode(string comment = "") { }
        public static void RefactorCode(string comment = "") { }
        public static void TestCode(string comment = "") { }
        public static T TestValue<T>(T value, string comment = "") { return value; }
        public static bool TestTrueCondition(string comment = "") { return true; }
        public static void TestCode(Action action, string comment = "") { action.Invoke(); }
    }
}