import { FilterModel } from "./filter-model";

export class WorkLogFilterModel extends FilterModel {
    public names: string[];
    public projectNames: string[];
    public startDate: string | null;
    public endDate: string | null;
    public userIds: number[];
    public projectIds: number[];
}
