import {ReviewUpdateDTO} from "../DTOs/Review/ReviewUpdateDTO";
import {ReviewCreateDTO} from "../DTOs/Review/ReviewCreateDTO";
import {HttpCustomClient} from "./HttpCustomClient";
import {ReviewDTO} from "../DTOs/Review/ReviewDTO";

export class ReviewService{
    http: HttpCustomClient;
    baseUrl:string;
    private constructor() {
        this.http = HttpCustomClient.getInstance();
        this.baseUrl = "/review";
    }
    
    private static instance: ReviewService;
    public static getInstance(): ReviewService {
        if (!ReviewService.instance) {
            ReviewService.instance = new ReviewService();
        }
        return ReviewService.instance;
    }
    
    async getAll() {
        return await this.http.Get<ReviewDTO[]>(`${this.baseUrl}`);
    }
    async get(id:string) {
        return await this.http.Get<ReviewDTO>(`${this.baseUrl}/${id}`);
    }
    async getMany(ids:string[]) {
        let result = [] as ReviewDTO[];
        await Promise.all(ids.map(async (id) => {
            result.push(await this.get(id));
        }));
        return result;
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