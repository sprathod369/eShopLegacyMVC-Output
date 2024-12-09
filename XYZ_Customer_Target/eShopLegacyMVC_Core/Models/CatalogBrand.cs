using System.ComponentModel.DataAnnotations; 
namespace eShopLegacyMVC_Core.Models 
{ 
    public class CatalogBrand 
    { 
        public int Id { get; set; } 
        [Required] 
        [StringLength(100)] 
        public string Brand { get; set; } 
    } 
} 