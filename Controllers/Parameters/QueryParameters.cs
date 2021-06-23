using Microsoft.AspNetCore.Mvc.ModelBinding;

public class QueryParameters {
    [BindRequired]
    public string ConnectionId { get; set; }
}