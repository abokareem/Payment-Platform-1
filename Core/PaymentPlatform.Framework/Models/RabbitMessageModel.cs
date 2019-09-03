using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentPlatform.Framework.Models
{
    public class RabbitMessageModel
    {
        //TODO: Заменить перечислением
        public string Action { get; set; }
        public string Sender { get; set; }
        public object Model { get; set; }
    }
}
