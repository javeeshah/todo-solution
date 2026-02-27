namespace Todo.Api.Dtos
{
    /// <summary>
    /// DTO class for To do Item Creation
    /// </summary>
    public class TodoItemCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
