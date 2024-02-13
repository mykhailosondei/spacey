import {ImageDTO} from "../Image/ImageDTO";

export interface UserUpdateDTO {
    name: string;
    phoneNumber: string;
    address: string;
    description: string;
    birthDate: Date;
    avatar: ImageDTO;
}
