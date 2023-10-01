using Microsoft.AspNetCore.Mvc;
using LeitourApi.Models;
using LeitourApi.Repository;
using LeitourApi.Interfaces;
using LeitourApi.Data;
using Microsoft.AspNetCore.Identity;

namespace LeitourApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SavedBooksController : ControllerBase
{
    private readonly IUnitOfWork uow;
    private readonly Message message;
    public SavedBooksController(IUnitOfWork unitOfWork)
    {
        uow = unitOfWork;
        message = new Message("Usuário", "o");
    }

    [HttpGet]
    public async Task<ActionResult<List<SavedBook>>> GetAllSaved([FromHeader] string token)
    {
        int id = TokenService.DecodeToken(token);
        if(await uow.UserRepository.GetById(id) == null)
            return message.MsgInvalid();
        List<SavedBook> saved = await uow.SavedRepository.GetAllByCondition(s => s.UserId == id);
        return (saved != null) ? saved : message.MsgNotFound();
    }

    [HttpGet("User/{email}")]
    public async Task<ActionResult<List<SavedBook>>> GetAllSavedByEmail(string email)
    {
        User? user = await uow.UserRepository.GetByCondition(u => u.Email == email);
        if(user == null)
            return message.NotFound();
        List<SavedBook> saved = await uow.SavedRepository.GetAllByCondition(s => s.UserId == user.Id && s.Public == true);
        return (saved != null) ? saved : message.MsgNotFound();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SavedBook>> GetSaved(int id)
    {
        var saved = await uow.SavedRepository.GetById(id);
        return (saved != null) ? saved : message.MsgNotFound();
    }

    [HttpPost]
    public async Task<ActionResult<SavedBook>> AddSaved([FromHeader] string token, [FromBody] SavedBook saved)
    {
        int userId = TokenService.DecodeToken(token);
        User? user = await uow.UserRepository.GetById(userId);
        if(user == null)
            return message.MsgNotFound();
        if(user.Access == "Desativado")
            return message.MsgDeactivate();
        if(userId != saved.UserId)
            return message.MsgInvalid();
        uow.SavedRepository.Add(saved);
        return CreatedAtAction("AddSaved", new { saved });
    }



    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateSaved([FromHeader] string token,int id)
    {
        int userId = TokenService.DecodeToken(token);
        User? user = await uow.UserRepository.GetById(userId);
        if(user == null)
            return message.MsgNotFound();
        if(user.Access == "Desativado")
            return message.MsgDeactivate();
        var saved = await uow.SavedRepository.GetById(id);
        if(saved == null)
            return message.MsgNotFound();
        saved.Public = !saved.Public;
        uow.SavedRepository.Update(saved);
        return message.MsgPublic(saved.Public);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSaved([FromHeader] string token, int savedId)
    {
        int id = TokenService.DecodeToken(token);
        User? user = await uow.UserRepository.GetById(id);
        if(user == null)
            return message.MsgNotFound();
        if(user.Access == "Desativado")
            return message.MsgDeactivate();
        var saved = await uow.SavedRepository.GetById(id);
        if(saved == null)
            return message.MsgNotFound();
        uow.UserRepository.Update(user);
        return message.MsgAlterated();
    }
}