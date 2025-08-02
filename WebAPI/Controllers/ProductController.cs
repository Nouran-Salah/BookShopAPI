using AutoMapper;
using BLL.UnitOfWork;
using DAl.models;
using DAL.DTO.ProductDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{

    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetAllProduct V1")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetAllProductV1()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            var productsDto = _mapper.Map<List<ProductReadDto>>(products);
            return Ok(productsDto);
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        [Route("GetAllProduct V2")]
        public async Task<ActionResult> GetAllProductV2()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            var productsDto = _mapper.Map<List<ProductReadDto>>(products);
            foreach (var p in productsDto)
            {
                p.Title = $"[V2] {p.Title}";
            }
            return Ok(productsDto);
        }


        [HttpGet]
        [Route("GetProductById/{Id:int}")]
        [ResponseCache(CacheProfileName ="DefaultCache")]
        public async Task<ActionResult> GetProductById(int Id)
        {
            if (Id == 0)
            {
                return BadRequest("Id cannot be zero");
            }
            var pro = await _unitOfWork.Products.GetByIdAsync(Id);
            var productDto = _mapper.Map<ProductReadDto>(pro);
            return Ok(productDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct(ProductCreateDto product)
        {
            var Cat = await _unitOfWork.Categories.GetByNameAsync(product.CategoryName);
            var productCreated = _mapper.Map<Product>(product);
            productCreated.CategoryId = Cat.Id;
            await _unitOfWork.Products.CreateAsync(productCreated);
            return NoContent();
        }
        [HttpPut]
        public async Task<IActionResult> EditProduct(int Id, ProductUpdateDto product)
        {
            var existingProduct = await _unitOfWork.Products.GetByIdAsync(Id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            _mapper.Map(product, existingProduct);
            var Cat = await _unitOfWork.Categories.GetByNameAsync(product.CategoryName);
            existingProduct.CategoryId = Cat.Id;
            await _unitOfWork.Products.UpdateAsync(existingProduct);

            return NoContent();
        }
        [HttpPatch]
        public async Task<IActionResult> PatchProduct(int Id, [FromBody] JsonPatchDocument<ProductUpdateDto> patchDoc)
        {
            var existingProduct = await _unitOfWork.Products.GetByIdAsync(Id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            var productToPatch = _mapper.Map<ProductUpdateDto>(existingProduct);

            patchDoc.ApplyTo(productToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!string.IsNullOrEmpty(productToPatch.CategoryName))
            {
                var category = await _unitOfWork.Categories.GetByNameAsync(productToPatch.CategoryName);
                if (category == null)
                    return BadRequest("Invalid Category Name");

                existingProduct.CategoryId = category.Id;
            }

            _mapper.Map(productToPatch, existingProduct);

            await _unitOfWork.SaveAsync();

            return NoContent();
        }



        [HttpDelete]
        [Route("DeleteProduct/{Id:int}")]
        public async Task<IActionResult> DeleteProduct(int Id)
        {
            if (Id == 0)
            {
                return BadRequest("Id cannot be zero");
            }
            var product = await _unitOfWork.Products.GetByIdAsync(Id);
            if (product == null)
            {
                return NotFound();
            }
            await _unitOfWork.Products.DeleteAsync(Id);
            return NoContent();
        }
    }
}
