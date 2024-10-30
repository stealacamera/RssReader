import { Tokens } from "./tokens";
import { User } from "./user";

export interface LoggedinUser {
    user: User,
    tokens: Tokens
}