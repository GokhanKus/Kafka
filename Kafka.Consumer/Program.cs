namespace Kafka.Consumer
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine("Kafka Consumer 1");
			var topicName = "use-case-5-topic";
			var kafkaService = new KafkaService();
			await kafkaService.ConsumeMessageInTheSpecificPartitionOffSet(topicName);
			Console.ReadLine();
		}
	}
}
