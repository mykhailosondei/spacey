import {HttpCustomClient} from "./HttpCustomClient";
import {UserDTO} from "../DTOs/User/UserDTO";
import {UserUpdateDTO} from "../DTOs/User/UserUpdateDTO";

export class UserService {
    http : HttpCustomClient;
    baseUrl;
    private constructor() {
        this.http = HttpCustomClient.getInstance();
        this.baseUrl = "/user";
    }
    
    private static instance: UserService;
    public static getInstance(): UserService {
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
    
    async getMany(ids:string[]) {
        let result = [] as UserDTO[];
        await Promise.all(ids.map(async (id) => {
            result.push(await this.get(id).then((response) => response.data));
        }));
        return result;
    }
    async getFromToken() {
        return await this.http.Get<UserDTO>(`${this.baseUrl}/fromToken`);
    }
    async update(user:UserUpdateDTO) {
        return await this.http.Put(`${this.baseUrl}`, user);
    }
    async delete(id:string) {
        return await this.http.Delete(`${this.baseUrl}/${id}`);
    }
}