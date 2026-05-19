using ECommerce.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Interface.Interfaces
{
    public interface ILocalizationService
    {
        string GetCurrentLanguage();
        string GetLocalizedTitle(LocalizableEntity entity);
        string GetLocalizedDescription(LocalizableEntity entity);
        List<string> GetLocalizedList(List<string>? ar, List<string>? en);
    }
}
