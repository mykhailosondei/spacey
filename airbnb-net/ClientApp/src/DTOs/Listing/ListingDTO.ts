import {ImageDTO} from "../Image/ImageDTO";
import {HostDTO} from "../Host/HostDTO";
import {PropertyType} from "../../values/PropertyType";
import {Address} from "../../values/Address";
import {Ratings} from "../../values/Ratings";



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
    ratings: Ratings[];
    bookingsIds: string[];
    amenities: string[];
    likedUsersIds: string[];
}

export default ListingDTO;