using Microsoft.AspNetCore.Mvc;
using LeitourApi.Models;
using LeitourApi.Repository;
using LeitourApi.Interfaces;
using System.Threading.Tasks;
using LeitourApi.Data;

namespace LeitourApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly Message _message;
        private readonly IUnitOfWork uow;
        public PostsController(IUnitOfWork unitOfWork)
        {
            uow = unitOfWork;
            _message = new Message("Post", "o");
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetPosts()
        {
            var posts = await uow.PostRepository.GetAll();
            return posts != null ? posts : _message.MsgNotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await uow.PostRepository.GetById(id);
            return post != null ? post : _message.MsgNotFound();
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<List<Post>>> GetPostsByEmail(string email)
        {
            var user = await uow.UserRepository.GetByCondition(u => u.Email == email);
            if(user == null)
                return new Message("Usuario","o").MsgNotFound();
            var posts = await uow.PostRepository.GetAllByCondition(p => p.UserId == user.Id);
            return posts != null ? posts : _message.MsgNotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Post>> PostPost([FromHeader] string token, Post post)
        {
            
            if (TokenService.DecodeToken(token) != post.UserId)
                return _message.MsgInvalid();
            uow.PostRepository.Add(post);
            return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost([FromHeader] string token, int id, [FromBody] Post updatePost)
        {
            var post = await uow.PostRepository.GetById(id);
            if (post == null) 
                return _message.MsgNotFound();
            if (id != updatePost.Id || TokenService.DecodeToken(token) != updatePost.UserId)
                return _message.MsgInvalid();
            uow.PostRepository.Update(updatePost);
            return _message.MsgCreated();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost([FromHeader] string token, int id)
        {
            var post = await uow.PostRepository.GetById(id);
            if (post == null) 
                return _message.MsgNotFound();
            if (TokenService.DecodeToken(token) != post.UserId) 
                return _message.MsgInvalid();
            uow.PostRepository.Delete(post);
            return _message.MsgDeleted();
        }
    }
}