using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace Kafka.Producer
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var kafkaService = new KafkaService();
			var topicName = "mycluster2-topic";
			await kafkaService.CreateTopicWithClusterAsync(topicName);
			await kafkaService.SendMessageToCluster(topicName);

			Console.WriteLine("mesajlar gonderilmistir");
		}
	}
}
