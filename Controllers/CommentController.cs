using Microsoft.AspNetCore.Mvc;
using LeitourApi.Models;
using LeitourApi.Repository;
using LeitourApi.Interfaces;
using System.Threading.Tasks;
using LeitourApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LeitourApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly Message _message;
        private readonly IUnitOfWork uow;
        public CommentController(IUnitOfWork unitOfWork)
        {
            uow = unitOfWork;
            _message = new Message("Comentário", "o");
        }

        [HttpGet("/api/Posts/[Controller]/{id}")]
        public async Task<ActionResult<List<Comment>>> GetComments(int id,[FromQuery(Name = Constants.OFFSET)] int page)
        {
            var Post = await uow.PostRepository.GetById(id);
            if(Post == null)
                return new Message("Post","o").MsgNotFound(); 
            
            var comment = await uow.CommentRepository.GetAllByCondition(c => c.PostId == Post.Id,page);
            return comment != null ? comment : _message.MsgNotFound();
        }

        [Obsolete("Write a better path name")]
        [HttpGet("/api/Posts/[Controller]/comment/{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var Comment = await uow.CommentRepository.GetById(id);
            return Comment != null ? Comment : _message.MsgNotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment([FromHeader] string token, Comment comment)
        {
            User? user = await uow.UserRepository.GetById(TokenService.DecodeToken(token));
            if(user == null)
                return new Message("Usuari","o").MsgNotFound();
            if (user.Id != comment.UserId)
                return _message.MsgInvalid();
            if (user.Access == "Desativado")
                return _message.MsgDeactivate();
            comment.PostDate = DateTime.UtcNow;
            uow.CommentRepository.Add(comment);
            return CreatedAtAction("PostComment", comment);
        }


        [HttpPut("/api/Posts/[Controller]/{id}")]
        public async Task<IActionResult> PutComment([FromHeader] string token, int id, [FromBody] Comment updateComment)
        {
            User? user = await uow.UserRepository.GetById(TokenService.DecodeToken(token));
            if(user == null)
                return new Message("Usuari","o").MsgNotFound();
            if (user.Access == "Desativado")
                return _message.MsgDeactivate();
            var Comment = await uow.CommentRepository.GetById(id);
            if (Comment == null) 
                return _message.MsgNotFound();
            if (id != updateComment.CommentId || TokenService.DecodeToken(token) != updateComment.UserId)
                return _message.MsgInvalid();
            updateComment.AlteratedDate = DateTime.UtcNow;
            uow.CommentRepository.Update(updateComment);
            return _message.MsgCreated() ;
        }
        
        [HttpDelete("/api/Posts/[Controller]/{id}")]
        public async Task<IActionResult> DeleteComment([FromHeader] string token, int id)
        {
            var Comment = await uow.CommentRepository.GetById(id);
            if (Comment == null) 
                return _message.MsgNotFound();
            User? user = await uow.UserRepository.GetById(TokenService.DecodeToken(token));
            if(user == null)
                return new Message("Usuari","o").MsgNotFound();
            if (user.Id != Comment.UserId)
                return _message.MsgInvalid();
            if (user.Access == "Desativado")
                return _message.MsgDeactivate();
            uow.CommentRepository.Delete(Comment);
            return _message.MsgDeleted();
        }
    }
}