import {HttpCustomClient} from "./HttpCustomClient";

export default class MessageService {
    http: HttpCustomClient;
    baseUrl: string;

    private constructor() {
        this.http = HttpCustomClient.getInstance();
        this.baseUrl = "/Message";
    }

    private static instance: MessageService;

    public static getInstance(): MessageService {
        if (!MessageService.instance) {
            MessageService.instance = new MessageService();
        }
        return MessageService.instance;
    }

    async get(id: string) {
        return await this.http.Get(`${this.baseUrl}/${id}`);
    }

    async sendMessage(conversationId: string, message: string) {
        return await this.http.Post(`${this.baseUrl}/${conversationId}`, {MessageContent: message});
    }
}