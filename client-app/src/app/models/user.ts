export interface User {
    id : string,
    email : string,
    username: string;
    displayName: string;
    token: string;
    image?: string;
    roles: string[]; // Array of roles
}

export interface UserFormValues {
    email: string;
    password: string;
    displayName?: string;
    username?: string;
}