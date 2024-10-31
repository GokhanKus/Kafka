using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}
/*
rabbit mq'da push mantigi vardir rabbitmq consumerlara mesaj gonderir
kafkada pull mantigi vardir consumerler kafkadan mesaji alır

bizim topic'imizin 3 partitionu var dolayısıyla max 3 adet consumer mesajlari tüketebilir, ornegin 4 tane consumer ayaga kaldirilirsa bir tanesi idle'da bekler
ona mesajlar gitmez bunun icin debugsız consumer'i 3 ve 4 kere calistiralim sonra producerdan mesajları gonderelim 
 */
