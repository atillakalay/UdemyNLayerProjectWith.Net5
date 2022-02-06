using System.ComponentModel.DataAnnotations;

namespace UdemyNLayerProject.WEB.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
