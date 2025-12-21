namespace RoomReservationApiNet.Models
{
  public class DatabaseSettings
  {
    public required string Provider { get; set; }
    public required ConnectionStrings ConnectionStrings { get; set; }
  }

  public class ConnectionStrings
  {
    public required string SqlServer { get; set; }
    public required string MySQL { get; set; }
    public required string PostgreSQL { get; set; }
    public required string SQLite { get; set; }
    public required string MongoDB { get; set; }
  }
}
