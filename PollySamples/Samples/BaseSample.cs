using Polly;

namespace PollySamples.Samples
{
    public abstract class BaseSample
    {
        protected IAsyncPolicy _policy;
        protected int _executions;
        protected int _attempts;

        protected BaseSample()
        {
            PrintSampleTitle();
            _policy = ConfigurePolicy();
        }

        private void PrintSampleTitle()
        {
            const int textWidth = 100;

            var title = GetType().Name.Replace("_", " with ");

            Console.WriteLine();
            Console.WriteLine(new string('#', textWidth));
            Console.WriteLine($"# {title}".PadRight(textWidth - 1, ' ') + "#");
            Console.WriteLine(new string('#', textWidth));
            Console.WriteLine();

            PrintHelpText();
        }

        public async Task ExecuteAsync()
        {
            ResetCounters();

            await ExecutePolicyAsync();

            Console.WriteLine(
                $"\nTotal of {nameof(_executions)}.: {_executions}" +
                $"\nTotal of {nameof(_attempts)}...: {_attempts}");
        }

        protected abstract IAsyncPolicy ConfigurePolicy();

        protected abstract Task ExecutePolicyAsync();

        protected virtual void PrintHelpText()
        {
            // optional implementation
        }

        private void ResetCounters()
        {
            _executions = 0;
            _attempts = 0;
        }

        protected int AddExecution() => ++_executions;
        protected int AddAttempt() => ++_attempts;
    }
}
