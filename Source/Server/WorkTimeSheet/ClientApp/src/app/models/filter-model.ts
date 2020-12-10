import { Pagination } from "./pagination";

export abstract class FilterModel extends Pagination {

    public noPagination(): void {
        this.overridePageSize = true;
        this.pageSize = -1;
    }

}
