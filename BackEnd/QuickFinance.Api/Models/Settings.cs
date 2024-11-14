using System.Text.Json;
using System.Text.Json.Serialization;

public class Settings
{
    public int Id { get; set; }
    public string SettingsName { get; set; }
    public string JsonValue { get; set; }  // Store settings as a JSON string
    public string UserId { get; set; }
    [JsonIgnore]
    public virtual ApplicationUser User { get; set; }

    // Optional: Helper methods to parse the JSON data
    public T GetValue<T>()
    {
        return JsonSerializer.Deserialize<T>(JsonValue);
    }

    public void SetValue<T>(T value)
    {
        JsonValue = JsonSerializer.Serialize(value);
    }
}
