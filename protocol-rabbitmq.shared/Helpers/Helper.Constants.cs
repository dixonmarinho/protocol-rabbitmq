namespace protocol.rabbitmq.shared.Helpers
{
    public static partial class Helper
    {
        public static class Constants
        {
            public static string TITLE_APP = "rabbitmq Protocol";

            public static string RABBITMQ_ROUTINGKEY = "PROTOCOL.KEY";
            public static string RABBITMQ_QUEUENAME = "PROTOCOL.QUEUE";
            public static string RABBITMQ_EXCHANGENAME = "EXCHANGE.QUEUE";

            public static string RabbitMQHost { get; set; }
            public static int RabbitMQPort { get; set; }



        }
    }
}
