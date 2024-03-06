import {Message} from "../Message/Message";

export interface Conversation {
    id: string;
    userId: string;
    hostId: string;
    messages: Message[];
    bookingId: string;
    createdAt: Date;
    isRead: boolean;
}