using Microsoft.AspNetCore.Mvc;

namespace LeitourApi.Models;

[ApiExplorerSettings(IgnoreApi = true)]
public class Message : ControllerBase
{
    public string CREATE = "criad";
    public string UPDATE = "atualizad";
    public string DELETE = "deletad";
    public string DEACTIVATE = "desativad";
    //public const string CREATE = "criad";
    private string controller {get;set;}
    private string artigo {get;set;}
    
    public Message(string controller, string artigo){
        this.controller = controller;
        this.artigo = artigo;
    }

    public ActionResult MsgNotFound() => NotFound($"{controller} não existe.");
    public ActionResult MsgInvalid() => Unauthorized("Autenticação invalida, logue novamente.");

    public ObjectResult InternalError(string acao) => StatusCode(StatusCodes.Status500InternalServerError, $"{controller} não pode ser {acao}{artigo}");
    public ActionResult MsgWrongPassword() => BadRequest("Senha incorreta.");
    public ActionResult MsgAlreadyExists() => BadRequest($"{controller} já existe");
    public ActionResult MsgSaved() => Ok($"{controller} foi salv{artigo}");
    public ActionResult MsgCreated() => StatusCode(StatusCodes.Status201Created,$"{controller} foi {CREATE}{artigo}");
    public ActionResult MsgAlterated() => Ok($"{controller} foi {UPDATE}{artigo}");
    public ActionResult MsgDeleted() => Ok($"{controller} foi {DELETE}{artigo}");
    public ActionResult MsgDeactivate() => Ok($"O usuario foi desativado");
    public ActionResult MsgDebug(string message) => Ok($"{message}");

    public ActionResult MsgPublic(bool isPublic)
    {
        string publico = isPublic ? "publico" : "privado";
        return Ok($"Este livro agora está {publico}");
    }

    public ActionResult MsgNotReturned() => StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao pesquisar {controller}. Verifique a conexão de internet.");

    //public ActionResult<List<Annotation>> MsgPrivated() => BadRequest("Essas anotações são privadas");
}