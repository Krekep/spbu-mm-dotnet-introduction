using System;
using System.Collections.Generic;

namespace CustomThreadPool
{
    public class MyTask<TResult> : IMyTask<TResult>
    {
        private TResult? result;
        private (AggregateException ex, bool isThrow) exception;
        private readonly Func<TResult> task;
        private readonly MyThreadPool threadPool;
        public bool IsCompleted
        {
            get;
            private set;
        }

        public MyTask(Func<TResult> func, MyThreadPool threadPool)
        {
            this.task = func;
            result = default;
            this.threadPool = threadPool;
            exception = (new AggregateException(), false);
        }

        public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> map)
        {
            IMyTask<TNewResult> newTask = new ContinuedTask<TResult, TNewResult>(map, this, threadPool);
            threadPool.Enqueue(newTask);
            return newTask;
        }

        public TResult GetResult(out bool isSuccess, out AggregateException ex)
        {
            isSuccess = true;
            ex = default;
            while (!IsCompleted)
            {
                if (exception.isThrow)
                {
                    isSuccess = false;
                    ex = exception.ex;
                }
                if (IsCompleted)
                    return result;
                else
                {
                    Thread.Sleep(100);
                }
            }
            if (exception.isThrow)
            {
                isSuccess = false;
                ex = exception.ex;
            }
            return result;
        }

        public void Run()
        {
            try 
            {
                result = task();
            }
            catch (Exception ex) 
            {
                exception = (new AggregateException(ex), true);
            }
            IsCompleted = true;
        }
    }
}
