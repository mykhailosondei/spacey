import {HostUpdateDTO} from "../DTOs/Host/HostUpdateDTO";
import {HttpCustomClient} from "./HttpCustomClient";

export class HostService {
    http : HttpCustomClient;
    baseUrl;
    
    private constructor() {
        this.http = new HttpCustomClient();
        this.baseUrl = "/host";
    }
    
    private static instance: HostService;
    public getInstance(): HostService {
        if (!HostService.instance) {
            HostService.instance = new HostService();
        }
        return HostService.instance;
    }
    
    async getAll() {
        return await this.http.Get(`${this.baseUrl}`);
    }
    async getFromToken() {
        return await this.http.Get(`${this.baseUrl}/fromToken`);
    }
    async get(id:string) {
        return await this.http.Get(`${this.baseUrl}/${id}`);
    }
    async update(host:HostUpdateDTO) {
        return await this.http.Put(`${this.baseUrl}`, host);
    }
    async delete(id:string) {
        return await this.http.Delete(`${this.baseUrl}/${id}`);
    }
}