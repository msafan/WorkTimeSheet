import { Pagination } from "./pagination";

export class PaginatedResults<T> {
    public pagination: Pagination;
    public items: T[];
}
