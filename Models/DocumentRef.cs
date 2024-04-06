using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace docvault_frontend.Models;

public class DocumentRef
{

    [Display(Name = "File Name")]
    public string FileName { get; set; }
    public List<Tag> Tags { get; set; }


    [JsonConstructor]
    public DocumentRef(string FileName, List<Tag>? tags)
    {
        this.FileName = FileName;
        if (tags is null)
            this.Tags = new List<Tag>(0);
        else
            this.Tags = tags;
    }

    public DocumentRef(EditableDocument editableDocument)
    {
        if (editableDocument.FileName is null)
        {
            throw new NullReferenceException("FileName");
        }
        if (!editableDocument.FileName.Equals(editableDocument.NewFileName))
        {
            throw new InvalidOperationException("File Names do not match.");
        }

        this.FileName = editableDocument.FileName;
        List<Tag>? desez = editableDocument.ConvertTagToList();
        if (desez is null)
            this.Tags = new List<Tag>(0);
        else
            this.Tags = desez;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}