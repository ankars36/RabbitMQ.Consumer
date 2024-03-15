using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://joyrhfww:OrRKGmmKZv2_JULc8tyrHt6K0D3aaSpC@cow.rmq2.cloudamqp.com/joyrhfww");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

#region P2P (Point-to-Point)
//string queueName = "Example-P2P-Queue";
//channel.QueueDeclare(queue: queueName, exclusive: false, durable: false, autoDelete: false);

//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
//consumer.Received += (sender, e) =>
//{
//    string message = Encoding.UTF8.GetString(e.Body.Span);
//    Console.WriteLine(message);
//};
#endregion

#region Pub/Sub (Publish/Subscribe)
//string exchangeName = "example-pub-sub-exchange";
//channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

//string queueName = channel.QueueDeclare().QueueName;
//channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: string.Empty);

//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
//consumer.Received += (sender, e) =>
//{
//    string message = Encoding.UTF8.GetString(e.Body.Span);
//    Console.WriteLine(message);
//};
#endregion

#region Work Queue
//string queueName = "example-work-queue";
//channel.QueueDeclare(
//    queue: queueName, 
//    exclusive: false, 
//    durable: false, 
//    autoDelete: false);

//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(
//    queue: queueName, 
//    autoAck: true, 
//    consumer: consumer);

//channel.BasicQos(
//    prefetchCount: 1, 
//    prefetchSize: 0, 
//    global: false);

//consumer.Received += (sender, e) =>
//{
//    string message = Encoding.UTF8.GetString(e.Body.Span);
//    Console.WriteLine(message);
//};
#endregion

#region Request/Response
string queueName = "example-request-response-queue";
channel.QueueDeclare(
    queue: queueName,
    exclusive: false,
    durable: false,
    autoDelete: false);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
    //.....
    byte[] responseMessage = Encoding.UTF8.GetBytes($"Process Completed {message}");

    IBasicProperties properties = channel.CreateBasicProperties();
    properties.CorrelationId = e.BasicProperties.CorrelationId;

    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: e.BasicProperties.ReplyTo,
        body: responseMessage,
        basicProperties: properties);
};
#endregion

Console.Read();