namespace Exerussus._1Extensions.ProjectCenter
{
    public static partial class Project
    {
        public static readonly LoggerCore Logger = new ();
        public static readonly ParserCore Parser = new ();
        public static readonly SerializerCore Serializer = new ();
        public static readonly AssetDataBaseCore AssetDataBase = new ();
        public static readonly QoLCore QoL = new ();
        public static readonly UtilsCore Utils = new ();
        public static readonly ConstantsCore Constants = new ();
        
        public partial class LoggerCore { }
        public partial class ParserCore { }
        public partial class SerializerCore { }
        public partial class QoLCore { }
        public partial class UtilsCore { }
        public partial class AssetDataBaseCore { }
        public partial class ConstantsCore { }
    }
}