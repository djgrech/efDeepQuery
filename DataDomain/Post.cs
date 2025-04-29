namespace DataDomain;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}
