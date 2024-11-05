using Shared.Events;
using System.Runtime.CompilerServices;

namespace Order.API.Services
{
	public static class BusExt
	{
		public static async Task CreateTopicsOrQueues(this WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var ibus = scope.ServiceProvider.GetRequiredService<IBus>();
			await ibus.CreateTopicOrQueue([BusConstants.OrderCreatedEventTopicName]);
		}
	}
}
