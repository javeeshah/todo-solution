using Todo.Api.Dtos;
using Todo.Api.Models;

namespace Todo.Api.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoItem>> GetAllAsync();
        Task<TodoItem?> GetByIdAsync(int id);
        Task<TodoItem> CreateAsync(TodoItemCreateDto dto);
        Task<TodoItem?> UpdateAsync(TodoItemUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
