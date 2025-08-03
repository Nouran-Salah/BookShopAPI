using AutoMapper;
using BLL.UnitOfWork;
using DAl.models;
using DAL.DTO.CategoryDto;
using DAL.DTO.ProductDto;
using DAL.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private  IUnitOfWork _unitOfWork;
        private  IMapper _mapper;
       public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetAllCategories")]
        public async Task<ActionResult> GetAllCategories(int page=1 , int pageSize = 5)
        {
            var allCats = await _unitOfWork.Categories.GetAllAsync();
            if (allCats == null || !allCats.Any())
            {
                return NotFound("No categories found");
            }
            var ordered = allCats.OrderBy(c => c.catOrder).ThenByDescending(c => c.catName).ToList();
            var CategoryReadDto = _mapper.Map<List<CategoryReadDto>>(ordered);
            var paged = CategoryReadDto
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .ToList();

            return Ok(paged);
           
        }

        [HttpGet]
        [Route("GetCateogryById/{Id:int}")]
        public async Task<ActionResult> GetCategoryById(int Id)
        {
            if (Id == 0)
            {
                return BadRequest("Id cannot be zero");
            }
            var cat = await _unitOfWork.Categories.GetByIdAsync(Id);
            if (cat == null) {
                return NotFound("Category not found");
            }
            var CatDto = _mapper.Map<CategoryReadDto>(cat);
            return Ok(CatDto);
        }

        [HttpPost]
        public async Task<ActionResult> createCategory(CategoryCreateDto cat)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (cat == null)
                return BadRequest("Category cannot be null");

            var category = _mapper.Map<Category>(cat);
            await _unitOfWork.Categories.CreateAsync(category);
            return CreatedAtAction(nameof(createCategory), new { id = category.Id }, category);
        }



        [HttpPut]
        [Route("EditCategory/{Id:int}")]
        public async Task<IActionResult> EditCategory(CategoryReadDto cat)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(cat.Id);
            if (category == null)
            {
                return NotFound("Category not found");
            }
            _mapper.Map(cat, category);

            await _unitOfWork.Categories.UpdateAsync(category);

            return Ok();
        }

        [HttpDelete]
        [Route("DeleteCategory/{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {

            var foundcategory = await _unitOfWork.Categories.GetByIdAsync(id);
            if (foundcategory == null)
            {
               return NotFound("Category not found");
            }
            await _unitOfWork.Categories.SoftDeleteAsync(id);

            return NoContent();

        }

        [HttpPatch]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCategory(int id, [FromBody] JsonPatchDocument<CategoryReadDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var categoryEntity = await _unitOfWork.Categories.GetByIdAsync(id);
            if (categoryEntity == null)
                return NotFound();

            var categoryToPatch = _mapper.Map<CategoryReadDto>(categoryEntity);

      
            patchDoc.ApplyTo(categoryToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            _mapper.Map(categoryToPatch, categoryEntity);

        
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

    }
}
