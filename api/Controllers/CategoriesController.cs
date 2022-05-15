using api.Entities;
using api.Entities.DTOs;
using api.Repository;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        public CategoriesController(IMapper mapper, ICategoryRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _repository.GetAllCategory();

            List<CategoryDTO> categoriesDTO = new();

            foreach (var category in categories)
            {
                categoriesDTO.Add(_mapper.Map<CategoryDTO>(category));
            }

            return Ok(categoriesDTO);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(Guid Id)
        {
            var category = await _repository.GetCategoryById(Id);

            if (category is null)
            {
                return NotFound();
            }

            var categoryDTO = _mapper.Map<CategoryDTO>(category);

            return Ok(categoryDTO);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> Post([FromBody] InsertCategoryDTO insertCategoryDTO)
        {
            if (insertCategoryDTO is null)
            {
                return BadRequest();
            }

            var category = _mapper.Map<Category>(insertCategoryDTO);

            await _repository.CreateCategory(category);

            var categoryDTO = _mapper.Map<CategoryDTO>(category);

            return CreatedAtAction(nameof(Get), new { id = categoryDTO.Id }, categoryDTO);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(Guid Id, [FromBody] CategoryDTO category)
        {
            if (category is null)
            {
                return BadRequest();
            }

            if (category.Id != Id)
            {
                return BadRequest("The object id does not match the one passed in parameter");
            }

            var res = await _repository.GetCategoryById(Id);

            if (res is null)
            {
                return NotFound();
            }

            res.Description = category.Description;
            res.Enabled = category.Enabled;
            res.UpdateAt = DateTime.UtcNow;

            var success = await _repository.UpdateCategory(res);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var exist = _repository.CategoryExists(Id);

            if (!exist)
            {
                return NotFound();
            }

            await _repository.DeleteCategory(Id);

            return Ok(Id);
        }
    }
}
