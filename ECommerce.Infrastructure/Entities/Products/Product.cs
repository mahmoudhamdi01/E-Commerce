using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Entities.Products
{
    public class Product : LocalizableEntity
    {
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public ProductBrand ProductBrand { get; set; }
        [ForeignKey("ProductBrand")]
        public int BrandId { get; set; }
        public ProductType ProductType { get; set; }
        [ForeignKey("ProductType")]
        public int TypeId { get; set; }
    }
}
