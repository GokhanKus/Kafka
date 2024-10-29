using Confluent.Kafka.Admin;
using Confluent.Kafka;

namespace Kafka.Producer
{
	internal class KafkaService
	{
		private const string TopicName = "mytopic";
		internal async Task CreateTopicAsync()
		{
			using var adminClient = new AdminClientBuilder(new AdminClientConfig
			{
				BootstrapServers = "localhost:9094"
			}).Build();
			try
			{
				await adminClient.CreateTopicsAsync(new[]{
				new TopicSpecification{Name = TopicName,NumPartitions = 3,ReplicationFactor = 1}
				});
				Console.WriteLine($"topic ({TopicName}) olustu");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}