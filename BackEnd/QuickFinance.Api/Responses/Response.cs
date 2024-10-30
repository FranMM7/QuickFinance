using System.ComponentModel.DataAnnotations;
public class Response<T>
{
    public T Data { get; set; }
    public bool Succeeded { get; set; } = true;
    public string Message { get; set; }
    public List<string> Errors { get; set; }
}