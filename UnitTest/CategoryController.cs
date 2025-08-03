using BLL.UnitOfWork;
using Moq;

using AutoMapper;
using BLL.UnitOfWork;
using BLL.Repository;
using DAL.DTO.ProductDto;
using DAL.DTO.CategoryDto;
using DAL.models;
using WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace UnitTest
{
    public class CategoryControllerTest
    {
        private Mock<IUnitOfWork> _UnitOfWork;
        private Mock<IMapper> _mapper;
        private Mock<ICategoryRepository> _categoryRepositoryMock;

        public CategoryControllerTest()
        {
            _UnitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();

            _UnitOfWork.Setup(u => u.Categories).Returns(_categoryRepositoryMock.Object);
        }

        [Fact]
        public async Task AddCategory_ReturnsCreated_WhenValid()
        {
            var CategoryDto = new CategoryCreateDto
            {
                catName = "Test Category",
                catOrder = 12,

            };
            var mappedCategory = new Category
            {
                Id = 1,
                catName = CategoryDto.catName,
                catOrder = CategoryDto.catOrder,
                markedAsDeleted = false,
                createdDate = DateTime.Now

            };
            _mapper.Setup(m => m.Map<Category>(CategoryDto)).Returns(mappedCategory);
            _categoryRepositoryMock.Setup(repo => repo.CreateAsync(mappedCategory)).Returns(Task.CompletedTask);
            _UnitOfWork.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);
            var controller = new CategoryController(_UnitOfWork.Object, _mapper.Object);

            var result= await controller.createCategory(CategoryDto);
           var createdAt= Assert.IsType<CreatedAtActionResult>(result);
            var Newproduct =Assert.IsType<Category>(createdAt.Value);
            Assert.Equal(CategoryDto.catName, Newproduct.catName);

        }

        [Fact]
        public async Task AddCategory_ReturnsBadRequest_WhenModelStateInvalid()
        {
            var CategoryDto = new CategoryCreateDto
            {
                catName = "Test Category",
                catOrder = 12,
            };
            var controller = new CategoryController(_UnitOfWork.Object, _mapper.Object);
            controller.ModelState.AddModelError("catName", "Required");
            var result = await controller.createCategory(CategoryDto);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsOk_WhenCategoriesExist()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, catName = "Category1", catOrder = 1, createdDate= DateTime.Now , markedAsDeleted=false },
                new Category { Id = 2, catName = "Category2", catOrder = 2 ,  createdDate= DateTime.Now , markedAsDeleted=false},
            };
            var CategoriesDto= new List<CategoryReadDto>
            {
                new CategoryReadDto { Id = 1, catName = "Category1", catOrder = 1, createdDate= DateTime.Now , markedAsDeleted=false },
                new CategoryReadDto { Id = 2, catName = "Category2", catOrder = 2 ,  createdDate= DateTime.Now , markedAsDeleted=false},
            };

            _mapper.Setup(m => m.Map<List<CategoryReadDto>>(categories)).Returns(CategoriesDto);
            _categoryRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);
            var controller = new CategoryController(_UnitOfWork.Object, _mapper.Object);
            var result = await controller.GetAllCategories();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategories = Assert.IsType<List<CategoryReadDto>>(okResult.Value);
            Assert.Equal(categories.Count, returnedCategories.Count);


        }

        [Fact]
        public async Task GetAllCategories_ReturnsEmptyList_WhenNoCategoriesExist()
        {
            _categoryRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Category>());

            var controller = new CategoryController(_UnitOfWork.Object, _mapper.Object);

            var result = await controller.GetAllCategories();

            var okResult = Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsOk_WhenCategoryExists()
        {
            var category = new Category { Id = 1, catName = "Category1", catOrder = 1, createdDate = DateTime.Now, markedAsDeleted = false };
            var categoryDto = new CategoryReadDto { Id = 1, catName = "Category1", catOrder = 1, createdDate = DateTime.Now, markedAsDeleted = false };
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(category);
            _mapper.Setup(m => m.Map<CategoryReadDto>(category)).Returns(categoryDto);
            var controller = new CategoryController(_UnitOfWork.Object, _mapper.Object);
            var result = await controller.GetCategoryById(1);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategory = Assert.IsType<CategoryReadDto>(okResult.Value);
            Assert.Equal(category.Id, returnedCategory.Id);
        }
        [Fact]
        public async Task GetCategoryById_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Category)null);
            var controller = new CategoryController(_UnitOfWork.Object, _mapper.Object);
            var result = await controller.GetCategoryById(1);
            Assert.IsType<NotFoundObjectResult>(result);
        }
        [Fact]
        public async Task EditCategory_ReturnsOk_WhenCategoryUpdatedAsync()
        {
            var updatedCategoryDto = new CategoryReadDto { Id = 1, catName = "Updated Category", catOrder = 2, createdDate = DateTime.Now, markedAsDeleted = false };
            var category = new Category { Id = 1, catName = "Category1", catOrder = 1, createdDate = DateTime.Now, markedAsDeleted = false };
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(category);
            _mapper.Setup(m => m.Map<Category>(updatedCategoryDto)).Returns(category);
            _categoryRepositoryMock.Setup(repo => repo.UpdateAsync(category)).Returns(Task.CompletedTask);
            _UnitOfWork.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);
            var controller = new CategoryController(_UnitOfWork.Object, _mapper.Object);
            var result =await controller.EditCategory(updatedCategoryDto);
            Assert.IsType<OkResult>(result);
          }

        [Fact]
        public async Task EditCategory_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            var updatedCategoryDto = new CategoryReadDto { Id = 1, catName = "Updated Category", catOrder = 2, createdDate = DateTime.Now, markedAsDeleted = false };
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Category)null);
            var controller = new CategoryController(_UnitOfWork.Object, _mapper.Object);
            var result = await controller.EditCategory(updatedCategoryDto);
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}