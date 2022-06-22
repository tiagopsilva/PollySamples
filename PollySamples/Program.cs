using PollySamples.Samples;

Console.WriteLine("Polly basic samples");

await new Sample01_RetryAsync().ExecuteAsync();
await new Sample02_WaitAndRetryAsync().ExecuteAsync();