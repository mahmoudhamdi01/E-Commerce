using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Entities
{
    public class LocalizableEntity : BaseEntity<int>
    {
        public string TitleArabic { get; set; } = default!;
        public string TitleEnglish { get; set; } = default!;
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
    }
}
