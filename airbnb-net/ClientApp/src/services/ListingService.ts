import {HttpCustomClient} from "./HttpCustomClient";
import ListingDTO from "../DTOs/Listing/ListingDTO";
import {ListingCreateDTO} from "../DTOs/Listing/ListingCreateDTO";
import {ListingUpdateDTO} from "../DTOs/Listing/ListingUpdateDTO";

export class ListingService {
    http: HttpCustomClient;
    baseUrl:string;
    private constructor() {
        this.http = new HttpCustomClient();
        this.baseUrl = "/listing";
    }
    
    private static instance: ListingService;
    public getInstance(): ListingService {
        if (!ListingService.instance) {
            ListingService.instance = new ListingService();
        }
        return ListingService.instance;
    }
    
    async getAll() {
        return await this.http.Get(`${this.baseUrl}`);
    }
    async get(id:string) {
        return await this.http.Get(`${this.baseUrl}/${id}`);
    }
    async getByPropertyType(propertyType:string) {
        return await this.http.Get(`${this.baseUrl}/${propertyType}`);
    }
    async getByBoundingBox(x1:number, y1:number, x2:number, y2:number) {
        return await this.http.Get(`${this.baseUrl}/boundingbox?x1=${x1}&y1=${y1}&x2=${x2}&y2=${y2}`);
    }
    async getByAddress(city:string, country:string, street:string) {
        return await this.http.Get(`${this.baseUrl}/address?city=${city}&country=${country}&street=${street}`);
        
    }
    async create(listing:ListingCreateDTO) {
        return await this.http.Post(`${this.baseUrl}`, listing);
    }
    async update(listing:ListingUpdateDTO) {
        return await this.http.Put(`${this.baseUrl}`, listing);
    }
    async delete(id:string) {
        return await this.http.Delete(`${this.baseUrl}/${id}`);
    }
    async like(id:string) {
        return await this.http.Post(`${this.baseUrl}/${id}/like`);
    }
    async unlike(id:string) {
        return await this.http.Post(`${this.baseUrl}/${id}/unlike`);
    }
}