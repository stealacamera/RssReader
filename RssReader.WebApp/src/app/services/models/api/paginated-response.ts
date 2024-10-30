export interface PaginatedResponse<TCursor, TValues> {
    nextCursor: TCursor,
    items: TValues
}