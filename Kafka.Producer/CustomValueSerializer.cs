using Confluent.Kafka;
using System.Text;
using System.Text.Json;

namespace Kafka.Producer
{
	//bugun OrderCreatedEvent baska bir gun UserCreatedEvent icin serilize islemi yapmak isteyebilirim o yuzden bu class dinamik olarak generic bir class olsun
	internal class CustomValueSerializer<T> : ISerializer<T> where T : class
	{
		public byte[] Serialize(T data, SerializationContext context)
		{
			var dataToSerialize = JsonSerializer.Serialize(data);
			var byteData = Encoding.UTF8.GetBytes(dataToSerialize);
			return byteData;
			//return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
		}
	}
}
