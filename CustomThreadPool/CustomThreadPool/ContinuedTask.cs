
using System;

namespace CustomThreadPool
{
    internal class ContinuedTask<TResult, TNewResult> : IMyTask<TNewResult>
    {
        private TNewResult? result;
        private (AggregateException ex, bool isThrow) exception;
        private readonly Func<TResult, TNewResult> task;
        private readonly IMyTask<TResult> parentTask;
        private readonly MyThreadPool threadPool;
        private volatile bool isCompleted;
        public bool IsCompleted
        {
            get
            {
                return isCompleted;
            }
            private set
            {
                isCompleted = value;
            }
        }

        public ContinuedTask(Func<TResult, TNewResult> func, IMyTask<TResult> parentTask, MyThreadPool threadPool)
        {
            this.task = func;
            this.parentTask = parentTask;
            result = default;
            this.threadPool = threadPool;
            isCompleted = false;
        }

        public IMyTask<TNewResultContinue> ContinueWith<TNewResultContinue>(Func<TNewResult, TNewResultContinue> map)
        {
            IMyTask<TNewResultContinue> newTask = new ContinuedTask<TNewResult, TNewResultContinue>(map, this, threadPool);
            threadPool.Enqueue(newTask);
            return newTask;
        }


        public TNewResult GetResult(out bool isSuccess, out AggregateException ex)
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
                    Thread.Sleep(50);
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
            bool isSuccess;
            AggregateException exception;
            TResult x = parentTask.GetResult(out isSuccess, out exception);
            if (isSuccess)
            {
                result = task(x);
            }
            else
            {
                this.exception = (exception, true);
            }
            IsCompleted = true;
        }
    }
}
