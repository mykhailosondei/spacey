import {Ratings} from "../../values/Ratings";

export interface ReviewUpdateDTO {
    id?: string;
    comment: string;
    ratings: Ratings;
}
