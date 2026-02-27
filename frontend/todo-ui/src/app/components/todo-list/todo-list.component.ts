import { Component, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TodoService } from '../../services/todo.service';
import { TodoItemDto, TodoItemCreateDto, TodoItemUpdateDto } from '../../models/todo.model';

@Component({
  selector: 'todo-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './todo-list.component.html',
  styleUrl: './todo-list.component.css'
})
export class TodoListComponent {
  private svc = inject(TodoService);
  items = signal<TodoItemDto[]>([]);
  loading = signal(false);
  newTitle = signal('');
  editingId = signal<number | null>(null);
  editTitle = signal('');
  editComplete = signal(false);

  constructor() {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.svc.getAll().subscribe({
      next: data => {
        this.items.set(data);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  add(): void {
    const title = this.newTitle();
    if (!title?.trim()) return;
    const dto: TodoItemCreateDto = { title: title.trim(), isComplete: false };
    this.svc.create(dto).subscribe({
      next: created => {
        this.items.update(arr => [ ...arr, created ]);
        this.newTitle.set('');
      }
    });
  }

  startEdit(item: TodoItemDto): void {
    this.editingId.set(item.id);
    this.editTitle.set(item.title);
    this.editComplete.set(item.isComplete);
  }

  cancelEdit(): void {
    this.editingId.set(null);
  }

  saveEdit(): void {
    const id = this.editingId();
    if (id == null) return;
    const payload: TodoItemUpdateDto = {
      id,
      title: this.editTitle().trim(),
      isComplete: this.editComplete()
    };
    this.svc.update(id, payload).subscribe({
      next: updated => {
        this.items.update(arr => arr.map(i => i.id === updated.id ? updated : i));
        this.editingId.set(null);
      }
    });
  }

  toggle(item: TodoItemDto): void {
    const payload: TodoItemUpdateDto = {
      id: item.id,
      title: item.title,
      isComplete: !item.isComplete
    };
    this.svc.update(item.id, payload).subscribe({
      next: updated => {
        this.items.update(arr => arr.map(i => i.id === updated.id ? updated : i));
      }
    });
  }

  remove(id: number): void {
    this.svc.delete(id).subscribe({
      next: () => {
        this.items.update(arr => arr.filter(i => i.id !== id));
      }
    });
  }
}