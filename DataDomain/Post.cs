﻿namespace DataDomain;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public int BlogId { get; set; }
    public virtual Blog Blog { get; set; }
}
