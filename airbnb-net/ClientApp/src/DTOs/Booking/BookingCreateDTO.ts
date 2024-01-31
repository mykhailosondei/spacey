export interface BookingCreateDTO {
    id?: string;
    checkIn: Date;
    checkOut: Date;
    listingId: string;
    numberOfGuests?: number;
}