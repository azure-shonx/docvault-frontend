using System.Text;
using Newtonsoft.Json;
using docvault_frontend.Models;

namespace docvault_frontend.Data;

public class BackendCommunicator
{
    private readonly HttpClient httpClient = new HttpClient();

    public async Task<bool> TestRoot()
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Program.BACKEND);
        return await WriteRequest(request, null);
    }

    public async Task<bool> UploadDocument(string FileName, byte[] Data, List<Tag> Tags)
    {
        UploadDocumentRequest udr = new UploadDocumentRequest(FileName, Convert.ToBase64String(Data), Tags);
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, Program.BACKEND + "document/upload/");
        return await WriteRequest(request, udr);
    }

    public async Task<URLReply?> GetDocumentDownloadURL(string FileName)
    {
        GetDocumentRequest gdr = new GetDocumentRequest(FileName);
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Program.BACKEND + "document/download/");
        return await WriteRequestWithReply<URLReply?>(request, gdr);
    }

    public async Task<DocumentRef?> GetDocument(string FileName)
    {
        GetDocumentRequest gdr = new GetDocumentRequest(FileName);
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Program.BACKEND + "document/get/");
        return await WriteRequestWithReply<DocumentRef?>(request, gdr);
    }

    public async Task<List<DocumentRef>> GetDocuments()
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Program.BACKEND + "document/getall/");
        List<DocumentRef>? reply = await WriteRequestWithReply<List<DocumentRef>>(request, null);
        if (reply is null)
            return new List<DocumentRef>(0);
        return reply;
    }

    public async Task<bool> DeleteDocument(string FileName)
    {
        DeleteDocumentRequest ddr = new DeleteDocumentRequest(FileName);
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, Program.BACKEND + "document/delete/");
        return await WriteRequest(request, ddr);
    }

    public async Task<bool> UpdateTags(string FileName, List<Tag> Tags)
    {
        UpdateTagsRequest utr = new UpdateTagsRequest(FileName, Tags);
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Program.BACKEND + "document/update/tags/");
        return await WriteRequest(request, utr);
    }

    public async Task<bool> UpdateName(string OldName, string NewName)
    {
        RenameDocumentRequest rdr = new RenameDocumentRequest(OldName, NewName);
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Program.BACKEND + "document/update/name/");
        return await WriteRequest(request, rdr);
    }

    private async Task<bool> WriteRequest(HttpRequestMessage request, Request? data)
    {
        if (data is not null)
            request.Content = new StringContent(data.ToString(), Encoding.UTF8, "application/json");
        using (HttpResponseMessage response = await httpClient.SendAsync(request))
        {
            var statusCode = response.StatusCode;
            if ((int)statusCode != 200)
            {
                Console.WriteLine($"BackendCommunicator got response {(int)statusCode}");
                return false;
            }
            return true;
        }
    }

    private async Task<T?> WriteRequestWithReply<T>(HttpRequestMessage request, Request? data)
    {
        if (data is not null)
            request.Content = new StringContent(data.ToString(), Encoding.UTF8, "application/json"); ;
        using (HttpResponseMessage response = await httpClient.SendAsync(request))
        {
            System.Net.HttpStatusCode statusCode = response.StatusCode;
            if ((int)statusCode != 200)
            {
                Console.WriteLine($"BackendCommunicator got response {statusCode}");
                return default;
            }
            return await GetReply<T>(response.Content.ReadAsStream());
        }
    }

    private async Task<T?> GetReply<T>(Stream stream)
    {
        string json = await new StreamReader(stream).ReadToEndAsync();
        if (String.IsNullOrEmpty(json))
        {
            return default;
        }
        return JsonConvert.DeserializeObject<T>(json);
    }

}
