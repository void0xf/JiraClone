export interface ApiResponse<T> {
  isSuccess: Boolean;
  data: T;
  requestId: string;
}
