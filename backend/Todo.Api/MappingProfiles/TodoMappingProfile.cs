using AutoMapper;
using Todo.Api.Dtos;
using Todo.Api.Models;

namespace Todo.Api.MappingProfiles
{
    /// <summary>
    /// Auto Mapper mapping profile for To Do items between domain model (TodoItem) and the DTOs.
    /// </summary>
    public class TodoMappingProfile: Profile
    {
        /// <summary>
        /// Constructor to define the mapping configuration.
        /// </summary>
        public TodoMappingProfile()
        {
            CreateMap<TodoItemCreateDto, TodoItem>();
            CreateMap<TodoItemUpdateDto, TodoItem>();
            CreateMap<TodoItem, TodoItemDto>();
        }
    }
}
