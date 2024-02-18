import React from "react";
import {ListingCreateDTO} from "../DTOs/Listing/ListingCreateDTO";
import {InputArea} from "./InputArea";

interface TitleCreatorParams {
    setListing: (value: (((prevState: ListingCreateDTO) => ListingCreateDTO) | ListingCreateDTO)) => void;
    listing: ListingCreateDTO
}

export const TitleCreator = (props: TitleCreatorParams) => {
    
    const [title, setTitle] = React.useState<string>(props.listing.title ?? "");
    
    const validateTitle = (value: string) => {
        return value.length < 32;
    }
    
    const updateTitle = (value: string) => {
        value.replace(/\n+/g, ' ').trim();
        setTitle(value);
        if (!validateTitle(value)) return;
        props.setListing((prevState: ListingCreateDTO) => {
            return {...prevState, title: value};
        });
    }
    
    return <div className="title-creator">
        <InputArea value={title} onChange={(e)=> updateTitle(e.target.value)} isValid={validateTitle}></InputArea>
    </div>;
};