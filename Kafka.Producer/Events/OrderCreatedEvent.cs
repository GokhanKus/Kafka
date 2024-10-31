namespace Kafka.Producer.Events
{
	//classlari '==' ile karsilastirirsak referans tipleriyle karsilastirir
	//recordlarda ise icersindeki propertlerin degerleri uzerinden bir karsilastirma yapar
	//microserviceler arasında data tasirken mumkun oldugunca recordlar uzerinden data tasıyacagiz
	//bu recordlari onceki mvc ve api projelerinde dto sınıflarında da kullanmistim.
	internal record OrderCreatedEvent
	{
		public string OrderCode { get; init; } = default!;
		public decimal TotalPrice { get; init; }
        public int UserId { get; init; }
    }
}
/*
bu objeyi mesaj olarak gondermek icin once json'a serilize daha sonrada binary formata donusturecegiz, cunku kafka'ya complex type'da mesajlar bu sekilde gonderilir
*/
