using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Exceptions
{
    public sealed class AddressNotFoundException(string UserName) : NotFoundException($"User {UserName} Has No Address")
    {
    }
}
