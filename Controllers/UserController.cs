using Microsoft.AspNetCore.Mvc;
using LeitourApi.Models;
using LeitourApi.Repository;
using LeitourApi.Interfaces;

namespace LeitourApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUnitOfWork unitOfWork;

    public UserController(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;
    
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        List<User> users = await unitOfWork.User.GetAll();
        return users;
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        User user = await unitOfWork.User.GetById(id);
        return user;
    }
}
