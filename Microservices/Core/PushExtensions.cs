using Cysharp.Threading.Tasks;

namespace Exerussus._1Extensions.MicroserviceFeature
{
    public static class PushExtensions
    {
        public static async UniTask PushBroadcast<T>(this T channel) where T : IChannel
        {
            await Microservices.PushBroadcast(channel);
        }
    }
}