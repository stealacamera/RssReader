import { User } from "./api/user";

export interface LocalUser {
    user: User,
    isVerified: boolean,
    isLoggedIn: boolean,
}