# Kafka
Bu proje, **Apache Kafka** ile producer, consumer işlemlerini ve API tabanlı bir uygulamayı içeren bir örnek uygulamadır. Kafka'nın özellikleri, farklı senaryolarla ve ekran görüntüleriyle açıklanmıştır.

---

## Apache Kafka Nedir?

**Apache Kafka**, yüksek hacimli gerçek zamanlı ve anlık veri akışlarını işlemek için tasarlanmış, dağıtık bir mesajlaşma platformudur. Kafka, veriyi "log" benzeri bir yapıda saklar ve hem dağıtık hem de ölçeklenebilir bir yapıya sahiptir.

### Neden Kafka Kullanılır?

- **Gerçek zamanlı veri işleme:** Veri üreticilerinden (producers) alınan bilgileri hızlıca tüketicilere (consumers) iletir.
- **Dağıtık mimari:** Büyük ölçekli sistemlerde kolayca ölçeklenebilir.
- **Yüksek performans:** Düşük gecikme süreleri ile yüksek veri hacimlerini işleyebilir.
- **Hata toleransı:** Sistem kesintilerinde bile veri kaybını minimuma indirir.

### Gerçek Dünya Kullanım Alanları

1. **Log ve Olay Akışı Yönetimi:** Sistem ve uygulama loglarının toplanıp analiz edilmesi (ör. ELK stack ile).
2. **Gerçek Zamanlı Analitik:** Kullanıcı davranışlarının analiz edilmesi (ör. E-ticaret siteleri için öneri sistemleri).
3. **Mikroservisler Arası İletişim:** Mikroservislerin veri alışverişinde bir köprü görevi görmesi.
4. **IoT ve Sensör Verileri İşleme:** Cihazlardan gelen verilerin gerçek zamanlı işlenmesi.
5. **Finansal İşlemler:** Bankacılık ve borsa gibi yüksek hassasiyet gerektiren sistemlerde veri akışını yönetmek.

---

## Proje Özeti

Bu proje, Kafka'nın aşağıdaki işlevlerini ele alan bir örnek uygulamayı içerir:
- **Producer:** Farklı veri türlerini Kafka'ya gönderir.
- **Consumer:** Kafka'dan mesajları tüketir.
- **API uygulamaları:** Kafka ile veri alışverişini bir API üzerinden gerçekleştirir.

---

## Özellikler ve Yetenekler

### API Uygulaması

- **Stock API (Consumer):** Kafka'dan gelen mesajları tüketir.
- **Order API (Producer):** Kafka'ya mesaj gönderir ve gerektiğinde bir topic/queue oluşturur.

### Console Uygulamaları

1. **Producer Özellikleri:**
   - Farklı key türleriyle mesaj gönderimi (null, int, complex).
   - Zaman damgası (timestamp) ekleme.
   - Belirli bir partition'a mesaj gönderme.
   - Acknowledgement (onay) mekanizması ile mesaj gönderme.
   - Cluster yapılarını destekleme (docker-compose).

2. **Consumer Özellikleri:**
   - Mesajları farklı consumer gruplarında tüketme.
   - Belirli bir partition veya offset'ten itibaren mesaj tüketimi.
   - Mesaj başlıkları (header) ile tüketim.
   - Custom serializer ve deserializer kullanımı.

---

## Kullanım

### API ile Mesaj Gönderimi ve Tüketimi

1. **Mesaj Gönderme:**
   - `Order API` üzerinden bir sipariş oluşturulup Kafka'ya gönderilir.

2. **Mesaj Tüketimi:**
   - `Stock API`, Kafka'dan gelen sipariş mesajlarını tüketir.

**Örnek:**  
`Order API` -> `Kafka Topic` -> `Stock API`

---

## Ekran Görüntüleri

### String Mesaj Gönderimi (`null key`)
Produce

![Send string message with null key](https://github.com/user-attachments/assets/47dd85ae-0758-49e2-874d-311ac47ee503)  
KAFKA UI:
![Gönderilen Mesajlar](https://github.com/user-attachments/assets/aab375f2-1464-4716-835f-0af7bb79380f)  
![Mesaj Görüntüleme](https://github.com/user-attachments/assets/2384d6a4-f123-4dfb-9dba-7fcdbe471241)  

Consume:  
![Consume them](https://github.com/user-attachments/assets/12d92743-b717-401b-92d1-a072438efad3)

---

### Integer Key ile Mesaj Gönderimi
Produce:  
![Send string message with int key](https://github.com/user-attachments/assets/67e832c8-6134-45aa-8f9b-b8281fb460a4)

KAFKA UI:
![Consume them](https://github.com/user-attachments/assets/ce1dd1c3-3746-4630-9127-cc5d204d80da)  


Consume:  
![Ekstra Görüntü](https://github.com/user-attachments/assets/748cda46-dfc2-4951-a753-f45c4b70cd73)

---

### Complex Mesaj Gönderimi
Produce:  
![Send complex message with int key](https://github.com/user-attachments/assets/2a7e84d2-25d0-4dae-ac06-c3a0820e46f4)  

KAFKA UI:
![Ekstra Görüntü](https://github.com/user-attachments/assets/d43b3429-9ad5-40be-9ee5-b3d1fbe769ce)

Consume:  
![Consume them](https://github.com/user-attachments/assets/40e0259e-a05e-466b-8783-2da414514e30)

---

### Belirli Partition'a Mesaj Gönderimi
Produce:  
![SendMessageToSpecificPartition](https://github.com/user-attachments/assets/35838680-19b4-44d0-8615-c5a23d3c225d)  

KAFKA UI:
![Partition Seçimi](https://github.com/user-attachments/assets/96511138-98ff-411f-9c74-e271e4e4abba)

Consume:  
![Consume them](https://github.com/user-attachments/assets/9f1b7839-286b-40ce-a3dc-e25caa6dc0c8)

---

### Acknowledgement ile Mesaj Gönderimi
Produce:  
![SendMessageWithAcknowledgement](https://github.com/user-attachments/assets/a57c625f-11f8-4458-8a02-e90489dce97e)

---

### Cluster ve Topic Oluşturma
Produce:  
![CreateTopicWithClusterAsync](https://github.com/user-attachments/assets/65ae5f51-feeb-4e73-824e-3939fb784427)  
KAFKA UI:
![Cluster Ayarları](https://github.com/user-attachments/assets/4b9e92cd-af9c-4a26-a318-1b16f0708ba3)  
![image](https://github.com/user-attachments/assets/37e09a85-dcc8-4355-a007-6c5030791814)
DOCKER UI:
![image](https://github.com/user-attachments/assets/ff071ee3-753e-4d36-8a98-1386b46b9fcc)


---

### API'den Veri Gönderimi
Produce:  
![send model from api](https://github.com/user-attachments/assets/dba75d51-7268-4c22-8c8f-a899824eb122)  
![Veri Detayları](https://github.com/user-attachments/assets/53e25cc3-2dfe-4738-9a7b-8def3795e195)

Consume:  
![consume from api](https://github.com/user-attachments/assets/3bf658d4-3ed1-42d0-9ebb-e046e922f7d8)

