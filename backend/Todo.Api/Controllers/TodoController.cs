using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Todo.Api.Dtos;
using Todo.Api.Services;

namespace Todo.Api.Controllers
{
    /// <summary>
    /// Provides HTTP endpoints for managing todo items, including retrieval and creation operations.       
    /// </summary>
    /// <remarks>This controller interacts with the TodoService to perform CRUD operations on todo items. It
    /// is decorated with the ApiController attribute, enabling automatic model validation and response
    /// formatting.</remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Controller constructor to initialize the TodoController with a logger, 
        /// a todo service, and an AutoMapper instance for mapping between domain models and DTOs.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="todoService"></param>
        public TodoController(ITodoService todoService, IMapper mapper)
        {
            _todoService = todoService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all todo items asynchronously from the data store and returns them as an enumerable collection. 
        /// This endpoint responds to HTTP GET requests at the route "api/todo".
        /// </summary>
        /// <returns>List of TodoItemDto</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<TodoItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var items = await _todoService.GetAllAsync();
            return Ok(_mapper.Map<List<TodoItemDto>>(items));
        }

        /// <summary>
        /// Retrieves a todo item asynchronously by its unique identifier from the data store.
        /// This endpoint responds to HTTP GET requests at the route "api/todo/{id}", where {id} is an integer representing the todo item's ID. If the item is found, it returns an HTTP 200 OK response with the item; if not found, it returns an HTTP 404 Not Found response.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TodoItemDto</returns>
        [HttpGet("{id:int}")]
        [ProducesErrorResponseType(typeof(NotFoundResult))]
        [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _todoService.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(_mapper.Map<TodoItemDto>(item));
        }

        /// <summary>
        /// Creates a new todo item asynchronously in the data store using the provided DTO for item creation. 
        /// This endpoint responds to HTTP POST requests at the route "api/todo". 
        /// If the item is successfully created, it returns an HTTP 201 Created response with the created item; 
        /// if there is a validation error, it returns an HTTP 400 Bad Request response.
        /// </summary>
        /// <param name="dtoItem"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TodoItemCreateDto dtoItem)
        {
            // ApiController + FluentValidation produce ValidationProblemDetails automatically,
            // so manual ModelState checks are not required here.

            var createdItem = await _todoService.CreateAsync(dtoItem);
            var resultDto = _mapper.Map<TodoItemDto>(createdItem);
            return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] TodoItemUpdateDto dto)
        {
            if (id != dto.Id) 
            {
                var problem = new ProblemDetails
                {
                    Title = "ID mismatch",
                    Detail = "The id in the route does not match the id in the request body.",
                    Status = StatusCodes.Status400BadRequest
                };
                return BadRequest(problem);
            }

            var updated = await _todoService.UpdateAsync(dto);
            return updated == null ? NotFound() : Ok(_mapper.Map<TodoItemDto>(updated));
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _todoService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
