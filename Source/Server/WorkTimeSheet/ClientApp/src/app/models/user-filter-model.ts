import { FilterModel } from "./filter-model";

export class UserFilterModel extends FilterModel {
    public name: string;
    public email: string;
    public roles: string[];
}
