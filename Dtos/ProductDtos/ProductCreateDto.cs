using System.ComponentModel.DataAnnotations;

namespace OnlineShopping.Dtos.ProductDtos
{
    public class ProductCreateDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string SpecialTag { get; set; }
        [Range(1, int.MaxValue)]
        public double Price { get; set; }
        public string Image { get; set; }
        public string ImageLocalPath { get; set; }
        public IFormFile ImgFile { get; set; }
    }
}
