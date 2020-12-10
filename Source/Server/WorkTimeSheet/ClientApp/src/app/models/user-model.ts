import { UserRoleModel } from "./user-role-model";

export class UserModel {
    public id: number;
    public organizaionId: number;
    public name: string;
    public email: string;
    public userRoles: UserRoleModel[];
}
