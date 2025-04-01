using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exerussus._1Extensions.Async;
using UnityEngine;

namespace Exerussus._1Extensions.SmallFeatures
{
 public class JobHandler
    {
        /// <param name="prefix">приписка к логам</param>
        /// <param name="logLevel">уровень логирования</param>
        public JobHandler(string prefix, LogLevel logLevel)
        {
            _prefix = prefix;
            _logLevel = (int)logLevel;
        }

        private readonly string _prefix;
        private readonly int _logLevel;
        private readonly List<Job> _jobQueue = new();
        private readonly List<AsyncJob> _asyncJobQueue = new();
        private readonly List<AsyncJob> _asyncJobDone = new();
        private float _time;
        
        public void AddJob(Action action, string comment, float delay = 0)
        {
#if UNITY_EDITOR
            if (_logLevel > 1) Debug.Log($"{_prefix} | JobHandler | Принято в работу : {comment}");
#endif
            _jobQueue.Add(new Job(action, _time + delay
#if UNITY_EDITOR
                , comment: comment
#endif
                ));
            
        }

        public async Task AddJobAsync(Action action, string comment, float delay = 0, int timeoutMs = 10000)
        {
#if UNITY_EDITOR
            if (_logLevel > 1) Debug.Log($"{_prefix} | JobHandler | Принято в работу : {comment}");
#endif

            var job = new AsyncJob(action, _time + delay
#if UNITY_EDITOR
                , comment: comment
#endif
            );
            
            _asyncJobQueue.Add(job);

            await TaskUtils.WaitUntilAsync(() => job.IsDone, 100, timeoutMs);
            
            _asyncJobDone.Remove(job);
        }

        public void Update()
        {
            _time = Time.realtimeSinceStartup;
            UpdateJobQueue();
            UpdateAsyncJobQueue();
        }

        private void UpdateAsyncJobQueue()
        {
            if (_asyncJobQueue.Count == 0) return;

            for (int i = 0; i < _asyncJobQueue.Count; i++)
            {
                var job = _asyncJobQueue[i];

                if (job.EndTime <= Time.realtimeSinceStartup)
                {
                    ExecuteAsyncJob(i);
                    i--;
                }
            }
        }

        private void UpdateJobQueue()
        {
            if (_jobQueue.Count == 0) return;

            for (int i = 0; i < _jobQueue.Count; i++)
            {
                var job = _jobQueue[i];

                if (job.EndTime <= Time.realtimeSinceStartup)
                {
                    ExecuteJob(i);
                    i--;
                }
            }
        }

        private void ExecuteAsyncJob(int index)
        {
            var job = _asyncJobQueue[index];
            _asyncJobQueue.RemoveAt(index);

            try
            {
                job.Action.Invoke();
#if UNITY_EDITOR
                if (_logLevel > 1) Debug.Log($"JobHandler | Выполнена задача : {job.Comment}");
#endif
            }
            catch (Exception e)
            {
                if (_logLevel > 0) Debug.LogError($"ERROR ! {_prefix} | JobHandler | Ошибка при выполнении асинхронной задачи! |" +
                                                          
#if UNITY_EDITOR
                                                  $" Comment : {job.Comment}." +
#endif
                                                  
                                                  $" Детали:\n{e}");
            }
            finally
            {
                job.IsDone = true;
                _asyncJobDone.Add(job);
            }
        }

        private void ExecuteJob(int index)
        {
            var job = _jobQueue[index];
            _jobQueue.RemoveAt(index);

            try
            {
                job.Action.Invoke();
#if UNITY_EDITOR
                if (_logLevel > 1) Debug.Log($"JobHandler | Выполнена задача : {job.Comment}");
#endif
            }
            catch (Exception e)
            {
                if (_logLevel > 0) Debug.LogError($"ERROR ! {_prefix} | JobHandler | Ошибка при выполнении задачи! |" +
                                                  
#if UNITY_EDITOR
                                                  $" Comment : {job.Comment}." +
#endif
                                                  
                                                  $" Детали:\n{e}");
            }
        }
        
        public enum LogLevel
        {
            None = 0,
            ErrorsOnly = 1,
            All = 2,
        }
    }
    
    public class Job
    {
        public Job(Action action, float endTime
#if UNITY_EDITOR
            , string comment = null
#endif
            )
        {
            EndTime = endTime;
            Action = action;
#if UNITY_EDITOR
            Comment = comment;
#endif
        }
        
#if UNITY_EDITOR
        public string Comment { get; set; }
#endif
        public float EndTime { get; set; }
        public Action Action { get; set; }
    }
    
    public class AsyncJob
    {
        public AsyncJob(Action action, float endTime
#if UNITY_EDITOR
            , string comment = null
#endif
            )
        {
            EndTime = endTime;
            Action = action;
#if UNITY_EDITOR
            Comment = comment;
#endif
        }
        
#if UNITY_EDITOR
        public string Comment { get; set; }
#endif
        public float EndTime { get; set; }
        public Action Action { get; set; }
        public bool IsDone { get; set; }
    }
}