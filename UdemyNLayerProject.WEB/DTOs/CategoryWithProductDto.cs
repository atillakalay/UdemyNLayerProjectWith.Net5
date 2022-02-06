using System.Collections.Generic;

namespace UdemyNLayerProject.WEB.DTOs
{
    public class CategoryWithProductDto : CategoryDto
    {
        public ICollection<ProductDto> Products { get; set; }
    }
}
