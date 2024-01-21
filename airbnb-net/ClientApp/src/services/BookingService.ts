import {HttpCustomClient} from "./HttpCustomClient";

export class BookingService {
    //ts
    http : HttpCustomClient;
    baseUrl;
    private constructor() {
        this.http = HttpCustomClient.getInstance();
        this.baseUrl = "/booking";
    }
    
    public static instance: BookingService;
    public static getInstance(): BookingService {
        if (!BookingService.instance) {
            BookingService.instance = new BookingService();
        }
        return BookingService.instance;
    }
    
    async getAll() {
        return await this.http.Get(`${this.baseUrl}`);
    }
    async get(id:string) {
        return await this.http.Get(`${this.baseUrl}/${id}`);
    }
    async create(booking:any) {
        return await this.http.Post(`${this.baseUrl}`, booking);
    }
    async update(booking:any) {
        return await this.http.Put(`${this.baseUrl}`, booking);
    }
    async delete(id:string) {
        return await this.http.Delete(`${this.baseUrl}/${id}`);
    }
}