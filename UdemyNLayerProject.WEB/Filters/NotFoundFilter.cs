using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using UdemyNLayerProject.WEB.DTOs;

namespace UdemyNLayerProject.WEB.Filters
{

    public class NotFoundFilter : ActionFilterAttribute
    {
        private readonly CategoryApiService.CategoryApiService _categoryApiService;

        public NotFoundFilter(CategoryApiService.CategoryApiService categoryApiService)
        {
            _categoryApiService = categoryApiService;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            int id = (int)context.ActionArguments.Values.FirstOrDefault();

            var product = await _categoryApiService.GetByIdAsync(id);

            if (product != null)
            {
                await next();
            }
            else
            {
                ErrorDto errorDto = new ErrorDto();
                errorDto.Status = 404;
                errorDto.Errors.Add($"id'si {id} olan kategori veritabanında bulunamadı.");

                context.Result = new RedirectToActionResult("Error", "Home", errorDto);
            }
        }
    }
}
