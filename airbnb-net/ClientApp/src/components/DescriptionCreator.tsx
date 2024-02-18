import {ListingCreateDTO} from "../DTOs/Listing/ListingCreateDTO";
import {useState} from "react";
import {InputArea} from "./InputArea";

export const DescriptionCreator = (props: {
    setListing: (value: (((prevState: ListingCreateDTO) => ListingCreateDTO) | ListingCreateDTO)) => void,
    listing: ListingCreateDTO
}) => {
    
    const [description, setDescription] = useState<string>(props.listing.description ?? "");
    
    const validateDescription = (value: string) => {
        return value.length < 500;
    }
    
    const updateDescription = (value: string) => {
        value.trim();
        setDescription(value);
        if (!validateDescription(value)) return;
        props.setListing((prevState: ListingCreateDTO) => {
            return {...prevState, description: value};
        });
    }
    
    return <div className="description-creator">
        <InputArea value={description} onChange={(e) => updateDescription(e.target.value)} isValid={validateDescription}></InputArea>
    </div>
};