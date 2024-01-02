import {ReviewUpdateDTO} from "../DTOs/Review/ReviewUpdateDTO";
import {ReviewCreateDTO} from "../DTOs/Review/ReviewCreateDTO";
import {HttpCustomClient} from "./HttpCustomClient";

export class ReviewService{
    http: HttpCustomClient;
    baseUrl:string;
    private constructor() {
        this.http = new HttpCustomClient();
        this.baseUrl = "/review";
    }
    
    private static instance: ReviewService;
    public getInstance(): ReviewService {
        if (!ReviewService.instance) {
            ReviewService.instance = new ReviewService();
        }
        return ReviewService.instance;
    }
    
    async getAll() {
        return await this.http.Get(`${this.baseUrl}`);
    }
    async get(id:string) {
        return await this.http.Get(`${this.baseUrl}/${id}`);
    }
    async create(review:ReviewCreateDTO) {
        return await this.http.Post(`${this.baseUrl}`, review);
    }
    async update(review:ReviewUpdateDTO) {
        return await this.http.Put(`${this.baseUrl}`, review);
    }
    async delete(id:string) {
        return await this.http.Delete(`${this.baseUrl}/${id}`);
    }
}