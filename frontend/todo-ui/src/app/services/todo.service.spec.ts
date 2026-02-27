import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TodoService } from './todo.service';
import { TodoItemDto, TodoItemCreateDto, TodoItemUpdateDto } from '../models/todo.model';

describe('TodoService', () => {
  let service: TodoService;
  let httpMock: HttpTestingController;
  const baseUrl = '/api/todo';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [TodoService]
    });
    service = TestBed.inject(TodoService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getAll', () => {
    it('should fetch all todos', () => {
      const mockTodos: TodoItemDto[] = [
        { id: 1, title: 'Test Todo 1', isComplete: false },
        { id: 2, title: 'Test Todo 2', isComplete: true }
      ];

      service.getAll().subscribe(todos => {
        expect(todos).toEqual(mockTodos);
        expect(todos.length).toBe(2);
      });

      const req = httpMock.expectOne(baseUrl);
      expect(req.request.method).toBe('GET');
      req.flush(mockTodos);
    });

    it('should return empty array when no todos exist', () => {
      service.getAll().subscribe(todos => {
        expect(todos).toEqual([]);
        expect(todos.length).toBe(0);
      });

      const req = httpMock.expectOne(baseUrl);
      req.flush([]);
    });
  });

  describe('getById', () => {
    it('should fetch a single todo by id', () => {
      const mockTodo: TodoItemDto = { id: 1, title: 'Test Todo', isComplete: false };

      service.getById(1).subscribe(todo => {
        expect(todo).toEqual(mockTodo);
        expect(todo.id).toBe(1);
      });

      const req = httpMock.expectOne(`${baseUrl}/1`);
      expect(req.request.method).toBe('GET');
      req.flush(mockTodo);
    });

    it('should handle different todo ids', () => {
      const mockTodo: TodoItemDto = { id: 42, title: 'Another Todo', isComplete: true };

      service.getById(42).subscribe(todo => {
        expect(todo.id).toBe(42);
      });

      const req = httpMock.expectOne(`${baseUrl}/42`);
      req.flush(mockTodo);
    });
  });

  describe('create', () => {
    it('should create a new todo', () => {
      const createDto: TodoItemCreateDto = { title: 'New Todo', isComplete: false };
      const mockResponse: TodoItemDto = { id: 3, title: 'New Todo', isComplete: false };

      service.create(createDto).subscribe(todo => {
        expect(todo).toEqual(mockResponse);
        expect(todo.id).toBe(3);
      });

      const req = httpMock.expectOne(baseUrl);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(createDto);
      req.flush(mockResponse);
    });

    it('should create a completed todo', () => {
      const createDto: TodoItemCreateDto = { title: 'Completed Todo', isComplete: true };
      const mockResponse: TodoItemDto = { id: 4, title: 'Completed Todo', isComplete: true };

      service.create(createDto).subscribe(todo => {
        expect(todo.isComplete).toBe(true);
      });

      const req = httpMock.expectOne(baseUrl);
      req.flush(mockResponse);
    });
  });

  describe('update', () => {
    it('should update an existing todo', () => {
      const updateDto: TodoItemUpdateDto = { id: 1, title: 'Updated Todo', isComplete: true };
      const mockResponse: TodoItemDto = { id: 1, title: 'Updated Todo', isComplete: true };

      service.update(1, updateDto).subscribe(todo => {
        expect(todo).toEqual(mockResponse);
        expect(todo.title).toBe('Updated Todo');
      });

      const req = httpMock.expectOne(`${baseUrl}/1`);
      expect(req.request.method).toBe('PUT');
      expect(req.request.body).toEqual(updateDto);
      req.flush(mockResponse);
    });

    it('should toggle todo completion status', () => {
      const updateDto: TodoItemUpdateDto = { id: 2, title: 'Test Todo', isComplete: false };
      const mockResponse: TodoItemDto = { id: 2, title: 'Test Todo', isComplete: false };

      service.update(2, updateDto).subscribe(todo => {
        expect(todo.isComplete).toBe(false);
      });

      const req = httpMock.expectOne(`${baseUrl}/2`);
      req.flush(mockResponse);
    });
  });

  describe('delete', () => {
    it('should delete a todo', () => {
      service.delete(1).subscribe(response => {
        expect(response).toBeUndefined();
      });

      const req = httpMock.expectOne(`${baseUrl}/1`);
      expect(req.request.method).toBe('DELETE');
      req.flush(null);
    });

    it('should handle deletion of different todo ids', () => {
      service.delete(99).subscribe();

      const req = httpMock.expectOne(`${baseUrl}/99`);
      expect(req.request.method).toBe('DELETE');
      req.flush(null);
    });
  });  
});
