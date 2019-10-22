using System;

namespace PaymentPlatform.Framework.Models
{
    /// <summary>
    /// Модель для таблицы Serilog.
    /// </summary>
    public class SerilogModel
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Сообщение.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Шаблон сообщения.
        /// </summary>
        public string MessageTemplate { get; set; }

        /// <summary>
        /// Уровень.
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Время.
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Ошибка.
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// Параметры.
        /// </summary>
        public string Properties { get; set; }
    }
}
