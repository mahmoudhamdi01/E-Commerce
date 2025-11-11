using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Exceptions
{
    public sealed class BasketNotFoundException(string Key) : NotFoundException($"Basket With Id {Key} is Not Found")
    {
    }
}
