using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Shared.Events;

namespace Order.API.Services
{
	public class Bus(IConfiguration configuration, ILogger<Bus> logger) : IBus
	{
		private readonly ProducerConfig config = new() //primarly constructor geleneksel ctordan farklı olarak..
		{
			BootstrapServers = configuration.GetSection(key: "BusSettings").GetSection(key: "Kafka")["BootstrapServers"],
			Acks = Acks.All,
			MessageTimeoutMs = 5000,
			AllowAutoCreateTopics = true, //daha once topic yoksa gider topic olusturur ardindan mesaj gonderir
		};
		public async Task<bool> Publish<T1, T2>(T1 key, T2 value, string topicQueueName)
		{
			using var producer = new ProducerBuilder<T1, T2>(config)
				.SetKeySerializer(new CustomKeySerializer<T1>())
				.SetValueSerializer(new CustomValueSerializer<T2>())
				.Build();

			var message = new Message<T1, T2>()
			{
				Key = key,
				Value = value
			};

			var result = await producer.ProduceAsync(topicQueueName, message);
			return result.Status == PersistenceStatus.Persisted; //5sn icinde gonderebilirse true, yoksa false exception fırlatacak
		}
		public async Task CreateTopicOrQueue(List<string> topicOrQueueNameList)
		{
			using var adminClient = new AdminClientBuilder(new AdminClientConfig
			{
				BootstrapServers = configuration.GetSection(key: "BusSettings").GetSection(key: "Kafka")["BootstrapServers"],
			}).Build();

			try
			{
				foreach (var topicOrQueue in topicOrQueueNameList)
				{
					await adminClient.CreateTopicsAsync(new[]{
					new TopicSpecification{Name = topicOrQueue, NumPartitions = 6, ReplicationFactor = 1}
					});
					logger.LogInformation($"topic ({topicOrQueue}) olustu");
				}
			}
			catch (Exception ex)
			{
				logger.LogWarning(ex.Message);  
			}
		}
	}
}
