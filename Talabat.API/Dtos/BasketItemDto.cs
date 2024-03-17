using System.ComponentModel.DataAnnotations;

namespace Talabat.API.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]

        public string ProductName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        [Range(0.1,double.MaxValue,ErrorMessage ="Price Must Be Bigger Than 0.1")]
        public decimal Price { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Quantity Must Be Bigger Than 1")]

        public int Quantity { get; set; }

    }
}