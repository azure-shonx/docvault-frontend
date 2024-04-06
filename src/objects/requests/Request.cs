using Newtonsoft.Json;

public abstract class Request
{

    public string FileName { get; }

    public Request(string FileName)
    {
        this.FileName = FileName;
    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}