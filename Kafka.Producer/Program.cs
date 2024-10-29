using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace Kafka.Producer
{
	internal class Program
	{
		string topicName = "topic_one";
		static async Task Main(string[] args)
		{
			var kafkaService = new KafkaService();
			await kafkaService.CreateTopicAsync();
		}
	}
}
