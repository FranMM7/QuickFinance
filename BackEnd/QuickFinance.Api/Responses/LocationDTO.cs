public class LocationDTO
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public string Name { get; set; }
    public int State { get; set; } = 1; //1=active, 0=inactive
    public string UserId { get; set; }
   
}