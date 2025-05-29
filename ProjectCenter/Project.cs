namespace Exerussus._1Extensions.ProjectCenter
{
    public static class Project
    {
        public static readonly LoggerCore Logger = new ();
        public static readonly ParserCore Parser = new ();
        public static readonly SerializerCore Serializer = new ();
        public static readonly AssetDataBaseCore AssetDataBase = new ();
        public static readonly QoLCore QoL = new ();
        public static readonly UtilsCore Utils = new ();
        public static readonly ConstantsCore Constants = new ();
        
        public class LoggerCore { }
        public class ParserCore { }
        public class SerializerCore { }
        public class QoLCore { }
        public class UtilsCore { }
        public class AssetDataBaseCore { }
        public class ConstantsCore { }
    }
}