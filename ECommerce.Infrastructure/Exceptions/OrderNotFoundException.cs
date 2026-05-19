using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Exceptions
{
    public class OrderNotFoundException(Guid id) : NotFoundException($"Order With Id {id} is Not Found")
    {
    }
}
