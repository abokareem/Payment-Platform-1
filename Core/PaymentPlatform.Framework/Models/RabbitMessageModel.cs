namespace PaymentPlatform.Framework.Models
{
    /// <summary>
    /// Модель сообщения RabbitMq
    /// </summary>
    public class RabbitMessageModel
    {
        /// <summary>
        /// Действие.
        /// </summary>
        public int Action { get; set; }

        /// <summary>
        /// Отправитель.
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Объект модели.
        /// </summary>
        public object Model { get; set; }
    }
}
