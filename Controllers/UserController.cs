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

    [HttpGet("criptografar/{message}")]
    public async Task<IActionResult> Criptografar(string message)
    {
        string criptografado = Cryptography.Criptografar(message);
        return Ok(criptografado);
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers([FromQuery(Name = Constants.OFFSET)] int page) => await uow.UserRepository.GetAll(page);

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        User? user = await uow.UserRepository.GetById(id);
        return (user != null) ? user : message.NotFound();
    }

    [HttpPost("register")]
    public async Task<ActionResult<dynamic>> AddUser([FromBody] User newUser)
    {
        User? registeredUser = await uow.UserRepository.GetByEmail(newUser.Email);
        if (registeredUser != null)
            return message.MsgAlreadyExists();
        newUser.Password = Hash.GerarHash(newUser.Password);
        uow.UserRepository.Add(newUser);
        User? loggingUser = await uow.UserRepository.GetUser(newUser.Id);
        string token = TokenService.GenerateToken(loggingUser);
        loggingUser.Password = newUser.Password;
        return new { user = loggingUser, token };
    }

    [HttpPost("login")]
    public async Task<ActionResult<dynamic>> Authenticate([FromBody] User loggingUser)
    {
        User? registeredUser = await uow.UserRepository.GetByEmail(loggingUser.Email);
        if (registeredUser == null)
            return message.MsgNotFound();
        if(await uow.UserRepository.IsDeactivated(registeredUser.Id))
            return message.MsgDeactivate();
        if (Hash.GerarHash(loggingUser.Password) != registeredUser.Password)
        if (Hash.GerarHash(loggingUser.Password) != registeredUser.Password)
            return message.MsgWrongPassword();
        string token = TokenService.GenerateToken(registeredUser);
        registeredUser.Password = loggingUser.Password;
        return new { user = registeredUser, token };
    }
    [HttpPost("autologin")]
    public async Task<ActionResult<dynamic>> AutoLogin([FromHeader] string token)
    {
        int id = TokenService.DecodeToken(token);
        var registeredUser = await uow.UserRepository.GetUser(id);
        if (registeredUser == null)
            return message.MsgNotFound();
        if(await uow.UserRepository.IsDeactivated(id))
            return message.MsgDeactivate();
        if(registeredUser.Id != id)
            return message.MsgInvalid();
        return new { user = registeredUser, token };
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<User>> GetUserByEmail(string email)
    {
        User? user = await uow.UserRepository.GetByEmail(email);
        return (user != null) ? user : message.MsgNotFound();
    }

    [HttpPut("alter")]
    public async Task<IActionResult> PutUser([FromHeader] string token, [FromBody] User user)
    {
        int id = TokenService.DecodeToken(token);
        var registeredUser = await uow.UserRepository.GetUser(user.Id);
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
        User? user = await uow.UserRepository.GetUser(id);
        if (user == null)
            return message.MsgNotFound();
        if(await uow.UserRepository.IsDeactivated(user.Id))
            return message.MsgDeactivate();
        user.Access = "Desativado";
        uow.UserRepository.Update(user);
        return Ok($"{user.NameUser} foi desativado");
    }
}