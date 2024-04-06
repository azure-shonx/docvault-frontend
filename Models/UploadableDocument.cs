using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace docvault_frontend.Models;

public class UploadableDocument
{
    [Display(Name = "File Name")]
    public string FileName { get; set; }

    public string Tags { get; set; }

    public IFormFile File { get; set; }

    public UploadableDocument() { }



    public UploadableDocument(DocumentRef document)
    {
        FileName = document.FileName;
        Tags = JsonConvert.SerializeObject(document.Tags);
    }

    public List<Tag>? ConvertTagToList()
    {
        List<Tag>? deserial = JsonConvert.DeserializeObject<List<Tag>>(Tags);
        if (deserial is null)
        {
            return null;
        }
        return deserial;
    }

    public byte[] ToBytes()
    {
        var reader = new BinaryReader(File.OpenReadStream());
        byte[] content = reader.ReadBytes((int)File.Length);
        reader.Dispose();
        return content;
    }


    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}