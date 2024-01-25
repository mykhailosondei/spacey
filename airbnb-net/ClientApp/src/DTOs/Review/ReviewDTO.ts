import {Ratings} from "../../values/Ratings";

export interface ReviewDTO {
    id: string;
    comment: string;
    ratings: Ratings;
    bookingId: string;
    userId: string;
}
