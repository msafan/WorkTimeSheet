import { PaginatedResults } from "./paginated-results";
import { WorkLogModel } from "./work-log-model";

export class WorkLogReport {
    public totalTime: any;
    public paginatedResults: PaginatedResults<WorkLogModel>;
}
