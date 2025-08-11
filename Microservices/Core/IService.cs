using Cysharp.Threading.Tasks;

namespace Exerussus._1Extensions.MicroserviceFeature
{
    public interface IService
    {
        
    }
    
    public interface IService<in T1> : IService 
        where T1 : IChannel
    {
        public UniTask PullBroadcast(T1 channel);
    }
    
    public interface IService<in T1, in T2> : IService 
        where T1 : IChannel
        where T2 : IChannel
    {
        public UniTask PullBroadcast(T1 channel);
        public UniTask PullBroadcast(T2 channel);
    }
    
    public interface IService<in T1, in T2, in T3> : IService 
        where T1 : IChannel
        where T2 : IChannel
        where T3 : IChannel
    {
        public UniTask PullBroadcast(T1 channel);
        public UniTask PullBroadcast(T2 channel);
        public UniTask PullBroadcast(T3 channel);
    }
    
    public interface IService<in T1, in T2, in T3, in T4> : IService 
        where T1 : IChannel
        where T2 : IChannel
        where T3 : IChannel
        where T4 : IChannel
    {
        public UniTask PullBroadcast(T1 channel);
        public UniTask PullBroadcast(T2 channel);
        public UniTask PullBroadcast(T3 channel);
        public UniTask PullBroadcast(T4 channel);
    }
    
    public interface IService<in T1, in T2, in T3, in T4, in T5> : IService 
        where T1 : IChannel
        where T2 : IChannel
        where T3 : IChannel
        where T4 : IChannel
        where T5 : IChannel
    {
        public UniTask PullBroadcast(T1 channel);
        public UniTask PullBroadcast(T2 channel);
        public UniTask PullBroadcast(T3 channel);
        public UniTask PullBroadcast(T4 channel);
        public UniTask PullBroadcast(T5 channel);
    }
    
    public interface IService<in T1, in T2, in T3, in T4, in T5, in T6> : IService 
        where T1 : IChannel
        where T2 : IChannel
        where T3 : IChannel
        where T4 : IChannel
        where T5 : IChannel
        where T6 : IChannel
    {
        public UniTask PullBroadcast(T1 channel);
        public UniTask PullBroadcast(T2 channel);
        public UniTask PullBroadcast(T3 channel);
        public UniTask PullBroadcast(T4 channel);
        public UniTask PullBroadcast(T5 channel);
        public UniTask PullBroadcast(T6 channel);
    }   
    
    public interface IService<in T1, in T2, in T3, in T4, in T5, in T6, in T7> : IService 
        where T1 : IChannel
        where T2 : IChannel
        where T3 : IChannel
        where T4 : IChannel
        where T5 : IChannel
        where T6 : IChannel
        where T7 : IChannel
    {
        public UniTask PullBroadcast(T1 channel);
        public UniTask PullBroadcast(T2 channel);
        public UniTask PullBroadcast(T3 channel);
        public UniTask PullBroadcast(T4 channel);
        public UniTask PullBroadcast(T5 channel);
        public UniTask PullBroadcast(T6 channel);
        public UniTask PullBroadcast(T7 channel);
    }
    
    public interface IService<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8> : IService 
        where T1 : IChannel
        where T2 : IChannel
        where T3 : IChannel
        where T4 : IChannel
        where T5 : IChannel
        where T6 : IChannel
        where T7 : IChannel
        where T8 : IChannel
    {
        public UniTask PullBroadcast(T1 channel);
        public UniTask PullBroadcast(T2 channel);
        public UniTask PullBroadcast(T3 channel);
        public UniTask PullBroadcast(T4 channel);
        public UniTask PullBroadcast(T5 channel);
        public UniTask PullBroadcast(T6 channel);
        public UniTask PullBroadcast(T7 channel);
        public UniTask PullBroadcast(T8 channel);
    }

    public interface IService<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9> : IService
        where T1 : IChannel
        where T2 : IChannel
        where T3 : IChannel
        where T4 : IChannel
        where T5 : IChannel
        where T6 : IChannel
        where T7 : IChannel
        where T8 : IChannel
        where T9 : IChannel
    {
        public UniTask PullBroadcast(T1 channel);
        public UniTask PullBroadcast(T2 channel);
        public UniTask PullBroadcast(T3 channel);
        public UniTask PullBroadcast(T4 channel);
        public UniTask PullBroadcast(T5 channel);
        public UniTask PullBroadcast(T6 channel);
        public UniTask PullBroadcast(T7 channel);
        public UniTask PullBroadcast(T8 channel);
        public UniTask PullBroadcast(T9 channel);
    }
    
    public interface IService<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10> : IService
        where T1 : IChannel
        where T2 : IChannel
        where T3 : IChannel
        where T4 : IChannel
        where T5 : IChannel
        where T6 : IChannel
        where T7 : IChannel
        where T8 : IChannel
        where T9 : IChannel
        where T10 : IChannel
    {
        public UniTask PullBroadcast(T1 channel);
        public UniTask PullBroadcast(T2 channel);
        public UniTask PullBroadcast(T3 channel);
        public UniTask PullBroadcast(T4 channel);
        public UniTask PullBroadcast(T5 channel);
        public UniTask PullBroadcast(T6 channel);
        public UniTask PullBroadcast(T7 channel);
        public UniTask PullBroadcast(T8 channel);
        public UniTask PullBroadcast(T9 channel);
        public UniTask PullBroadcast(T10 channel);
    }
}