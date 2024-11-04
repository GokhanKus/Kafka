namespace Kafka.Consumer
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine("Kafka Consumer 1");
			var topicName = "mycluster2-topic";
			var kafkaService = new KafkaService();
			await kafkaService.ConsumeMessageFromCluster(topicName);
			Console.ReadLine();
		}
	}
}
