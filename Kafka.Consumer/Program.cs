namespace Kafka.Consumer
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var topicName = "use-case-1-topic";
			var kafkaService = new KafkaService();
			await kafkaService.ConsumeSimpleMessageWithNullKey(topicName);
			Console.ReadLine();
		}
	}
}
