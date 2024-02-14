export interface BookingUpdateDTO {
    id?: string;
    checkIn: Date;
    checkOut: Date;
    userId: string;
    listingId: string;
    numberOfGuests?: number;
}
