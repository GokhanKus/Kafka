using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace Kafka.Producer
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var kafkaService = new KafkaService();
			var topicName = "use-case-3-topic";
			await kafkaService.CreateTopicAsync(topicName);
			await kafkaService.SendComplexMessageWithIntKeyAndHeader(topicName);

			Console.WriteLine("mesajlar gonderilmistir");
		}
	}
}
