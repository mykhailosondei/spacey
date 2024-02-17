import {ListingCreateDTO} from "../../DTOs/Listing/ListingCreateDTO";
import {IconLabelButton} from "./IconLabelButton";
import Logo from "../Icons/Logo";

interface PropertyTypeCreatorParams {
    setListing: (value: (((prevState: ListingCreateDTO) => ListingCreateDTO) | ListingCreateDTO)) => void;
    listing: ListingCreateDTO
}

export const PropertyTypeCreator = (props: PropertyTypeCreatorParams) => {
    return <div className="property-type-creator">
        <div className="property-types">
            <IconLabelButton icon={<Logo/>} label={"Logo"}/>
        </div>
    </div> ;
};