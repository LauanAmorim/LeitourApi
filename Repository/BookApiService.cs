

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
    readonly HttpClient client = new();
    public async Task<JObject> HttpGet(Uri url)
    {
        JObject jsonObject = new();
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                jsonObject = JObject.Parse(jsonResponse);
                if (jsonObject["items"].IsNullOrEmpty())
                    jsonObject["Code"] = StatusCodes.Status404NotFound;
                else
                    jsonObject["Code"] = StatusCodes.Status200OK;
            }
            else
                jsonObject["Code"] = StatusCodes.Status500InternalServerError;
        }
        catch (Exception) { jsonObject["Code"] = StatusCodes.Status500InternalServerError; }
        return jsonObject;
    }

    public List<BookApi>? FormatResponse(JObject response)
    {
        List<BookApi> Books = new();
        try
        {
            JArray? jArray = (JArray?)response["items"];
            if(jArray == null)
                return null;
            foreach (JObject jsonItems in jArray.Cast<JObject>())
            {
                BookApi book = new();

                book.Key = GetStringValue(jsonItems, "id");

                JObject? jsonInfo = (JObject?)jsonItems["volumeInfo"];
                if(jsonInfo == null)

                    continue;

                book.Title = GetStringValue(jsonInfo, "title");
                book.Authors = GetStringFromArrayValue(jsonInfo, "authors");
                book.Publisher = GetStringValue(jsonInfo, "publisher");
                book.PublishedDate = GetStringValue(jsonInfo, "publishedDate");
                book.Description = GetStringValue(jsonInfo, "description");
                book.Language = GetStringValue(jsonInfo, "language");
                book.Pages = GetIntValue(jsonInfo, "pageCount");
                JObject? jsonImage = (JObject?) jsonInfo["imageLinks"];
                book.Cover = (jsonImage != null) ? GetStringValue(jsonImage, "thumbnail").Replace("zoom=1","zoom=0") : "";
                book.ISBN_10 = "";
                book.ISBN_13 = "";

                JObject[] jsonArray = jsonInfo["industryIdentifiers"].ValueAsArray<JObject>();
                foreach (JObject jObj in jsonArray)
                {
                    if (jObj["type"].ToString() == "ISBN_10")
                        book.ISBN_10 = GetStringValue(jObj, "identifier");
                    else if (jObj["type"].ToString() == "ISBN_13")
                        book.ISBN_13 = GetStringValue(jObj, "identifier");
                }
                Books.Add(book);
            }
        }
        catch (Exception) { }
        return Books;
    }

    private string GetStringValue(JObject? json, string key)
    {
        try
        {
            if (json != null && json[key] != null)
            {
                return json[key].ToString();
            }
            return "";
        }
        catch (Exception)
        {
            return "";
        }
    }

    private string GetStringFromArrayValue(JObject json, string key)
    {
        try
        {
            string[] array = json[key].ValueAsArray<string>();
            return string.Join(",", array);
        }
        catch { return ""; }
    }

    private int GetIntValue(JObject json, string key)
    {
        try { return (int)json[key]; }
        catch { return 0; }
    }
}
