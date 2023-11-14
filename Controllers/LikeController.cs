using Microsoft.AspNetCore.Mvc;
using LeitourApi.Models;
using LeitourApi.Repository;
using LeitourApi.Interfaces;
using System.Threading.Tasks;
using LeitourApi.Data;

namespace LeitourApi.Controllers
{
    [Route("api/Posts")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly Message _message;
        private readonly IUnitOfWork uow;
       
        public const string offset = "offset";

        public LikeController(IUnitOfWork unitOfWork)
        {
            uow = unitOfWork;
            _message = new Message("Like", "o");
        }


        [HttpPost("like/{id}")]
        public async Task<IActionResult> Like([FromHeader] string token,int id)
        {
            int userId = TokenService.DecodeToken(token);
            if (userId == -1)
                return new Message("Usuario","o").MsgNotFound();
            if(await uow.UserRepository.IsDeactivated(userId))
                return _message.MsgDeactivate();
            Post? post = await uow.PostRepository.GetById(id);
            if(post == null)
                return _message.MsgNotFound();
            int result = await uow.PostRepository.Like(userId,id);
            if(result == -1)
                return BadRequest("Erro");
            return result == 0 ? Ok("Post Curtido") : Ok("Post Descurtido");
        }
    }
}