import {ImageDTO} from "../Image/ImageDTO";
import {HostDTO} from "../Host/HostDTO";
import {Address} from "../../values/Address";

export interface UserDTO {
    id: string;
    name: string;
    email: string;
    phoneNumber: string;
    createdAt: Date;
    passwordHash: string;
    address: Address;
    birthdate: Date;
    description: string;
    avatar: ImageDTO;
    host: HostDTO;
    bookingsIds: string[];
    reviewsIds: string[];
    likedListingsIds: string[];
    lastAccess: Date;
}