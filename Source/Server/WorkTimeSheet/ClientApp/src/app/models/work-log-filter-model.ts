import { FilterModel } from "./filter-model";

export class WorkLogFilterModel extends FilterModel {
    public names: string[];
    public projectNames: string[];
    public startDate: Date;
    public userIds: number[];
    public projectIds: number[];
}
