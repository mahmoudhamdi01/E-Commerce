using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Interface.IServices.Product
{
    public class ProductCreateUpdateDTO
    {
        [Required]
        [MaxLength(100)]
        public string TitleArabic { get; set; } = default!;

        [Required]
        [MaxLength(100)]
        public string TitleEnglish { get; set; } = default!;

        [MaxLength(500)]
        public string? DescriptionAr { get; set; }

        [MaxLength(500)]
        public string? DescriptionEn { get; set; }

        public IFormFile? Picture { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(1, 100)]
        public int BrandId { get; set; }

        [Range(1, 100)]
        public int TypeId { get; set; }
    }
}
