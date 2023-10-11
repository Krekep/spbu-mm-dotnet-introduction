using System.Threading.Tasks;

namespace CustomThreadPool
{
    public class MyThreadPool : IDisposable
    {
        private int threadCount = 1;
        private Thread[] threads;
        private Queue<IMyTask> taskQueue;
        private List<(IMyTask startTask, IMyTask continuedTask)> continuedQueue;
        private bool isRunning = true;
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;
        private bool disposed;
        private Thread supportThread;

        public MyThreadPool(int threadCount) 
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;

            disposed = false;
            taskQueue = new Queue<IMyTask>();
            continuedQueue = new List<(IMyTask, IMyTask)>();
            supportThread = new Thread(() => { ClearContinuedQueue(); });

            this.threadCount = threadCount;
            threads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(() => { Run(cancellationToken); });
                threads[i].Start();
            }
        }

        private void FreeObject(object obj)
        {
            Monitor.Enter(obj);
            Monitor.PulseAll(obj);
            Monitor.Exit(obj);
        }

        private void Run(CancellationToken token)
        {
            while (!token.IsCancellationRequested || taskQueue.Count > 0)
            {
                //if (taskQueue.Count > 0)
                //{
                //    if (Monitor.TryEnter(taskQueue))
                //    {
                //        var task = taskQueue.Dequeue();
                //        Monitor.Exit(taskQueue);
                //        task.Run();
                //    }
                //}
                //else
                //{
                //    Thread.Sleep(100);
                //}

                Monitor.Enter(taskQueue);
                if (taskQueue.Count > 0)
                {
                        var task = taskQueue.Dequeue();
                        Monitor.PulseAll(taskQueue);
                        Monitor.Exit(taskQueue);
                        task.Run();
                }
                else
                {
                    Monitor.Wait(taskQueue);
                    Monitor.Exit(taskQueue);
                }
            }
            FreeObject(taskQueue);
        }

        public void Enqueue<TResult>(IMyTask<TResult> newTask)
        {
            while (true)
            {
                if (isRunning)
                {
                    if (Monitor.TryEnter(taskQueue))
                    {
                        taskQueue.Enqueue((IMyTask)newTask);
                        Monitor.PulseAll(taskQueue);
                        Monitor.Exit(taskQueue);
                        break;
                    }
                }
                else
                    break;
            }
        }

        private void ClearContinuedQueue()
        {
            while (isRunning)
            {
                int n = continuedQueue.Count;
                for (int i = 0; i < n; i++)
                {
                    if (continuedQueue[i].startTask.IsCompleted)
                    {
                        Monitor.Enter(continuedQueue);
                        Monitor.Enter(taskQueue);
                        var newTask = continuedQueue[i].continuedTask;
                        taskQueue.Enqueue(newTask);
                        continuedQueue.RemoveAt(i);
                        Monitor.Exit(taskQueue);
                        Monitor.Exit(continuedQueue);
                    }
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    isRunning = false;
                    cancellationTokenSource.Cancel();
                    FreeObject(taskQueue);
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}