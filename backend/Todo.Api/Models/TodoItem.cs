namespace Todo.Api.Models
{
    /// <summary>
    /// To Do Item domain class to persist the data
    /// </summary>
    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }

    }
}
