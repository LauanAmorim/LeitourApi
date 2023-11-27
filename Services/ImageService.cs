using LeitourApi.Data;
using LeitourApi.Interfaces;
using LeitourApi.Models;
using LeitourApi.Services;
using Microsoft.AspNetCore.Mvc;

public class ImageService
{
 


    public ImageService(IUnitOfWork unitOfWork)
    {
      
    }

   /* public async Task<ActionResult> Validate(string token,User user)
    {
        int id = TokenService.DecodeToken(token);
        var registeredUser = await uow.UserRepository.GetUser(id);
        if (registeredUser == null)
            return message.MsgNotFound();
        if (await uow.UserRepository.IsDeactivated(id))
            return message.MsgDeactivate();
        if (registeredUser.Id != user.Id)
            return message.MsgInvalid();
        return Task<"SUCCESS">;
    }
    public async Task<ActionResult> Validate(string token)
    {
        int id = TokenService.DecodeToken(token);
        var registeredUser = await uow.UserRepository.GetUser(id);
        if (registeredUser == null)
            return message.MsgNotFound();
        if (await uow.UserRepository.IsDeactivated(id))
            return message.MsgDeactivate();
        return message.MsgSuccess();
    }*/
}

