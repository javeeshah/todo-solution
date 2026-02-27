export interface TodoItemDto {
  id: number;
  title: string;
  isComplete: boolean;
}

export interface TodoItemCreateDto {
  title: string;
  isComplete?: boolean;
}

export interface TodoItemUpdateDto {
  id: number;
  title: string;
  isComplete: boolean;
}