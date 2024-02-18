import {ListingCreateDTO} from "../DTOs/Listing/ListingCreateDTO";
import {NavBarDropdownWhere} from "./NavBarDropdownWhere";
import React from "react";
import InputField from "./InputFIeld";

interface LocationCreatorParams {
    setListing: (value: (((prevState: ListingCreateDTO) => ListingCreateDTO) | ListingCreateDTO)) => void;
    listing: ListingCreateDTO
}

export const LocationCreator = (props: LocationCreatorParams) => {
    
    const [whereQuery, setWhereQuery] = React.useState<string>("1");
    
    const notEmptyOrSpaces = (str: string) => {
        return str !== null && str.match(/^ *$/) === null;
    }
    
    React.useEffect(() => {
        props.setListing({...props.listing, address: whereQuery});
    }, [whereQuery]);
    
    return <div className={"location-creator"}>
        <div className="lc-content">
            <InputField value={whereQuery} onChange={(e) => setWhereQuery(e.target.value)} label={"Location"} onInvalid={()=>{}} isValid={()=>true} type={"text"}></InputField>
            <NavBarDropdownWhere className={"relative"} whereQuery={whereQuery} setWhereQuery={setWhereQuery} active={notEmptyOrSpaces(whereQuery)}></NavBarDropdownWhere>
        </div>
    </div>;
};