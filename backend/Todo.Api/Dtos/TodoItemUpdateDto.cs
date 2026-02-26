namespace Todo.Api.Dtos
{
    /// <summary>
    /// Dto class for To do Item update
    /// </summary>
    public class TodoItemUpdateDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
    }
}
