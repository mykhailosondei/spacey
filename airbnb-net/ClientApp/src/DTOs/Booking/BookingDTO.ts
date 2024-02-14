import {ReviewDTO} from "../Review/ReviewDTO";
import {BookingStatus} from "../../values/BookingStatus";

export interface BookingDTO  {
    id: string;
    checkIn: Date;
    checkOut: Date;
    userId: string;
    listingId: string;
    review?: ReviewDTO | null;
    numberOfGuests?: number;
    totalPrice?: number;
    status: BookingStatus;
    lastAccess: Date;
}