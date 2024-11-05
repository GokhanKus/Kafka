using Confluent.Kafka;
using System.Text;
using System.Text.Json;

namespace Shared.Events
{
	public class CustomValueDeserializer<T> : IDeserializer<T> where T : class
	{
		public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
		{
			var dataToDeserialize = JsonSerializer.Deserialize<T>(data)!;
			return dataToDeserialize;
		}
	}
}
