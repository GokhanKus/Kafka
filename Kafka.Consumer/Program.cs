namespace Kafka.Consumer
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine("Kafka Consumer 1");
			var topicName = "ack-topic";
			var kafkaService = new KafkaService();
			await kafkaService.ConsumeMessageWithAcknowledgement(topicName);
			Console.ReadLine();
		}
	}
}
