using System.Threading;
using Cysharp.Threading.Tasks;

namespace Exerussus._1Extensions.SmallFeatures
{
    public class ObjectPackage<T>
    {
        public T Value;
    }

    public static class ObjectPackageExtensions
    {
        public static async UniTask AwaitDone(this ObjectPackage<bool> package, int timeout = 20000, CancellationToken ct = default)
        {
            const int checkInterval = 100;
            
            while (!package.Value || !ct.IsCancellationRequested)
            {
                if (timeout <= 0) return;
                await UniTask.Delay(checkInterval, cancellationToken: ct);
                timeout -= checkInterval;
            }
        }
    }
}