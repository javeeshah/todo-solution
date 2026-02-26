using Todo.Api.Dtos;
using Todo.Api.Models;
using System.Threading;

namespace Todo.Api.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoItem>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TodoItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<TodoItem> CreateAsync(TodoItemCreateDto dto, CancellationToken cancellationToken = default);
        Task<TodoItem?> UpdateAsync(TodoItemUpdateDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
