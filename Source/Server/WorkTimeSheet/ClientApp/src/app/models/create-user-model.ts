export class CreateUserModel {
    public name: string;
    public email: string;
    public password: string;
    public roleIds: number[];
    public confirmPassword: string;
}
