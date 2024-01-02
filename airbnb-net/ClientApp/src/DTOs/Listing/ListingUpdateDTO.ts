import 
{ImageDTO} from "../Image/ImageDTO";
import {Address} from "../../values/Address";
import {PropertyType} from "../../values/PropertyType";

export interface ListingUpdateDTO {
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
