using ECommerce.Interface.IServices.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Interface.IServices.Order
{
    public class OrderDTO
    {
        public int DeliveryMethodId { get; set; }
        public string BasketId { get; set; } = default!;
        public AddressDTO Address { get; set; } = default!;
    }
}
