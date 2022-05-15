using api.Entities;
using api.Entities.DTOs;
using api.Repository;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        private readonly IPostRepository _repository;
        private readonly IMapper _mapper;
        public PostController(IPostRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _repository.GetAllPost();

            List<PostDTO> PostsDTO = new();

            foreach (var post in posts)
            {
                PostsDTO.Add(_mapper.Map<PostDTO>(post));
            }

            return Ok(PostsDTO);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(Guid Id)
        {
            var post = await _repository.GetPostById(Id);

            if (post is null)
            {
                return NotFound();
            }

            var postDTO = _mapper.Map<PostDTO>(post);

            return Ok(postDTO);
        }

        [HttpPost]
        public async Task<ActionResult<PostDTO>> Post([FromBody] InsertPostDTO insertPostDTO)
        {
            if (insertPostDTO is null)
            {
                return BadRequest();
            }

            var post = _mapper.Map<Post>(insertPostDTO);


            var existCategory = _repository.CategoryExists(post.CategoryId);

            if (!existCategory)
            {
                return BadRequest("Category not found");
            }

            await _repository.CreatePost(post);

            var postDTO = _mapper.Map<PostDTO>(post);

            return CreatedAtAction(nameof(Get), new { id = postDTO.Id }, postDTO);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(Guid Id, [FromBody] PostDTO post)
        {
            if (post is null)
            {
                return BadRequest();
            }

            if (post.Id != Id)
            {
                return BadRequest("The object id does not match the one passed in parameter");
            }

            var existCategory = _repository.CategoryExists(post.CategoryId);

            if (!existCategory)
            {
                return BadRequest("Category not found");
            }

            var res = await _repository.GetPostById(Id);

            if (res is null)
            {
                return NotFound();
            }

            res.Title = post.Title;
            res.Content = post.Content;
            res.Enabled = post.Enabled;
            res.CategoryId = post.CategoryId;
            res.UpdateAt = DateTime.UtcNow;

            var success = await _repository.UpdatePost(res);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var exist = _repository.PostExists(Id);

            if (!exist)
            {
                return NotFound();
            }

            await _repository.DeletePost(Id);

            return NoContent();
        }
    }
}
