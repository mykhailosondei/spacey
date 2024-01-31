import {HttpCustomClient} from "./HttpCustomClient";
import {BookingDTO} from "../DTOs/Booking/BookingDTO";
import {BookingCreateDTO} from "../DTOs/Booking/BookingCreateDTO";
import {BookingUpdateDTO} from "../DTOs/Booking/BookingUpdateDTO";

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
            result.push(await this.get(id));
        }));
        return result;
    }
    async create(booking:BookingCreateDTO) {
        return await this.http.Post(`${this.baseUrl}`, booking);
    }
    async update(booking:BookingUpdateDTO) {
        return await this.http.Put(`${this.baseUrl}`, booking);
    }
    async delete(id:string) {
        return await this.http.Delete(`${this.baseUrl}/${id}`);
    }
}