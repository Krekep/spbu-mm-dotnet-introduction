
namespace CustomThreadPool
{
    public interface IMyTask
    {
        void Run();

        bool IsCompleted { get; }
    }

    public interface IMyTask<TResult> : IMyTask
    {
        bool IsCompleted { get; }
        public TResult GetResult(out bool isSuccess, out AggregateException ex);
        IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> map);
    }
}
