export interface BookingCreateDTO {
    id?: string;
    checkIn: Date;
    checkOut: Date;
    userId: string;
    listingId: string;
    numberOfGuests?: number;
}