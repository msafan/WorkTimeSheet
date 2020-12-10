import { FilterModel } from "./filter-model";

export class WorkLogFilterModel extends FilterModel {
    public name: string;
    public projectName: string;
    public startDate: Date;
    public userId: number;
    public projectId: number;
}
