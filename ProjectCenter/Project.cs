namespace Exerussus._1Extensions.ProjectCenter
{
    public static class Project
    {
        public static readonly LoggerCore Logger = new ();
        public static readonly ParserCore Parser = new ();
        public static readonly QoLCore QoL = new ();
        public static readonly UtilsCore Utils = new ();
        
        public class LoggerCore { }
        public class ParserCore { }
        public class QoLCore { }
        public class UtilsCore { }
    }
}