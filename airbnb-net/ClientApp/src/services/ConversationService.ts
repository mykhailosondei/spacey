import {HttpCustomClient} from "./HttpCustomClient";
import {Conversation} from "../DTOs/Conversation/Conversation";

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
        return await this.http.Get<Conversation>(`${this.baseUrl}/${id}`);
    }
    
    async getUserConversations(userId: string) {
        return await this.http.Get<Conversation[]>(`${this.baseUrl}/request`, {params: {userId: userId}});
    }
    
    async sendMessage(conversationId: string, message: string) {
        return await this.http.Post(`${this.baseUrl}/${conversationId}`, {message});
    }
    
    async markAsRead(conversationId: string) {
        return await this.http.Post(`${this.baseUrl}/${conversationId}/read`, {});
    }
    
    async create(userId: string, hostId: string, bookingId: string) {
        return await this.http.Post(`${this.baseUrl}?userId=${userId}&hostId=${hostId}&bookingId=${bookingId}`, {});
    }

    async getHostConversations(hostId: string) {
        return await this.http.Get<Conversation[]>(`${this.baseUrl}/request`, {params: {hostId: hostId}});
    }

    async createByBooking(bookingId: string) {
        return await this.http.Post<string>(`${this.baseUrl}/${bookingId}`, {});
    }

    async getByBooking(bookingId: string) {
        return await this.http.Get<Conversation>(`${this.baseUrl}/booking/${bookingId}`);
    }
}