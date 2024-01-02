import {ImageDTO} from "../Image/ImageDTO";
import {HostDTO} from "../Host/HostDTO";
import {PropertyType} from "../../values/PropertyType";
import {Address} from "../../values/Address";



export interface ListingDTO {
    id: string;
    title: string;
    description: string;
    address: Address;
    propertyType: PropertyType;
    latitude: number;
    longitude: number;
    pricePerNight: number;
    numberOfRooms: number;
    numberOfBathrooms: number;
    numberOfGuests: number;
    imagesUrls: ImageDTO[];
    host: HostDTO;
    bookingsIds: string[];
    amenities: string[];
    likedUsersIds: string[];
}

export default ListingDTO;