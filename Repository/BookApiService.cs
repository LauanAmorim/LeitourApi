

using LeitourApi.Models;
using System.Text.Json;
using System;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using static System.Collections.IEnumerable;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;

namespace LeitourApi.Repository;


public class BookApiRepository
{   
    
    /*private static string QUERY_PARAM = "q";
    private static string MODE = "mode";
    private static string LIMIT = "limit";
    private static string OFFSET = "offset";*/

    readonly HttpClient client = new();
    const int limit = 10;

    public async Task<JObject> HttpGet(Uri url){
        JObject jsonObject = new();
        try{
        HttpResponseMessage response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            jsonObject = JObject.Parse(jsonResponse);
            if(jsonObject["docs"].IsNullOrEmpty())
                jsonObject["Code"] = StatusCodes.Status404NotFound;
            else
                jsonObject["Code"] = StatusCodes.Status200OK;
        }
        else
            jsonObject["Code"] = StatusCodes.Status500InternalServerError;
        }catch(Exception e){jsonObject["Code"] = StatusCodes.Status500InternalServerError;}
        return jsonObject;
    }

    public Task<List<BookApi>> FormatResponse(JObject response)
    {
        List<BookApi> Books = new();
        try{
        JArray jArray = (JArray)response["docs"];
        foreach(JObject jsonItems in jArray.Cast<JObject>())
        {
            BookApi book = new();
       
            string key = jsonItems["key"].ToString();; 
            book.Key = key;

          //  JsonObject jsonVolumeInfo = jsonItems["volumeInfo"].AsObject();

            book.Title = jsonItems["title"].ToString();

        
            string[] authorArray = jsonItems["author_name"].ValueAsArray<string>();
            book.Authors = String.Join(",", authorArray);
            
            string[] publisherArray = jsonItems["publisher"].ValueAsArray<string>();
            book.Publisher = publisherArray.FirstOrDefault();
            

            book.PublishedDate= jsonItems["first_publish_year"].ToString();

           
            book.Description = "";//jsonItems["description"].ToString();

            
            try {string[] languageArray = jsonItems["language"].ValueAsArray<string>(); 
            book.Language = String.Join(",", languageArray);}
            catch{book.Language = "";}

            try { book.Pages = (int) jsonItems["number_of_pages_median"];}
            catch{ book.Pages = 0;}

        /*    try { book.BookIsbn = jsonItems["isbn"].ValueAsArray<string>()[0];}
            catch{ book.BookIsbn = "";}*/
           

            /*book.Categories = "null";

            string[] isbn = jsonItems["industryIdentifiers"].ValueAsArray<string>();

            //try { book.Categories = (string)jsonVolumeInfo["categories"].AsArray().AsValue(); }
            //catch{ book.Categories = "null";}
           /* */

            try { book.Cover = $"https://covers.openlibrary.org/b/olid/{key}-L.jpg";}
            catch{ book.Cover = "";}
            
            Books.Add(book);
        }
        }
        catch(Exception e){}
        return Task.FromResult(Books);
    }
}
