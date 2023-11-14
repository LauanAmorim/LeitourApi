using Microsoft.AspNetCore.Mvc;
using LeitourApi.Models;
using LeitourApi.Repository;
using LeitourApi.Interfaces;
using LeitourApi.Data;
using Microsoft.AspNetCore.Identity;
using System.Data;


namespace LeitourApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FollowController : ControllerBase
{
    private readonly IUnitOfWork uow;
    private readonly Message message;


    public FollowController(IUnitOfWork unitOfWork)
    {
        uow = unitOfWork;
        message = new Message("Usuário", "o");
    }


    [HttpPost("follow/{email}")]
    public async Task<IActionResult> Follow([FromHeader] string token, string email)
    {
        int id = TokenService.DecodeToken(token);
        User? user = await uow.UserRepository.GetById(id);
        if (user == null)
            return message.MsgInvalid();

        User? followingEmail = await uow.UserRepository.GetByEmail(email);

        if (followingEmail == null)
            return message.MsgNotFound();

        await uow.UserRepository.Follow(id,email);
        return Ok($"Você está seguindo {email}");
    }

    [HttpPost("unfollow/{email}")]
    public async Task<IActionResult> UnfollowFollow([FromHeader] string token, string email)
    {
        int id = TokenService.DecodeToken(token);
        User? user = await uow.UserRepository.GetById(id);
        if (user == null)
            return message.MsgInvalid();

        User? followingEmail = await uow.UserRepository.GetByEmail(email);

        if (followingEmail == null)
            return message.MsgNotFound();

        await uow.UserRepository.Unfollow(id, email);
        return Ok($"Você não está mais seguindo {email}");
    }


    [HttpGet("followingList/{email}")]
    public async Task<ActionResult<IEnumerable<User>>> FollowingFollow(string email)
    {

        User? user = await uow.UserRepository.GetByEmail(email);
        if (user == null)
            return message.MsgNotFound();

        var followingFollows = await uow.UserRepository.GetFollowing(user.Email);

        if (followingFollows == null)
            return NotFound($"{user.NameUser} não está seguindo ninguém");
        return followingFollows;
    }

    [HttpGet("followerList/{email}")]
    public async Task<ActionResult<IEnumerable<User>>> FollowersFollow(string email)
    {
        User? user = await uow.UserRepository.GetByEmail(email);
        if (user == null)
            return message.MsgNotFound();

        var followingFollows = await uow.UserRepository.GetFollowers(user.Id);

        if (followingFollows == null)
            return NotFound($"{user.NameUser} não tem seguidores");

        return followingFollows;
    }
}
