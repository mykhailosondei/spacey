
import {ImageDTO} from "../Image/ImageDTO";
import {PropertyType} from "../../values/PropertyType";
import {Address} from "node:cluster";

export interface ListingCreateDTO {
    id?: string;
    title: string;
    description: string;
    address: Address;
    propertyType: PropertyType;
    pricePerNight: number;
    numberOfRooms: number;
    numberOfBathrooms: number;
    numberOfGuests: number;
    hostId: string;
    imagesUrls: ImageDTO[];
    amenities: string[];
}
