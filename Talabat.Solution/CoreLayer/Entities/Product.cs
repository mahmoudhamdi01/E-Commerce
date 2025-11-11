using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Entities
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; }
        [ForeignKey("ProductBrand")]
        public int BrandId { get; set; }
        public ProductBrand ProductBrand { get; set; }
        [ForeignKey("ProductType")]
        public int TypeId { get; set; }
        public ProductType ProductType { get; set; }
    }
}
