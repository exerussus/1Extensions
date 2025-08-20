using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Exerussus._1Extensions.SmallFeatures;
using UnityEngine.Scripting;

namespace Exerussus._1Extensions.MicroserviceFeature
{
    public interface IService
    {
        
    }
    
    public interface IChannelPuller<in TChannel>
        where TChannel : IChannel
    {
        public UniTask PullBroadcast(TChannel channel);
        
        [Preserve]
        internal void RegisterPuller()
        {
            Func<TChannel, UniTask> pull = PullBroadcast;
            Microservices.RegisterPuller(this, pull);
        }
    }
    
    public interface IChannelPusher<in TChannel>
        where TChannel : IChannel
    {
        
    }
    
    public interface IServiceInspector
    {
        public Dictionary<Type, object> ChannelsSubs {get; set; }
        public Dictionary<Type, RegisteredService> RegisteredServices {get; set; }
        public GameShare GameShare {get; set; }
        public Dictionary<Type, HashSet<Type>> PushersToChannels {get; set; }
        public Dictionary<Type, HashSet<Type>> ChannelsToPullers {get; set; }
        public object LockChannelsPullers { get; set; }
    }
}