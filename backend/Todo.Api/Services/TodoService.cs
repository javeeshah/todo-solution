using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Todo.Api.Dtos;
using Todo.Api.Models;
using Todo.Api.Repositories;

namespace Todo.Api.Services
{
    /// <summary>
    /// To Do Service class to implement the business logic for managing To Do items, 
    /// including creating, retrieving, updating, and deleting items. 
    /// </summary>
    public class TodoService : ITodoService
    {        
        private readonly TodoContext _context;
        private readonly IMapper  _mapper;
        private readonly ILogger<TodoService> _logger;

        /// <summary>
        /// Constructor to initialize the TodoService with a TodoContext instance for database operations
        /// </summary>
        /// <param name="context"></param>
        public TodoService(TodoContext context, IMapper mapper, ILogger<TodoService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all To Do items from the data store and returns them as an enumerable collection.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all To Do items from the database.");
            return _context.TodoItems.ToList();
        }

        /// <summary>
        /// Retrieves a TodoItem with the specified unique identifier from the data store.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving To Do item with ID {Id} from the database.", id);
            return await _context.TodoItems.FindAsync(id);
        }
        
        /// <summary>
        /// Create method to add a new To Do item to the database using the provided DTO (Data Transfer Object) for item creation.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<TodoItem> CreateAsync(TodoItemCreateDto dto)
        {
            _logger.LogInformation("Creating a new To Do item with title: {Title}", dto.Title);
            var item = _mapper.Map<TodoItem>(dto);
            _context.TodoItems.Add(item);
            try
            {
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while creating To Do item");
                // rethrow so global middleware handles the response; preserve stack trace
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating To Do item");
                throw;
            }
        }

        /// <summary>
        /// Update an existing To Do item in the database based on the provided DTO, which includes the item's unique identifier and updated properties.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<TodoItem?> UpdateAsync(TodoItemUpdateDto dto)
        {
            _logger.LogInformation("Updating To Do item with ID {Id}", dto.Id);
            var existingItem = await _context.TodoItems.FindAsync(dto.Id);
            if (existingItem == null)
            {
                _logger.LogWarning("To Do item with ID {Id} not found for update.", dto.Id);
                return null;
            }
            _mapper.Map(dto, existingItem);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("To Do item with ID {Id} updated successfully.", dto.Id);
                return existingItem;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while updating To Do item with ID {Id}", dto.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating To Do item with ID {Id}", dto.Id);
                throw;
            }
        }

        /// <summary>
        /// Delete a To Do item from the database based on its unique identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting To Do item with ID {Id}", id);
            var existing = await _context.TodoItems.FindAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("To Do item with ID {Id} not found for deletion.", id);
                return false;
            }
            _context.TodoItems.Remove(existing);
            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("To Do item with ID {Id} deleted successfully.", id);
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while deleting To Do item with ID {Id}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while deleting To Do item with ID {Id}", id);
                throw;
            }
        }
    }
}
