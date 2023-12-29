import {UserDTO} from "./UserDTO";

interface AuthUser {
    user: UserDTO;
    token: string;
}

export default AuthUser;