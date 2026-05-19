using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Exceptions
{
    public class UnAuthorizedException(string Message = "Invalid Email Or Password") : Exception(Message)
    {
    }
}
