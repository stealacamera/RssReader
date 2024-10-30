import { Tokens } from "../models/api/tokens";
import { LocalUser } from "../models/local-user";

export class LocalStorageKeys {
    static tokens: StorageKey<Tokens> = { key: 'TOKENS', value: null};
    static loggedinUser: StorageKey<LocalUser> = {key: 'CURR_USER', value: null};
}

export interface StorageKey<T> {
    key: string,
    value: T | null
}