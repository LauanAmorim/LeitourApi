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
public class UserController : ControllerBase
{
    private readonly IUnitOfWork uow;
    private readonly Message message;
    public UserController(IUnitOfWork unitOfWork)
    {
        uow = unitOfWork;
        message = new Message("Usuário", "o");
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers() => await uow.UserRepository.GetAll();

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        User? user = await uow.UserRepository.GetById(id);
        return (user != null) ? user : message.NotFound();
    }

    [HttpPost("register")]
    public async Task<ActionResult<dynamic>> AddUser([FromBody] User newUser)
    {
        User? registeredUser = await uow.UserRepository.GetByCondition(u => u.Email == newUser.Email);
        if (registeredUser != null)
            return message.MsgAlreadyExists();
        uow.UserRepository.Add(newUser);
        User? loggingUser = await uow.UserRepository.GetById(newUser.Id);
        string token = TokenService.GenerateToken(loggingUser);
        return new { user = loggingUser, token };
    }

    [HttpPost("login")]
    public async Task<ActionResult<dynamic>> Authenticate([FromBody] User loggingUser)
    {
        User? registeredUser = await uow.UserRepository.GetByCondition(u => u.Email == loggingUser.Email);
        if (registeredUser == null)
            return message.MsgNotFound();
        if(await uow.UserRepository.IsDeactivated(registeredUser.Id))
            return message.MsgDeactivate();
        if (loggingUser.Password != registeredUser.Password)
            return message.MsgWrongPassword();
        string token = TokenService.GenerateToken(registeredUser);
        return new { user = registeredUser, token };
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<User>> GetUserByEmail(string email)
    {
        User? user = await uow.UserRepository.GetByCondition(u => u.Email == email);
        return (user != null) ? user : message.MsgNotFound();
    }

    [HttpPut("alter")]
    public async Task<IActionResult> PutUser([FromHeader] string token, [FromBody] User user)
    {
        int id = TokenService.DecodeToken(token);
        var registeredUser = await uow.UserRepository.GetById(user.Id);
        if (registeredUser == null)
            return message.MsgNotFound();
        if(await uow.UserRepository.IsDeactivated(user.Id))
            return message.MsgDeactivate();
        if(registeredUser.Id != id)
            return message.MsgInvalid();
        uow.UserRepository.Update(user);
        return message.MsgAlterated();
    }

    [HttpDelete("deactivate")]
    public async Task<IActionResult> DeactivateUser([FromHeader] string token)
    {
        int id = TokenService.DecodeToken(token);
        User? user = await uow.UserRepository.GetById(id);
        if (user == null)
            return message.MsgNotFound();
        if(await uow.UserRepository.IsDeactivated(user.Id))
            return message.MsgDeactivate();
        user.Access = "Desativado";
        uow.UserRepository.Update(user);
        return Ok($"{user.NameUser} foi desativado");
    }
}