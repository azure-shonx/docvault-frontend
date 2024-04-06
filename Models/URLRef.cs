using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace docvault_frontend.Models;

public class URLRef
{

    [Display(Name = "File Name")]
    public string FileName { get; }
    public string URL { get; }

    public URLRef(URLReply URLReply)
    {
        this.FileName = URLReply.FileName;
        this.URL = URLReply.URL;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}