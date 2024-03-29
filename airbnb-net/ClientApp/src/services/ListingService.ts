import {HttpCustomClient} from "./HttpCustomClient";
import ListingDTO from "../DTOs/Listing/ListingDTO";
import {ListingCreateDTO} from "../DTOs/Listing/ListingCreateDTO";
import {ListingUpdateDTO} from "../DTOs/Listing/ListingUpdateDTO";
import {AxiosRequestConfig} from "axios";
import {SearchConfig} from "../values/SearchConfig";
import {ListingFilter} from "../components/HostingListingsSection";
import {types} from "sass";
import Number = types.Number;

export class ListingService {
    http: HttpCustomClient;
    baseUrl:string;
    private constructor() {
        this.http = HttpCustomClient.getInstance();
        this.baseUrl = "/listing";
    }
    
    private static instance: ListingService;
    
    public static getInstance(): ListingService {
        if (!ListingService.instance) {
            ListingService.instance = new ListingService();
        }
        return ListingService.instance;
    }
    
    async getAll(from?: number, to?:number, config:AxiosRequestConfig = {}) {
        const fromParam = from !== undefined ? `?from=${from}` : ""
        const toParam = to !== undefined ? `&to=${to}` : ""
        return await this.http.Get<ListingDTO[]>(`${this.baseUrl}/${fromParam}${toParam}`, config);
    }
    async get(id:string, config:AxiosRequestConfig = {}) {
        return await this.http.Get<ListingDTO>(`${this.baseUrl}/${id}`, config);
    }
    async getUnavailableDates(id:string) {
        return await this.http.Get<string[]>(`${this.baseUrl}/${id}/unavailableDates`);
    }
    async getByPropertyType(propertyType:string) {
        return await this.http.Get<ListingDTO[]>(`${this.baseUrl}/propertyType/${propertyType}`);
    }
    async getByBoundingBox(x1:number, y1:number, x2:number, y2:number) {
        return await this.http.Get<ListingDTO[]>(`${this.baseUrl}/boundingbox?x1=${x1}&y1=${y1}&x2=${x2}&y2=${y2}`);
    }
    async getByAddress(city:string, country:string, street:string) {
        return await this.http.Get<ListingDTO[]>(`${this.baseUrl}/address?city=${city}&country=${country}&street=${street}`);
        
    }
    
    async getDistance(id:string,latitude:number, longitude:number) {
        return await this.http.Get<number>(`${this.baseUrl}/${id}/distance?latitude=${latitude}&longitude=${longitude}`);
    }
    
    async create(listing:ListingCreateDTO) {
        return await this.http.Post(`${this.baseUrl}`, listing);
    }
    async update(id: string, listing:ListingUpdateDTO) {
        return await this.http.Put(`${this.baseUrl}/${id}`, listing);
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

    /*async getBySearch(place: string, checkIn: string, checkOut: string, guests: number, propertyType: string) {
        return await this.http.Get<ListingDTO[]>(`${this.baseUrl}/search?place=${place}&checkIn=${checkIn}&checkOut=${checkOut}&guests=${guests}&propertyType=${propertyType}`);
    }*/
    
    async getBySearch(config: SearchConfig, from?: number, to?:number) {
        const fromParam = from !== undefined ? `?from=${from}` : ""
        const toParam = to !== undefined ? `&to=${to}` : ""
       return await this.http.Get<ListingDTO[]>(`${this.baseUrl}/search${fromParam}${toParam}`, {params: config});
    }

    async getListingsByFilterFromToken(filterState: ListingFilter) {
        return await this.http.Get<ListingDTO[]>(`${this.baseUrl}/ofHost/filter`, {params: filterState});
    }
}