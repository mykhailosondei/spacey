import {HttpCustomClient} from "./HttpCustomClient";
import {BookingDTO} from "../DTOs/Booking/BookingDTO";
import {BookingCreateDTO} from "../DTOs/Booking/BookingCreateDTO";
import {BookingUpdateDTO} from "../DTOs/Booking/BookingUpdateDTO";
import {BookingStatus} from "../values/BookingStatus";

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
        return await this.http.Get<BookingDTO[]>(`${this.baseUrl}`);
    }
    async get(id:string) {
        return await this.http.Get<BookingDTO>(`${this.baseUrl}/${id}`);
    }
    async getMany(ids:string[]) {
        let result = [] as BookingDTO[];
        await Promise.all(ids.map(async (id) => {
            result.push(await this.get(id).then((response) => response.data));
        }));
        return result;
    }
    
    async getFromTokenByStatus(status:BookingStatus) {
        return await this.http.Get<BookingDTO[]>(`${this.baseUrl}/fromTokenByStatus?status=${status}`);
    }
    
    async create(booking:BookingCreateDTO) {
        return await this.http.Post(`${this.baseUrl}`, booking);
    }
    async update(id: string, booking:BookingUpdateDTO) {
        return await this.http.Put(`${this.baseUrl}/${id}`, booking);
    }
    async cancel(id:string) {
        return await this.http.Post(`${this.baseUrl}/${id}/cancel`);
    }
    async delete(id:string) {
        return await this.http.Delete(`${this.baseUrl}/${id}`);
    }
}