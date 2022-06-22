using Polly;

namespace PollySamples.Samples
{
    public class Sample02_WaitAndRetryAsync : BaseSample
    {
        public static readonly TimeSpan[] Intervals = new[] {
            TimeSpan.FromSeconds(2),
            TimeSpan.FromSeconds(4),
            TimeSpan.FromSeconds(7),
        };

        protected override IAsyncPolicy ConfigurePolicy()
        {
            return Policy.Handle<Exception>().WaitAndRetryAsync(
                sleepDurations: Intervals,
                onRetry: OnSchedulingRetry);
        }

        protected override void PrintHelpText()
        {
            Console.WriteLine("-> The WaitAndRetryAsync method will wait the configured time to retry up to N times if the calls fail\n");
        }

        protected override async Task ExecutePolicyAsync()
        {
            try
            {
                await _policy.ExecuteAsync(async () => await Task.Run(() =>
                {
                    Console.WriteLine($"Running method for the {++_executions}st time at {DateTime.Now:HH:mm:ss}");

                    if (_executions < Intervals.Length)
                        throw new Exception();

                    throw new ApplicationException();
                }));
            }
            catch (ApplicationException)
            {
                Console.WriteLine($"\nOn the last attempt it generated the {nameof(ApplicationException)} which was passed to the initial thread!");
            }
        }

        private void OnSchedulingRetry(Exception exception, TimeSpan interval, int attempt, Context context)
        {
            AddAttempt();
            Console.WriteLine($"A failure has occurred at {DateTime.Now:HH:mm:ss} and will try again for the {attempt}st time after {interval.TotalSeconds} seconds\n");
        }
    }
}
