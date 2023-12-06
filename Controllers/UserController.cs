using Microsoft.AspNetCore.Mvc;
using LeitourApi.Models;
using LeitourApi.Repository;
using LeitourApi.Interfaces;
using LeitourApi.Data;
using LeitourApi.Services;
using System.IO;
using System;
using Microsoft.AspNetCore.Identity;
using System.Data;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;


namespace LeitourApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUnitOfWork uow;
    private readonly MessageService message;


    public UserController(IUnitOfWork unitOfWork)
    {
        uow = unitOfWork;
        message = new MessageService("Usuário", "o");
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
        newUser.Password = CryptographyService.GenerateHash(newUser.Password);
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
        if (await uow.UserRepository.IsDeactivated(registeredUser.Id))
            return message.MsgDeactivate();
        if (CryptographyService.GenerateHash(loggingUser.Password) != registeredUser.Password)
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
        if (await uow.UserRepository.IsDeactivated(id))
            return message.MsgDeactivate();
        if (registeredUser.Id != id)
            return message.MsgInvalid();
        return new { user = registeredUser, token };
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<User>> GetUserByEmail(string email)
    {
        User? user = await uow.UserRepository.GetByEmail(email);
        return (user != null) ? user : message.MsgNotFound();
    }

    [HttpGet("userStatistics/{email}")]
    public async Task<ActionResult<int[]>> GetUserStatisticsByEmail(string email)
    {
        User? user = await uow.UserRepository.GetByEmail(email);
        if (user == null)
            return new int[] { 0, 0, 0, 0 };
        int id = user.Id;
        int savedBooks = uow.SavedRepository.CountByCondition(s => s.UserId == id);
        int posts = uow.PostRepository.CountByCondition(s => s.UserId == id);
        int following = (await uow.UserRepository.GetFollowing(email)).Count;
        int followers = (await uow.UserRepository.GetFollowers(id)).Count;
        return new int[] { posts, savedBooks, following, followers };
    }

    [HttpGet("username/{username}")]
    public async Task<ActionResult<List<User>>> GetUserByUsername([FromQuery(Name = Constants.OFFSET)] int page, string username)
    {
        List<User>? user = await uow.UserRepository.GetByUsername(page, username);
        return (user != null) ? user : message.MsgNotFound();
    }

    [HttpPut("alter")]
    public async Task<IActionResult> PutUser([FromHeader] string token, [FromBody] User updatedUser)
    {
        int id = TokenService.DecodeToken(token);
        var user = await uow.UserRepository.GetUser(id);
        if (user == null)
            return message.MsgNotFound();
        if (await uow.UserRepository.IsDeactivated(id))
            return message.MsgDeactivate();
        if (updatedUser.Id != id)
            return message.MsgInvalid();
        uow.UserRepository.Update(updatedUser);
        return message.MsgAlterated();
    }

    [HttpPost("uploadImage")]
    public async Task<ActionResult<dynamic>> SendImage([FromHeader] string token, IFormFile file)
    {
        int id = TokenService.DecodeToken(token);
        var user = await uow.UserRepository.GetUser(id);
        if (user == null)
            return message.MsgNotFound();
        if (await uow.UserRepository.IsDeactivated(id))
            return message.MsgDeactivate();

        if (file != null && file.Length > 0)
        {
            try
            {
                string folder = Regex.Replace(user.Email, @"(\s+|@|&|,|\.|,|´|\[|\]|\{|\}|\:|~|\\|\/|\*|'|\(|\)|<|>|#)", "$");
                string PATH = $"Images/Users/{folder}";
                 if(System.IO.File.Exists(user.ProfilePhoto))
                    System.IO.File.Delete(user.ProfilePhoto);
                if (!Directory.Exists(PATH))
                    Directory.CreateDirectory(PATH);
                string FILE = PATH+"/"+file.FileName;
                FileStream filestream = System.IO.File.Create(FILE);
                await file.CopyToAsync(filestream);
    
                filestream.Flush();
                filestream.Close();
                user.ProfilePhoto = FILE;
                uow.UserRepository.Update(user);
                return Ok("A foto foi atualizada");
            }
            catch (Exception ex)
            {
                return BadRequest("ferrou " + ex.ToString());
            }
        }
        else
        {
            return BadRequest("Envie outra foto");
        }
    }


    [HttpDelete("deactivate")]
    public async Task<IActionResult> DeactivateUser([FromHeader] string token)
    {
        int id = TokenService.DecodeToken(token);
        User? user = await uow.UserRepository.GetUser(id);
        if (user == null)
            return message.MsgNotFound();
        if (await uow.UserRepository.IsDeactivated(user.Id))
            return message.MsgDeactivate();
        user.Access = "Desativado";
        uow.UserRepository.Update(user);
        return Ok($"{user.NameUser} foi desativado");
    }
}