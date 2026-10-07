public class GetArticlesRequest
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string SortBy { get; set; } = "createdAt";

    public string SortDirection { get; set; } = "desc";

    public Status? Status { get; set; }
}

public class CreateArticleRequest
{   
    public Status? Status { get; set; }
}



