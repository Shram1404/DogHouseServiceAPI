using System.ComponentModel.DataAnnotations;

namespace DogHouseServiceAPI.Models
{
    public class Dog
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Dog must have Name")]
        [MaxLength(100, ErrorMessage = "Name is to long (Max 100)")]
        public string Name { get; set; } = null!;

        [MaxLength(100, ErrorMessage = "Color is to long (Max 100)")]
        [Required(ErrorMessage = "Dog must have Color")]
        public string Color { get; set; } = null!;

        [Required(ErrorMessage = "Dog must have Tail Length")]
        [Range(0.01, 500, ErrorMessage = "Wrong Tail Length (0-500)")]
        public float TailLength { get; set; } = 0;

        [Range(0.01, 500, ErrorMessage = "Wrong Weight (0-500)")]
        [Required(ErrorMessage = "Dog must have Weight")]
        public float Weight { get; set; } = 0;
    }
}
