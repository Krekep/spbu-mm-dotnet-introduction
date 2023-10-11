using CustomThreadPool;

using System.Threading;

namespace UnitTest
{
    [TestClass]
    public class TestThreadPool
    {
        [TestMethod]
        public void TestOneTask()
        {
            MyThreadPool thread = new MyThreadPool(1);
            Func<string> func = () =>
            {
                return "Some test";
            }; 
            IMyTask<string> task = new MyTask<string>(func, thread);
            thread.Enqueue<string>(task);
            thread.Dispose();

            Assert.AreEqual("Some test", task.GetResult(out _, out _));
            Assert.IsTrue(task.IsCompleted);
        }

        [TestMethod]
        public void TestManyTasks()
        {
            List<int> expectedResults = new List<int>();

            int n = 10;
            MyThreadPool thread = new MyThreadPool(2);
            List<IMyTask<int>> functions = new List<IMyTask<int>>();
            for (int i = 0; i < n; i++)
            {
                int temp = i;
                Func<int> func = () =>
                {
                    int s = 0;
                    for (int j = 0; j < 1_000_000 + temp; j++)
                    { s += 1; }
                    return s;
                };
                IMyTask<int> task = new MyTask<int>(func, thread);
                functions.Add(task);
                thread.Enqueue<int>(task);

                expectedResults.Add(1_000_000 + temp);
            }
            for (int i = 0; i < n; i++)
            {
                Assert.AreEqual(expectedResults[i], functions[i].GetResult(out _, out _));
            }
            thread.Dispose();
        }

        [TestMethod]
        public void TestManyThreads()
        {
            List<int> expectedResults = new List<int>();

            int n = 10;
            MyThreadPool thread = new MyThreadPool(100);
            List<IMyTask<int>> functions = new List<IMyTask<int>>();
            for (int i = 0; i < n; i++)
            {
                int temp = i;
                Func<int> func = () =>
                {
                    int s = 0;
                    for (int j = 0; j < 1_000_000 + temp; j++)
                    { s += 1; }
                    return s;
                };
                IMyTask<int> task = new MyTask<int>(func, thread);
                functions.Add(task);
                thread.Enqueue<int>(task);

                expectedResults.Add(1_000_000 + temp);
            }
            for (int i = 0; i < n; i++)
            {
                Assert.AreEqual(expectedResults[i], functions[i].GetResult(out _, out _));
            }
            thread.Dispose();
        }

        [TestMethod]
        public void TestException()
        {
            MyThreadPool thread = new MyThreadPool(1);
            Func<string> func = () =>
            {
                throw new DivideByZeroException("My divide by zero exception");
                return "A";
            };
            IMyTask<string> task = new MyTask<string>(func, thread);
            thread.Enqueue<string>(task);
            thread.Dispose();

            bool isSuccess;
            AggregateException exception;
            string result = task.GetResult(out isSuccess, out exception);
            Assert.IsFalse(isSuccess);
            Assert.AreEqual("My divide by zero exception", exception.InnerException.Message);
            Assert.IsTrue(task.IsCompleted);
        }

        [TestMethod]
        public void TestContinueWith()
        {
            List<int> expectedResults = new List<int>();

            MyThreadPool thread = new MyThreadPool(4);
            Func<int> func = () =>
            {
                return 1;
            };
            IMyTask<int> task = new MyTask<int>(func, thread);
            thread.Enqueue(task);
            int result = task.GetResult(out _, out _);

            int n = 10;
            List<IMyTask<int>> functions = new List<IMyTask<int>>();
            for (int i = 0; i < n; i++)
            {
                Func<int, int> contFunc = (int x) =>
                {
                    return x * 2;
                };
                task = task.ContinueWith(contFunc);
                functions.Add(task);
                thread.Enqueue<int>(task);

                expectedResults.Add((int)Math.Pow(2, i + 1));
            }

            for (int i = 0; i < n; i++)
            {
                Assert.AreEqual(expectedResults[i], functions[i].GetResult(out _, out _));
            }
            thread.Dispose();
        }

        [TestMethod]
        public void TestThreadCount()
        {
            int expected = 4;
            HashSet<int> actualThreads = new HashSet<int>();

            MyThreadPool thread = new MyThreadPool(expected);

            int n = 10;
            List<IMyTask<int>> functions = new List<IMyTask<int>>();
            for (int i = 0; i < n; i++)
            {
                Func<int> func = () =>
                {
                    Thread.Sleep(100);
                    return Thread.CurrentThread.ManagedThreadId;
                };
                IMyTask<int> task = new MyTask<int>(func, thread);
                functions.Add(task);
                thread.Enqueue<int>(task);
            }

            for (int i = 0; i < n; i++)
            {
                actualThreads.Add(functions[i].GetResult(out _, out _));
            }
            thread.Dispose();

            Assert.AreEqual(expected, actualThreads.Count);
        }
    }
}