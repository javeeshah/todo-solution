namespace Todo.Api.Dtos
{
    /// <summary>
    /// To do item do main class to transfer the data between the API and the clients.
    /// </summary>
    public class TodoItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
