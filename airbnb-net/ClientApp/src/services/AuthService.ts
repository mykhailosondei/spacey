import {HttpCustomClient} from "./HttpCustomClient";
import {RegisterUserDTO} from "../DTOs/User/RegisterUserDTO";
import {LoginUserDTO} from "../DTOs/User/LoginUserDTO";

export class AuthService {
    http: HttpCustomClient;
    baseUrl: string;
    private constructor() {
        this.http = new HttpCustomClient();
        this.baseUrl = "/auth";
    }
    
    private static instance: AuthService;
    public static getInstance(): AuthService {
        if (!AuthService.instance) {
            AuthService.instance = new AuthService();
        }
        return AuthService.instance;
    }
    
    
    async register(user:RegisterUserDTO) {
        return await this.http.Post<RegisterUserDTO>(`${this.baseUrl}/register`, user);
    }
    async login(user:LoginUserDTO) {
        return await this.http.Post<LoginUserDTO>(`${this.baseUrl}/login`, user);
    }
    async isRegistered(email: string) {
        return await this.http.Post<boolean>(`${this.baseUrl}/isEmailTaken`, email);
    }
}