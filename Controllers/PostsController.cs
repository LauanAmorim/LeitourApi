using Microsoft.AspNetCore.Mvc;
using LeitourApi.Models;
using LeitourApi.Repository;
using LeitourApi.Interfaces;
using System.Threading.Tasks;
using LeitourApi.Data;
using LeitourApi.Services;
using NuGet.Common;
using System.ComponentModel;

namespace LeitourApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly MessageService _message;
        private readonly IUnitOfWork uow;
       
        public const string offset = "offset";

        public PostsController(IUnitOfWork unitOfWork)
        {
            uow = unitOfWork;
            _message = new MessageService("Post", "o");
        }


        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetPosts([FromHeader] string? token,[FromQuery(Name = offset)] int page)
        {
            int id = -1;
            if(token != null)
                id = TokenService.DecodeToken(token);
            List<Post>? posts;
            if(id == -1)
                posts = await uow.PostRepository.GetAll(page);
            else
                posts = await uow.PostRepository.GetAll(page,id);
            return posts != null ? posts : _message.MsgNotFound();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await uow.PostRepository.GetById(id);
            return post != null ? post : _message.MsgNotFound();
        }


        [HttpGet("email/{email}")]
        public async Task<ActionResult<List<Post>>> GetPostsByEmail(string email,[FromQuery(Name = offset)] int page)
        {
            var user = await uow.UserRepository.GetByCondition(u => u.Email == email);
            if(user == null)
                return new MessageService("Usuario","o").MsgNotFound();
            var posts = await uow.PostRepository.GetAllByCondition(p => p.UserId == user.Id,page);
            return posts != null ? posts : _message.MsgNotFound();
        }


        [HttpPost]
        public async Task<ActionResult<Post>> PostPost([FromHeader] string token, Post post)
        {
            if (TokenService.DecodeToken(token) != post.UserId)
                return _message.MsgInvalid();
            post.AlteratedDate = DateTime.UtcNow;
            uow.PostRepository.Add(post);
            return CreatedAtAction("PostPost", new { id = post.Id }, post);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost([FromHeader] string token, int id, [FromBody] Post updatePost)
        {
            var post = await uow.PostRepository.GetById(id);
            if (post == null) 
                return _message.MsgNotFound();
            if (id != updatePost.Id || TokenService.DecodeToken(token) != updatePost.UserId)
                return _message.MsgInvalid();
            updatePost.AlteratedDate = DateTime.UtcNow;
            uow.PostRepository.Update(updatePost);
            return _message.MsgAlterated();
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