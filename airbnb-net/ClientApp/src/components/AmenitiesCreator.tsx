import {ListingCreateDTO} from "../DTOs/Listing/ListingCreateDTO";
import {useState} from "react";
import {IconLabelButton} from "./IconLabelButton";
import {amenityIconsDictionary} from "../values/AmenityIconsDictionary";

interface AmenitiesCreatorParams {
        setListing: (value: (((prevState: ListingCreateDTO) => ListingCreateDTO) | ListingCreateDTO)) => void;
        listing: ListingCreateDTO
}

export const AmenitiesCreator = (props: AmenitiesCreatorParams) => {
    
    const [chosenAmenities, setChosenAmenities] = useState<string[]>(props.listing.amenities ?? []);
    
    const amenities = [
        "Wifi",
        "TV",
        "Kitchen",
        "AirConditioning",
        "Heating",
        "Washer",
        "Dryer",
        "FreeParking",
        "Pool",
        "Gym",
        "HotTub",
        "PetFriendly",
        "SmokingAllowed",
        "FamilyFriendly",
        "WheelchairAccessible"
    ];

    function toggleAmenity(amenity: string){
            if(chosenAmenities.includes(amenity)){
            setChosenAmenities(chosenAmenities.filter((a) => a !== amenity));
        }   else {
            setChosenAmenities([...chosenAmenities, amenity]);
        }
        props.setListing({...props.listing, amenities: chosenAmenities});
    }

    return <div className="amenities-creator">
        <div className="ac-amenities">
            {amenities.map((amenity, index) =>
                {
                    const Icon = amenityIconsDictionary[amenity];
                    return <IconLabelButton className={"third-flex"} chosen={chosenAmenities.includes(amenity)} onClick={() => toggleAmenity(amenity)} label={amenity} icon={<Icon/>}></IconLabelButton>
                }
            )}
        </div>
    </div>; 
};