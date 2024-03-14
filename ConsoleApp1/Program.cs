//Baglanti olusturma
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://joyrhfww:OrRKGmmKZv2_JULc8tyrHt6K0D3aaSpC@cow.rmq2.cloudamqp.com/joyrhfww");

//Baglanti aktiflestirme ve kanal acma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Queue olusturma
channel.QueueDeclare(queue: "Example-Queue-1", exclusive: false);

//Queue dan mesaj okuma
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "Example-Queue-1", autoAck: false, consumer: consumer);
consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
};

Console.Read();