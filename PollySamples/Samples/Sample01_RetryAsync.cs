using Polly;

namespace PollySamples.Samples
{
    public class Sample01_RetryAsync : BaseSample
    {
        public static readonly int Attemps = 3;

        protected override IAsyncPolicy ConfigurePolicy()
        { 
            return Policy.Handle<Exception>().RetryAsync(retryCount: Attemps, onRetry: OnSchedulingRetry);
        }

        protected override void PrintHelpText()
        {
            Console.WriteLine("-> RetryAsync method will try up to N times if calls fail\n");
        }

        protected override async Task ExecutePolicyAsync()
        {
            try
            {
                await _policy.ExecuteAsync(async () => await Task.Run(() =>
                {
                    Console.WriteLine($"Running method for the {AddExecution()}st time at {DateTime.Now:HH:mm:ss}");

                    if (_executions < Attemps)
                        throw new Exception();

                    throw new ApplicationException();
                }));
            }
            catch (ApplicationException)
            {
                Console.WriteLine($"\nOn the last attempt it generated the {nameof(ApplicationException)} which was passed to the initial thread!");
            }
        }

        private void OnSchedulingRetry(Exception exception, int attempt, Context context)
        {
            AddAttempt();
            Console.WriteLine($"A failure has occurred at {DateTime.Now:HH:mm:ss} and will try again for the {attempt}st time\n");
        }
    }
}
