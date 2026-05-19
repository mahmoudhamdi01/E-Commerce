using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Exceptions
{
    public class UserNotFoundException(string Email) : NotFoundException($"User With Email {Email} Is Not Found")
    {
    }
}
