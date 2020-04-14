using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;


namespace scores_generator
{
    public class Program
    {
        private static ConcurrentQueue<Int32> _Scores;
        private static CountdownEvent _countdownEvent;
        private Int32 _numberThreads;
        
        public Program(Int32 numberThreads)
        {
            _numberThreads = numberThreads;
            _Scores = new ConcurrentQueue<int>();
            _countdownEvent = new CountdownEvent(_numberThreads);
        }
        
        static void Main(string[] args)
        {
            var program = new Program(4);
            program.Run();
        }

        private void Run()
        {
            for(int i = 0; i < _numberThreads; i++)
            {
                ThreadPool.QueueUserWorkItem(
                    new WaitCallback(TaskCallBack));
            }
            _countdownEvent.Wait();

            Console.WriteLine("All goddamn threads finished");
            Console.WriteLine($"Number of added Scores in the Queu is at {_Scores.Count}");
        }

        private static void TaskCallBack(object state)
        {
            Console.WriteLine($"Thread #{Thread.CurrentThread.ManagedThreadId} has started its uneasy job..");
            Random gen = new Random();
            while(_Scores.Count < 1000000)
            {
                _Scores.Enqueue(gen.Next(5, 250));
            }
            _countdownEvent.Signal();
        }
    }
}
