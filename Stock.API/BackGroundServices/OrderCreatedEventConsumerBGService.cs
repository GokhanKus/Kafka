
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Shared.Events;
using Shared.Events.Events;
using Stock.API.Services;

namespace Stock.API.BackGroundServices
{
	public class OrderCreatedEventConsumerBGService(IBus bus, ILogger<OrderCreatedEventConsumerBGService> logger) : BackgroundService
	{
		private IConsumer<string, OrderCreatedEvent>? _consumer;
		public override Task StartAsync(CancellationToken cancellationToken)
		{
			_consumer = new ConsumerBuilder<string, OrderCreatedEvent>(bus.GetConsumerConfig(BusConstants.OrderCreatedEventGroupId))
		   .SetValueDeserializer(new CustomValueDeserializer<OrderCreatedEvent>())
		   .Build();
			_consumer.Subscribe(BusConstants.OrderCreatedEventTopicName);
			return base.StartAsync(cancellationToken);
		}
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested) //app kapanirken buraya istek gelecek app'i durdurmakla ilgili o zaman whiledan cikalim
			{
				var consumeResult = _consumer!.Consume(5000);
				if (consumeResult != null)
				{
					try
					{
						var orderCreatedEvent = consumeResult.Message.Value;

						logger.LogInformation(
						   $"user id :{orderCreatedEvent.UserId}, order code:{orderCreatedEvent.OrderCode}, total price : {orderCreatedEvent.TotalPrice} ");

						_consumer.Commit(consumeResult);
					}
					catch (Exception ex)
					{
						logger.LogError(ex.Message);
					}
				}
				await Task.Delay(20, stoppingToken);
			}
		}
	}
}
