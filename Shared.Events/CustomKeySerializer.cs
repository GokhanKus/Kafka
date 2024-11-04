using Confluent.Kafka;
using System.Text;
using System.Text.Json;

namespace Shared.Events
{
	public class CustomKeySerializer<T> : ISerializer<T>
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
