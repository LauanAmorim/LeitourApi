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
        message = new Message("Livro", "o");
    }

    [HttpGet]
    public async Task<ActionResult<List<SavedBook>>> GetAllSaved([FromHeader] string token,[FromQuery(Name = Constants.OFFSET)] int page)
    {
        int id = TokenService.DecodeToken(token);
        if(await uow.UserRepository.GetById(id) == null)
            return message.MsgInvalid();
        List<SavedBook> saved = await uow.SavedRepository.GetAllByCondition(s => s.UserId == id,page);
        return (saved != null) ? saved : message.MsgNotFound();
    }

    [HttpGet("User/{email}")]
    public async Task<ActionResult<List<SavedBook>>> GetAllSavedByEmail(string email,[FromQuery(Name = Constants.OFFSET)] int page)
    {
        User? user = await uow.UserRepository.GetByCondition(u => u.Email == email);
        if(user == null)
            return message.NotFound();
        List<SavedBook> saved = await uow.SavedRepository.GetAllByCondition(s => s.UserId == user.Id && s.Public == true,page);
        return (saved != null) ? saved : message.MsgNotFound();
    }



    [HttpGet("{key}")]
    public async Task<ActionResult<SavedBook>> GetSaved(string key,[FromHeader] string token)
    {
        int userId = TokenService.DecodeToken(token);
        User? user = await uow.UserRepository.GetById(userId);
        if(user == null)
            return message.MsgNotFound();
        if(user.Access == "Desativado")
            return message.MsgDeactivate();
        SavedBook saved = await uow.SavedRepository.GetByCondition(s => s.UserId == user.Id && s.BookKey == key);
        return (saved != null) ? saved : message.MsgNotFound();
    }
    [HttpGet("new/{key}")]
    public async Task<ActionResult<dynamic>> GetSavedNew(string key,[FromHeader] string token)
    {
        int userId = TokenService.DecodeToken(token);
        User? user = await uow.UserRepository.GetById(userId);
        if(user == null)
            return message.MsgNotFound();
        if(user.Access == "Desativado")
            return message.MsgDeactivate();
        SavedBook saved = await uow.SavedRepository.GetByCondition(s => s.UserId == user.Id && s.BookKey == key);
        if(saved == null)
            return message.MsgNotFound();
        var annotation = await uow.AnnotationRepository.GetAllByCondition(a => a.SavedBookId == saved.Id);
        return new {saved, annotation};
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
        var savedBook = await uow.SavedRepository.GetByCondition(s => s.BookKey == saved.BookKey && s.UserId == userId);
        if(savedBook != null)
            return message.MsgAlreadyExists();
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
        var saved = await uow.SavedRepository.GetById(savedId);
        if(saved == null)
            return message.MsgNotFound();
        uow.SavedRepository.Delete(saved);
        return message.MsgDeleted();
    }
}