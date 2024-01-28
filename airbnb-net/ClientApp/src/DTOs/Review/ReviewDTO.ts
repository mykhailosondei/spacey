import {Ratings} from "../../values/Ratings";

export interface ReviewDTO {
    id: string;
    comment: string;
    ratings: Ratings;
    createdAt: Date;
    bookingId: string;
    userId: string;
}
