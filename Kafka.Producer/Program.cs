using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace Kafka.Producer
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var kafkaService = new KafkaService();
			var topicName = "retry-topic";
			//await kafkaService.CreateTopicRetryWithClusterAsync(topicName);
			await kafkaService.SendMessageWithRetryToCluster(topicName);
			Console.WriteLine("mesajlar gonderilmistir");
		}
	}
}
