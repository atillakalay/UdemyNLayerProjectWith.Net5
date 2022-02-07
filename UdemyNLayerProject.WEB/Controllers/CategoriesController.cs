using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UdemyNLayerProject.WEB.DTOs;
using UdemyNLayerProject.WEB.Filters;

namespace UdemyNLayerProject.WEB.Controllers
{
    public class CategoriesController : Controller
    {

        private readonly CategoryApiService.CategoryApiService _categoryApiService;
        private readonly IMapper _mapper;

        public CategoriesController(IMapper mapper, CategoryApiService.CategoryApiService categoryApiService)
        {
            _categoryApiService = categoryApiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryApiService.GetAllAsync();
            return View(_mapper.Map<IEnumerable<CategoryDto>>(categories));
        }


        public IActionResult Save()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Save(CategoryDto categoryDto)
        {
            await _categoryApiService.AddAsync(categoryDto);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryApiService.GetByIdAsync(id);
            return View(_mapper.Map<CategoryDto>(category));
        }
        [HttpPost]
        public async Task<IActionResult> Update(CategoryDto categoryDto)
        {
            await _categoryApiService.UpdateAsync(categoryDto);
            return RedirectToAction("Index");
        }

        [ServiceFilter(typeof(NotFoundFilter))]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryApiService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
