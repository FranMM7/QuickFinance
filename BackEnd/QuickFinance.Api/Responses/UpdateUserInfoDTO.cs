using System.Text.Json.Serialization;

public class UpdateUserInfoDto
{
    [JsonPropertyName("userName")]
    public string UserName { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("anonymousData")]
    public bool AnonymousData { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("middleName")]
    public string? MiddleName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }
}
