import {HttpCustomClient} from "./HttpCustomClient";

export default class ConversationService {
    http: HttpCustomClient;
    baseUrl: string;
    
    private constructor() {
        this.http = HttpCustomClient.getInstance();
        this.baseUrl = "/conversation";
    }
    
    private static instance: ConversationService;
    
    public static getInstance(): ConversationService {
        if (!ConversationService.instance) {
            ConversationService.instance = new ConversationService();
        }
        return ConversationService.instance;
    }
    
    async get(id: string) {
        return await this.http.Get(`${this.baseUrl}/${id}`);
    }
    
    async sendMessage(conversationId: string, message: string) {
        return await this.http.Post(`${this.baseUrl}/${conversationId}`, {message});
    }
    
    async create(userId: string, hostId: string, bookingId: string) {
        return await this.http.Post(`${this.baseUrl}?userId=${userId}&hostId=${hostId}&bookingId=${bookingId}`, {});
    }
}