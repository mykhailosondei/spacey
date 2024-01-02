import {HttpCustomClient} from "./HttpCustomClient";
import {UserDTO} from "../DTOs/User/UserDTO";
import {UserUpdateDTO} from "../DTOs/User/UserUpdateDTO";

export class UserService {
    http : HttpCustomClient;
    baseUrl;
    private constructor() {
        this.http = new HttpCustomClient();
        this.baseUrl = "/user";
    }
    
    private static instance: UserService;
    public getInstance(): UserService {
        if (!UserService.instance) {
            UserService.instance = new UserService();
        }
        return UserService.instance;
    }
    
    async getAll() {
        return await this.http.Get<UserDTO[]>(`${this.baseUrl}`);
    }
    async get(id:string) {
        return await this.http.Get<UserDTO>(`${this.baseUrl}/${id}`);
    }
    async update(user:UserUpdateDTO) {
        return await this.http.Put(`${this.baseUrl}`, user);
    }
    async delete(id:string) {
        return await this.http.Delete(`${this.baseUrl}/${id}`);
    }
}