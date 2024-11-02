using Confluent.Kafka.Admin;
using Confluent.Kafka;
using Kafka.Producer.Events;
using System.Text;
using System.ComponentModel.DataAnnotations;

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
				new TopicSpecification{Name = topicName,NumPartitions = 6, ReplicationFactor = 1}
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

			foreach (var item in Enumerable.Range(1, 100))
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
		//ayni key'e sahip olan mesajlar hep ayni partitiona gider 
		internal async Task SendSimpleMessageWithIntKey(string topicName)
		{
			var config = new ProducerConfig { BootstrapServers = "localhost:9094" };

			using var producer = new ProducerBuilder<int, string>(config).Build();

			foreach (var item in Enumerable.Range(1, 100))
			{
				var message = new Message<int, string>()
				{
					Value = $"Message(use-case-1) - {item}",
					Key = item
				};
				var result = await producer.ProduceAsync(topicName, message);

				foreach (var propertyInfo in result.GetType().GetProperties())
				{
					Console.WriteLine($"{propertyInfo.Name}:{propertyInfo.GetValue(result)}");
				}
				Console.WriteLine(new string('-', 50));
				await Task.Delay(10);
			}
		}
		//complex message derken class gibi record gibi veriler kastediliyor
		internal async Task SendComplexMessageWithIntKey(string topicName)
		{
			var config = new ProducerConfig { BootstrapServers = "localhost:9094" };

			using var producer = new ProducerBuilder<int, OrderCreatedEvent>(config)
				.SetValueSerializer(new CustomValueSerializer<OrderCreatedEvent>())
				.Build();

			foreach (var item in Enumerable.Range(1, 100))
			{
				var orderCreatedEvent = new OrderCreatedEvent { OrderCode = Guid.NewGuid().ToString(), TotalPrice = item * 100, UserId = item };
				var message = new Message<int, OrderCreatedEvent>()
				{
					Value = orderCreatedEvent,
					Key = item,
				};
				var result = await producer.ProduceAsync(topicName, message);

				foreach (var propertyInfo in result.GetType().GetProperties())
				{
					Console.WriteLine($"{propertyInfo.Name}:{propertyInfo.GetValue(result)}");
				}
				Console.WriteLine(new string('-', 50));
				await Task.Delay(10);
			}
		}

		//header'a metadata bilgileri konulur yani asıl dataya konulmayacak gereksiz datalar konulur mesajla ilgili extra bilgiler icerir
		internal async Task SendComplexMessageWithIntKeyAndHeader(string topicName)
		{
			var config = new ProducerConfig { BootstrapServers = "localhost:9094" };

			using var producer = new ProducerBuilder<int, OrderCreatedEvent>(config)
				.SetValueSerializer(new CustomValueSerializer<OrderCreatedEvent>())
				.Build();

			var header = new Headers
			{
				{ "correlation_id", Encoding.UTF8.GetBytes("123")},
				{ "version", Encoding.UTF8.GetBytes("v1")},
			};

			foreach (var item in Enumerable.Range(1, 3))
			{
				var orderCreatedEvent = new OrderCreatedEvent { OrderCode = Guid.NewGuid().ToString(), TotalPrice = item * 100, UserId = item };
				var message = new Message<int, OrderCreatedEvent>()
				{
					Value = orderCreatedEvent,
					Key = item,
					Headers = header
				};
				var result = await producer.ProduceAsync(topicName, message);

				foreach (var propertyInfo in result.GetType().GetProperties())
				{
					Console.WriteLine($"{propertyInfo.Name}:{propertyInfo.GetValue(result)}");
				}
				Console.WriteLine(new string('-', 50));
				await Task.Delay(10);
			}
		}
		internal async Task SendComplexMessageWithComplexKey(string topicName)
		{
			var config = new ProducerConfig { BootstrapServers = "localhost:9094" };

			using var producer = new ProducerBuilder<MessageKey, OrderCreatedEvent>(config)
				.SetValueSerializer(new CustomValueSerializer<OrderCreatedEvent>())
				.SetKeySerializer(new CustomKeySerializer<MessageKey>())
				.Build();

			foreach (var item in Enumerable.Range(1, 3))
			{
				var orderCreatedEvent = new OrderCreatedEvent { OrderCode = Guid.NewGuid().ToString(), TotalPrice = item * 100, UserId = item };
				var message = new Message<MessageKey, OrderCreatedEvent>()
				{
					Value = orderCreatedEvent,
					Key = new MessageKey("key1 value", "key2 value")
				};
				var result = await producer.ProduceAsync(topicName, message);

				foreach (var propertyInfo in result.GetType().GetProperties())
				{
					Console.WriteLine($"{propertyInfo.Name}:{propertyInfo.GetValue(result)}");
				}
				Console.WriteLine(new string('-', 50));
				await Task.Delay(10);
			}
		}
		internal async Task SendMessageWithTimeStamp(string topicName)
		{
			var config = new ProducerConfig { BootstrapServers = "localhost:9094" };

			using var producer = new ProducerBuilder<MessageKey, OrderCreatedEvent>(config)
				.SetValueSerializer(new CustomValueSerializer<OrderCreatedEvent>())
				.SetKeySerializer(new CustomKeySerializer<MessageKey>())
				.Build();

			foreach (var item in Enumerable.Range(1, 3))
			{
				var orderCreatedEvent = new OrderCreatedEvent { OrderCode = Guid.NewGuid().ToString(), TotalPrice = item * 100, UserId = item };
				var message = new Message<MessageKey, OrderCreatedEvent>()
				{
					Value = orderCreatedEvent,
					Key = new MessageKey("key1 value", "key2 value"),
					//Timestamp = new Timestamp(new DateTime(2015, 05, 23)) belirtmezsek default olarak zaten guncel tarihte timestamp eklenir
				};
				var result = await producer.ProduceAsync(topicName, message);

				foreach (var propertyInfo in result.GetType().GetProperties())
				{
					Console.WriteLine($"{propertyInfo.Name}:{propertyInfo.GetValue(result)}");
				}
				Console.WriteLine(new string('-', 50));
				await Task.Delay(10);
			}
		}
		internal async Task SendMessageToSpecificPartition(string topicName)
		{
			var config = new ProducerConfig { BootstrapServers = "localhost:9094" };

			using var producer = new ProducerBuilder<Null, string>(config).Build();

			foreach (var item in Enumerable.Range(1, 10))
			{
				var message = new Message<Null, string>
				{
					Value = $"message: {item}"
				};

				var topicPartition = new TopicPartition(topicName, new Partition(2)); //2.indexteki partitiona mesaj gonderelim

				var result = await producer.ProduceAsync(topicPartition, message);

				foreach (var propertyInfo in result.GetType().GetProperties())
				{
					Console.WriteLine($"{propertyInfo.Name}:{propertyInfo.GetValue(result)}");
				}
				Console.WriteLine(new string('-', 50));
				await Task.Delay(10);
			}
		}
		internal async Task SendMessageWithAcknowledgement(string topicName)
		{
			var config = new ProducerConfig
			{
				BootstrapServers = "localhost:9094",
				Acks = Acks.All //Acks.Leader ,Acks.None,
			};

			using var producer = new ProducerBuilder<Null, string>(config).Build();

			foreach (var item in Enumerable.Range(1, 10))
			{
				var message = new Message<Null, string>
				{
					Value = $"message: {item}"
				};

				var result = await producer.ProduceAsync(topicName, message);

				foreach (var propertyInfo in result.GetType().GetProperties())
				{
					Console.WriteLine($"{propertyInfo.Name}:{propertyInfo.GetValue(result)}");
				}
				Console.WriteLine(new string('-', 50));
				await Task.Delay(10);
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

bir consumer birden fazla partitiondan data okuyabilir; ama 1 partition varsa ve birden fazla consumer varsa sadece 1 consumer datayı okuyabilir
mesaj okundugunda ilgili offset bir yana kayar ve bu her partition icin ayrı ayrı offset degeri tutulur
consumer tarafında partitionlarda offset vardır okuma sırası diyebiliriz tek consumer oldugunu dusunelim bu her bir partition icin bir offset degeri vardır

3 partition var ve 3 consumer var bunlar group A olsun her birine birer partition gider 
ama yeni bir grupla (group B) 3 adet daha consumer olursa kafka her iki gruptaki consumerlara aynı veriyi duplicate eder

Acknowledgement for producer;
3 secenek var 

acks degeri 0 olursa;
partition'a leader'e ve diger brokerlardaki replicalara mesajin ulasmasi garanti degildir gitmeyedebilir ama performanslidir 
ornegin bir yerdeki ısı ve nem degerlerinin (1000 adet veri) ortalamasini gonderecegimiz zaman hatalı mesajlar tolere edilebilir
veriyi aliyor daha diske kaydetmeden bize hemen response'u doner arka planda kaydedebilirse eder.

acks degeri 1 olursa;
partition'a leader'e mesaj guvenli bir sekilde ulasmasinin onayini bekler mesaj basarili bir sekilde alindiginda kabul edilir, 
ama diger brokerlardaki replicalar icin ayni durum söz konusu degildir basarili mesaj garanti degildir defaultu budur

acks degeri All veya -1 olursa;
hem partitiondaki leader icin hem de diger brokerlardaki replicalar icin gonderilen mesajin basarili olmasının onayini bekler full guvenliklidir
buna ornek olarak mesela siparis bilgileri iceren mesajlar olabilir bununla ilgili veri kaybi tolere edilemez
ihtiyaca gore 3'u de kullanilabilir
*/