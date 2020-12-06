import { UserRole } from "./user-role";

export class AuthorizedUser {
    public userId: number;
    public name: string;
    public accessToken: string;
    public refreshToken: string;
    public roles: UserRole[];
}
