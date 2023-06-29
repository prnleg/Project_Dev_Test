using System.Collections.Concurrent;

namespace Project_Dev_Test.Web.Service
{
    public class ProcessQueueService
    {
        private const int NUM_THREADS = 2;
        private SemaphoreSlim semaphore;

        public ProcessQueueService()
        {
            Start(NUM_THREADS);
        }

        private void Start(int threads)
        {
            semaphore = new SemaphoreSlim(threads);
        }

        public async Task Enqueue(Task task)
        {
            await EnqueueRun(() =>
            {
                task.Start();
                return task;
            });
        }

        private async Task EnqueueRun(Func<Task> task)
        {
            Console.WriteLine($"Enqueued task");
            await semaphore.WaitAsync();
            try
            {
                Console.WriteLine($"Start running task");
                await task.Invoke();
            }
            finally
            {
                Console.WriteLine($"Finish running task");
                semaphore.Release();
            }
        }

    }
}
