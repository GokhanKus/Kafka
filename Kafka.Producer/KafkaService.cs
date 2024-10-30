using Confluent.Kafka.Admin;
using Confluent.Kafka;

namespace Kafka.Producer
{
	internal class KafkaService
	{
		internal async Task CreateTopicAsync(string topicName)
		{
			using var adminClient = new AdminClientBuilder(new AdminClientConfig
			{
				BootstrapServers = "localhost:9094"
			}).Build();
			try
			{
				await adminClient.CreateTopicsAsync(new[]{
				new TopicSpecification{Name = topicName,NumPartitions = 3,ReplicationFactor = 1}
				});
				Console.WriteLine($"topic ({topicName}) olustu");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
		internal async Task SendSimpleMessageWithNullKey(string topicName)
		{
			var config = new ProducerConfig { BootstrapServers = "localhost:9094" };

			using var producer = new ProducerBuilder<Null, string>(config).Build();

			foreach (var item in Enumerable.Range(1, 10))
			{
				var message = new Message<Null, string>()
				{
					Value = $"Message(use-case-1) - {item}"
				};
				var result = await producer.ProduceAsync(topicName, message);

				foreach (var propertyInfo in result.GetType().GetProperties())
				{
					Console.WriteLine($"{propertyInfo.Name}:{propertyInfo.GetValue(result)}");
				}
				Console.WriteLine(new string('-', 50));
				await Task.Delay(200);
			}
		}
	}
}
/*
Producer(Serialize) --> Kafka Broker --> Consumer(Deserialize)
Kafka broker ise mesajlari once ram'e sonra da diske kaydeder ya da;
zero copy ile ram olmadan dogrudan diske de kaydedebilir daha performansli ancak ssl olmamasi lazim 

Message 2 parcadan olusur (key, value) key null olursa mesaj rastgele bir partition'a gider 
key degeri verilirse ilgili hash algoritmasından sonra ilgili partitiona gider ve yine aynı keye sahip bir mesaj gonderilirse o da aynı partitiona gider
key ve value binary seklinde serialize edilir okunurkende record'da deserilize edilir
*/