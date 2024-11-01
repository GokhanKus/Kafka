using Confluent.Kafka;
using System.Text;
using System.Text.Json;

namespace Kafka.Consumer
{
	internal class CustomKeyDeserializer<T> : IDeserializer<T> where T : class
	{
		public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
		{
			var dataToDeserialize = JsonSerializer.Deserialize<T>(data)!;
			return dataToDeserialize;
		}
	}
}
