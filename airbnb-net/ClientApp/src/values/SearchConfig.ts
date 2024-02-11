export interface SearchConfig {
    propertyType?: string;
    place?: string;
    checkIn?: string;
    checkOut?: string;
    guests?: string;
    boundingBox: { x1: number, y1: number, x2: number, y2: number }
}