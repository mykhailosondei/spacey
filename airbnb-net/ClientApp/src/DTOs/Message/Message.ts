export interface Message {
    id: string;
    userId?: string | null;
    hostId?: string | null;
    content: string;
    createdAt: Date;
}