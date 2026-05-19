using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Interface.IServices.DeliveryMethod
{
    public class DeliveryMethodCreateDTO
    {
        [Required]
        [MaxLength(50)]
        public string ShortName { get; set; } = default!;

        [Required]
        [MaxLength(100)]
        public string Description { get; set; } = default!;

        [Required]
        [MaxLength(50)]
        public string DeliveryTime { get; set; } = default!;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
