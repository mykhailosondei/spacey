import {ListingCreateDTO} from "../DTOs/Listing/ListingCreateDTO";
import {IconLabelButton} from "./IconLabelButton";
import {PropertyType} from "../values/PropertyType";
import {propertyTypeDictionary} from "../values/PropertyTypeTitles";
import {useEffect, useState} from "react";

interface PropertyTypeCreatorParams {
    setListing: (value: (((prevState: ListingCreateDTO) => ListingCreateDTO) | ListingCreateDTO)) => void;
    listing: ListingCreateDTO
}

export const PropertyTypeCreator = (props: PropertyTypeCreatorParams) => {
    
    const propertyTypes = Object.values(PropertyType).filter((propertyType) => typeof propertyType === "string") as string[];

    const [chosenPropertyType, setChosenPropertyType] = useState<number>(props.listing.propertyType);
    
    useEffect(() => {
        props.setListing({...props.listing, propertyType: chosenPropertyType});
    }, [chosenPropertyType]);
    
    return <div className="property-type-creator">
        <div className="property-types">
            {propertyTypes.map((propertyType, index) => {
                return <IconLabelButton onClick={()=>setChosenPropertyType(index)} chosen={index == chosenPropertyType} className={"third-flex"} image={propertyTypeDictionary[propertyType]} label={propertyType}/>
            })}
        </div>
    </div>;
};