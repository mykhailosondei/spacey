export interface HostDTO {
    id: string;
    createdAt: Date;
    userId: string;
    numberOfListings: number;
    numberOfReviews: number;
    rating: number;
    listingsIds: string[];
    lastAccess: Date;
}