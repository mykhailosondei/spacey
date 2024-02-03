import {HttpCustomClient} from "./HttpCustomClient";
import {RegisterUserDTO} from "../DTOs/User/RegisterUserDTO";
import {LoginUserDTO} from "../DTOs/User/LoginUserDTO";
import AuthUser from "../DTOs/User/AuthUser";

export class AuthService {
    http: HttpCustomClient;
    baseUrl: string;
    private constructor() {
        this.http = HttpCustomClient.getInstance();
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
        const response = await this.http.Post<AuthUser>(`${this.baseUrl}/register`, user);
        if(response){
            this.http.SetBearerToken(response.data.token);
            console.log(response.data.token);
        }
        return response;
    }
    
    async switchToHost() {
        const response = await this.http.Post<AuthUser>(`${this.baseUrl}/switch-role-to-host/true`);
        if(response){
            this.http.SetBearerToken(response.data.token);
        }
        return response;
    }
    
    async switchToUser() {
        const response = await this.http.Post<AuthUser>(`${this.baseUrl}/switch-role-to-host/false`);
        if(response){
            this.http.SetBearerToken(response.data.token);
        }
        return response;
    }
    
    async login(user:LoginUserDTO) {
        const response = await this.http.Post<AuthUser>(`${this.baseUrl}/login`, user);
        if(response){
            this.http.SetBearerToken(response.data.token);
        }
        return response;
    }
    
    async isRegistered(email: string) {
        return await this.http.Post<boolean>(`${this.baseUrl}/isEmailTaken`, email);
    }

    async isInRole(role: string) {
        return await this.http.Get<boolean>(`${this.baseUrl}/is-in-role/${role}`);
    }
    
    logout() {
        this.http.RemoveBearerToken();
    }
}