export class Pagination {
    public pageNumber: number;
    public pageSize: number;
    public totalCount: number;
    public totalPages: number;
    public overridePageSize: boolean;

    public static noPagination(): Pagination {
        const pagination = new Pagination();
        pagination.overridePageSize = true;
        pagination.pageSize = -1;
        return pagination;
    }
}
