public class SettingsDTO
{
    public int Id { get; set; }
    public string SettingsName { get; set; }
    public string JsonValue { get; set; }  // Store settings as a JSON string
    public string UserId { get; set; }
}