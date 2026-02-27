import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TodoItemDto, TodoItemCreateDto, TodoItemUpdateDto } from '../models/todo.model';

@Injectable({
  providedIn: 'root'
})
export class TodoService {
  private base = '/api/todo';

  constructor(private http: HttpClient) {}

  getAll(): Observable<TodoItemDto[]> {
    return this.http.get<TodoItemDto[]>(this.base);
  }

  getById(id: number): Observable<TodoItemDto> {
    return this.http.get<TodoItemDto>(`${this.base}/${id}`);
  }

  create(payload: TodoItemCreateDto): Observable<TodoItemDto> {
    return this.http.post<TodoItemDto>(this.base, payload);
  }

  update(id: number, payload: TodoItemUpdateDto): Observable<TodoItemDto> {
    return this.http.put<TodoItemDto>(`${this.base}/${id}`, payload);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.base}/${id}`);
  }
}