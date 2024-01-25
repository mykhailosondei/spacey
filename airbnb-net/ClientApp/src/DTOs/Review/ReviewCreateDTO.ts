import {Ratings} from "../../values/Ratings";

export interface ReviewCreateDTO {
    id?: string;
    comment: string;
    ratings: Ratings;
    bookingId: string;
    userId: string;
}