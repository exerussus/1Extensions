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
        /// <param name="isProtected"></param>
        public JobHandler(LogLevel logLevel = LogLevel.None, bool isProtected = true)
        {
            _logLevel = (int)logLevel;
            _isProtected = isProtected;
        }

        private readonly int _logLevel;
        private readonly List<Job> _jobQueue = new();
        private readonly List<AsyncJob> _asyncJobQueue = new();
        private readonly List<AsyncJob> _asyncJobDone = new();
        private readonly bool _isProtected;
        private float _time;
        
        public void AddJob(Action action, float delay = 0)
        {
            _jobQueue.Add(new Job(action, _time + delay));
        }

        public async Task AddJobAsync(Action action, float delay = 0, int timeoutMs = 10000)
        {
            var job = new AsyncJob(action, _time + delay);
            
            _asyncJobQueue.Add(job);
            
            await TaskUtils.WaitUntilCondition(() => job.IsDone, 100, timeoutMs);
            
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

            if (_isProtected)
            {
                try
                {
                    job.Action.Invoke();
                }
                catch (Exception e)
                {
                    if (_logLevel > 0) Debug.LogError($"JobHandler ERROR! | Ошибка при выполнении асинхронной задачи! | Детали:\n{e}");
                }
                finally
                {
                    job.IsDone = true;
                    _asyncJobDone.Add(job);
                }
            }
            else
            {
                job.Action.Invoke();
                job.IsDone = true;
                _asyncJobDone.Add(job);
            }
        }

        private void ExecuteJob(int index)
        {
            var job = _jobQueue[index];
            _jobQueue.RemoveAt(index);
            
            if (_isProtected)
            {
                try
                {
                    job.Action.Invoke();
                }
                catch (Exception e)
                {
                    if (_logLevel > 0) Debug.LogError($"JobHandler ERROR! | Ошибка при выполнении задачи! | Детали:\n{e}");
                }
            }
            else
            {
                job.Action.Invoke();
            }
        }
        
        public enum LogLevel
        {
            None = 0,
            Errors = 1,
        }
    }
    
    public class Job
    {
        public Job(Action action, float endTime)
        {
            EndTime = endTime;
            Action = action;
        }

        public float EndTime { get; set; }
        public Action Action { get; set; }
    }
    
    public class AsyncJob
    {
        public AsyncJob(Action action, float endTime)
        {
            EndTime = endTime;
            Action = action;
        }
        
        public float EndTime { get; set; }
        public Action Action { get; set; }
        public bool IsDone { get; set; }
    }
}