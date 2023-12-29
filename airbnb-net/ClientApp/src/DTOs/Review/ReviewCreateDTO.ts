export interface ReviewCreateDTO {
    id?: string;
    comment: string;
    ratings: number[];
    bookingId: string;
    userId: string;
}