using Confluent.Kafka;
using Kafka.Consumer.Events;
using System.Text;

namespace Kafka.Consumer
{
	internal class KafkaService
	{
		internal async Task ConsumeSimpleMessageWithNullKey(string topicName)
		{
			var config = new ConsumerConfig
			{
				BootstrapServers = "localhost:9094",
				GroupId = "use-case-1-group-1",
				AutoOffsetReset = AutoOffsetReset.Earliest
				//ornegin queue'da 10 tane mesaj onceden varsa biz baglandigimizda once o 10 tane mesaji okuyacagimi daha sonrasında gelen mesajları okuyacagini belirtiyorum
				//latest dersek baglandigimiz andan itabaren mesajları okumaya baslar oncekileri 
			};
			using var consumer = new ConsumerBuilder<Null, string>(config).Build();
			consumer.Subscribe(topicName);

			while (true)
			{
				var consumeResult = consumer.Consume(5000); //burasi bloklayici bir satir; mesaj gelene kadar burada kod bloke olur o yüzden timeout verelim
															//5 saniye bekleyip bu consumer.consume() satırından cıkacak
				if (consumeResult != null)
				{
					Console.WriteLine($"gelen mesaj : ({consumeResult.Message.Value})");
				}
				await Task.Delay(500);
			}
		}

		internal async Task ConsumeSimpleMessageWithIntKey(string topicName)
		{
			var config = new ConsumerConfig
			{
				BootstrapServers = "localhost:9094",
				GroupId = "use-case-1-group-1",
				AutoOffsetReset = AutoOffsetReset.Earliest
				//ornegin queue'da 10 tane mesaj onceden varsa biz baglandigimizda once o 10 tane mesaji okuyacagimi daha sonrasında gelen mesajları okuyacagini belirtiyorum
				//latest dersek baglandigimiz andan itabaren mesajları okumaya baslar oncekileri 
			};
			using var consumer = new ConsumerBuilder<int, string>(config).Build();
			consumer.Subscribe(topicName);

			while (true)
			{
				var consumeResult = consumer.Consume(5000); //burasi bloklayici bir satir; mesaj gelene kadar burada kod bloke olur o yüzden timeout verelim
															//5 saniye bekleyip bu consumer.consume() satırından cıkacak
				if (consumeResult != null)
				{
					Console.WriteLine($"gelen mesaj: key = {consumeResult.Message.Key} value = {consumeResult.Message.Value})");
				}
				await Task.Delay(200);
			}
		}
		internal async Task ConsumeComplexMessageWithIntKey(string topicName)
		{
			var config = new ConsumerConfig
			{
				BootstrapServers = "localhost:9094",
				GroupId = "use-case-1-group-1",
				AutoOffsetReset = AutoOffsetReset.Earliest
				//ornegin queue'da 10 tane mesaj onceden varsa biz baglandigimizda once o 10 tane mesaji okuyacagimi daha sonrasında gelen mesajları okuyacagini belirtiyorum
				//latest dersek baglandigimiz andan itabaren mesajları okumaya baslar oncekileri 
			};
			using var consumer = new ConsumerBuilder<int, OrderCreatedEvent>(config)
				.SetValueDeserializer(new CustomValueDeserializer<OrderCreatedEvent>())
				.Build();

			consumer.Subscribe(topicName);

			while (true)
			{
				var consumeResult = consumer.Consume(5000); //burasi bloklayici bir satir; mesaj gelene kadar burada kod bloke olur o yüzden timeout verelim
															//5 saniye bekleyip bu consumer.consume() satırından cıkacak
				if (consumeResult != null)
				{
					var orderCreatedEvent = consumeResult.Message.Value;
					Console.WriteLine($"UserId: {orderCreatedEvent.UserId}\nOrderCode:{orderCreatedEvent.OrderCode}\nTotalPrice{orderCreatedEvent.TotalPrice}");
					Console.WriteLine(new string('-', 50));
				}
				await Task.Delay(20);
			}
		}
		internal async Task ConsumeComplexMessageWithIntKeyAndHeader(string topicName)
		{
			var config = new ConsumerConfig
			{
				BootstrapServers = "localhost:9094",
				GroupId = "use-case-1-group-1",
				AutoOffsetReset = AutoOffsetReset.Earliest
				//ornegin queue'da 10 tane mesaj onceden varsa biz baglandigimizda once o 10 tane mesaji okuyacagimi daha sonrasında gelen mesajları okuyacagini belirtiyorum
				//latest dersek baglandigimiz andan itabaren mesajları okumaya baslar oncekileri 
			};
			using var consumer = new ConsumerBuilder<int, OrderCreatedEvent>(config)
				.SetValueDeserializer(new CustomValueDeserializer<OrderCreatedEvent>())
				.Build();

			consumer.Subscribe(topicName);

			while (true)
			{
				var consumeResult = consumer.Consume(5000); //burasi bloklayici bir satir; mesaj gelene kadar burada kod bloke olur o yüzden timeout verelim
															//5 saniye bekleyip bu consumer.consume() satırından cıkacak
				if (consumeResult != null)
				{
					var correlationId = Encoding.UTF8.GetString(consumeResult.Message.Headers.GetLastBytes("correlation_id"));	//Headers[0].GetValueBytes();
					var version = Encoding.UTF8.GetString(consumeResult.Message.Headers.GetLastBytes("version"));   //Headers[1].GetValueBytes();
					Console.WriteLine($"headers: correlation_id: {correlationId}, version: {version}");

					var orderCreatedEvent = consumeResult.Message.Value;
					Console.WriteLine($"UserId: {orderCreatedEvent.UserId}\nOrderCode:{orderCreatedEvent.OrderCode}\nTotalPrice{orderCreatedEvent.TotalPrice}");
					Console.WriteLine(new string('-', 50));
				}
				await Task.Delay(20);
			}
		}
	}
}
/*
rabbit mq'da push mantigi vardir rabbitmq consumerlara mesaj gonderir
kafkada pull mantigi vardir consumerler kafkadan mesaji alır

bizim topic'imizin 3 partitionu var dolayısıyla max 3 adet consumer mesajlari tüketebilir, ornegin 4 tane consumer ayaga kaldirilirsa bir tanesi idle'da bekler
ona mesajlar gitmez bunun icin debugsız consumer'i 3 ve 4 kere calistiralim sonra producerdan mesajları gonderelim 
 */
