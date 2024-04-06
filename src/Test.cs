using System.Reflection.Metadata;
using docvault_frontend.Data;
using Newtonsoft.Json;
using docvault_frontend.Models;

public static class Test
{

    private static readonly string FILE_NAME = "Hello World.docx";

    private static readonly string NEW_NAME = "Hello World (copy).docx";

    private static readonly byte[] DATA = File.ReadAllBytes(FILE_NAME);
    public static async Task TestBackend()
    {
        BackendCommunicator backendCommunicator = new BackendCommunicator();
        if (!await backendCommunicator.TestRoot())
        {
            Console.WriteLine("Failed TestRoot");
            return;
        }
        else
        {
            Console.WriteLine("OK TestRoot");
        }
        if (!await backendCommunicator.UploadDocument(FILE_NAME, DATA, BuildTags()))
        {
            Console.WriteLine("Failed UploadDocument");
            return;
        }
        else
        {
            Console.WriteLine("OK UploadDocument");
        }
        URLReply? url = await backendCommunicator.GetDocumentDownloadURL(FILE_NAME);
        if (url is null)
        {
            Console.WriteLine("Failed GetDocumentDownloadURL");
            return;
        }
        else
        {
            Console.WriteLine($"URL is {url.ToString()}");
        }
        DocumentRef? document = await backendCommunicator.GetDocument(FILE_NAME);
        if (document is null)
        {
            Console.WriteLine("Failed GetDocument");
            return;
        }
        else
        {
            Console.WriteLine($"Document is {document.ToString()}");
        }
        List<DocumentRef> documents = await backendCommunicator.GetDocuments();
        if (documents is null)
        {
            Console.WriteLine("Failed GetDocuments (null)");
            return;
        }
        if (documents.Count < 1)
        {
            Console.WriteLine("Failed GetDocuments (count < 1)");
            return;
        }
        else
        {
            Console.WriteLine($"Documents are {JsonConvert.SerializeObject(documents)}");
        }
        if (!await backendCommunicator.UpdateTags(FILE_NAME, BuildNewTags()))
        {
            Console.WriteLine("Failed UpdateTags");
            return;
        }
        else
        {
            Console.WriteLine("OK UpdateTags");
        }
        if (!await backendCommunicator.UpdateName(FILE_NAME, NEW_NAME))
        {
            Console.WriteLine("Failed UpdateName");
            return;
        }
        else
        {
            Console.WriteLine("OK UpdateName");
        }
        if (!await backendCommunicator.DeleteDocument(NEW_NAME))
        {
            Console.WriteLine("Failed DeleteDocument");
            return;
        }
        else
        {
            Console.WriteLine("OK DeleteDocument");
        }
    }

    private static List<Tag> BuildTags()
    {
        List<Tag> Tags = new List<Tag>(2);
        Tags.Add(new Tag("File-Type", "Microsoft Word Document"));
        Tags.Add(new Tag("Author", "Sydney"));
        return Tags;
    }

    private static List<Tag> BuildNewTags()
    {
        List<Tag> Tags = new List<Tag>(2);
        Tags.Add(new Tag("File-Type", "Microsoft Word Document"));
        Tags.Add(new Tag("Author", "Sydney"));
        Tags.Add(new Tag("Creation-Date", DateTime.UtcNow.ToString()));
        return Tags;
    }
}