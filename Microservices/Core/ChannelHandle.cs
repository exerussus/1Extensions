using Cysharp.Threading.Tasks;

namespace Exerussus._1Extensions.MicroserviceFeature
{
    public readonly struct ChannelHandle
    {
        internal ChannelHandle(int id)
        {
            _id = id;
        }

        private readonly int _id;

        public bool IsValid() { return true; }
        public bool IsDone() { return true; }
        public bool TryCancel() { return true; }
        public void Cancel() {  }
        public UniTask Wait() { return default; }
    }
}