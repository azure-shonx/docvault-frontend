using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace docvault_frontend.Models;

public class EditableDocument
{
    public string FileName { get; set; } // id, must not change!! But MVC needs set to work properly.

    [Display(Name = "File Name")]
    public string NewFileName { get; set; }
    public string Tags { get; set; }

    public EditableDocument() { }



    public EditableDocument(DocumentRef document)
    {
        FileName = document.FileName;
        NewFileName = document.FileName;
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

    public bool WasRenamed()
    {
        return !FileName.Equals(NewFileName);
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}