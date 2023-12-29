export interface BookingDTO  {
    id: string;
    checkIn: Date;
    checkOut: Date;
    userId: string;
    listingId: string;
    review?: ReviewDTO | null;
    numberOfGuests?: number;
    totalPrice?: number;
    isCancelled?: boolean;
    lastAccess: Date;
}