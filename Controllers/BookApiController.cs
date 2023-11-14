using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LeitourApi.Models;
using LeitourApi.Repository;
using LeitourApi.Interfaces;
using System.IO;
using System.Text.Json.Nodes;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace LeitourApi.Controllers
{
    [Route("api/SearchBy/")]
    [ApiController]
    public class BookApiController : ControllerBase
    {

      
       // private static string API_PARAMS = "&maxResults=20&key=AIzaSyAz_H70Ju10k16gGDt-V-wQnYll-q7q7LY&startIndex=";
        private static string API_PARAMS = "&maxResults=20&startIndex=";
        private static string API_URL = "https://www.googleapis.com/books/v1/volumes?q=";

        public readonly BookApiRepository _bookApi;
        public readonly Message mesage;

        public BookApiController(BookApiRepository bookApi){
            _bookApi = bookApi;
            mesage = new Message("livro","o");    
        }

        [HttpGet("Title/{title}")]
        public async Task<ActionResult<IEnumerable<BookApi>>?> GetByTitle(string title,[FromQuery(Name = Constants.OFFSET)] int page)
        {
            Uri url = new($"{API_URL}intitle:{title}{API_PARAMS}{page}");
            JObject response = await _bookApi.HttpGet(url);
            if((int?) response["Code"] == StatusCodes.Status500InternalServerError)
                return mesage.MsgNotReturned();
            if((int?) response["Code"]  == StatusCodes.Status404NotFound)
                return mesage.MsgNotFound();
            List<BookApi> books = await _bookApi.FormatResponse(response);
            return books;
        }

        [HttpGet("isbn/{isbn}")]
        public async Task<ActionResult<BookApi>?> GetByIsbn(string isbn)
        {
            Uri url = new($"{API_URL}isbn:{isbn}");
            JObject response = await _bookApi.HttpGet(url);
            if((int?) response["Code"] == StatusCodes.Status500InternalServerError)
                return mesage.MsgNotReturned();
            if((int?) response["Code"]  == StatusCodes.Status404NotFound)
                return mesage.MsgNotFound();
            BookApi book = ( await _bookApi.FormatResponse(response))[0];
            return book;
        }
        [HttpGet("author/{author}")]
        public async Task<ActionResult<IEnumerable<BookApi>>?> GetByAuthor(string author,[FromQuery(Name = Constants.OFFSET)] int page)
        {
            Uri url = new($"{API_URL}inauthor:{author}{API_PARAMS}{page}");
            JObject response = await _bookApi.HttpGet(url);
            if((int?) response["Code"] == StatusCodes.Status500InternalServerError)
                return mesage.MsgNotReturned();
            if((int?) response["Code"]  == StatusCodes.Status404NotFound)
                return mesage.MsgNotFound();
            List<BookApi> books = await _bookApi.FormatResponse(response);
            return books;
        }
        [HttpGet("Key/{key}")]
        public async Task<ActionResult<BookApi>?> GetByKey(string key)
        {
            Uri url = new($"https://www.googleapis.com/books/v1/volumes/{key}");
            JObject response = await _bookApi.HttpGet(url);
            if((int?) response["Code"] == StatusCodes.Status500InternalServerError)
                return mesage.MsgNotReturned();
            if((int?) response["Code"]  == StatusCodes.Status404NotFound)
                return mesage.MsgNotFound();
            BookApi book = (await _bookApi.FormatResponse(response))[0];
            return book;
        }
    }
}