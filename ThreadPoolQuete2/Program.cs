using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace ThreadPoolQuete2
{
    class Program
    {
        private CountdownEvent _countdown;
        private Int32 _threadsCount;
        public static ConcurrentQueue<int> numberList = new ConcurrentQueue<int>();
        public Random random = new Random();

        static void Main(string[] args)
        {
            var program = new Program(1000000);
            program.Run();
        }

        public Program(Int32 threadsCount)
        {
            _threadsCount = threadsCount;
            _countdown = new CountdownEvent(threadsCount); 
        }

        public void Run()
        {
            Console.WriteLine("[INFO]: Thread n°{0} has started",Thread.CurrentThread.ManagedThreadId);
            for (int i = 0; i < _threadsCount; i++)
            {
                ThreadPool.QueueUserWorkItem(x =>
                {
                    int randomNumber = random.Next(1, 50);
                    numberList.Enqueue(randomNumber);
                    _countdown.Signal();
                });
            }

            while (_countdown.CurrentCount > 0)
            {
                Console.WriteLine("[INFO]: Waiting for threads to terminate ...");
                Thread.Sleep(100);
            }

            _countdown.Wait(); // Blocks current thread until the CountdownEvent counter is equal to 0

            Console.WriteLine("[SUCESS]: {0} numbers were added", numberList.Count());
            Console.ReadLine();
        }
    }
}
