import React, {useEffect} from "react";
import {ListingFilter} from "./HostingListingsSection";
import {amenityIconsDictionary} from "../values/AmenityIconsDictionary";

interface AmenitiesDropdownProps {
    onApplyClick: (filter: ListingFilter) => void;
    filter: ListingFilter;
}

export const AmenitiesDropdown = (props: AmenitiesDropdownProps) => {
    
    const [amenities, setAmenities] = React.useState<{name: string, selected: boolean}[]>([]);
    useEffect(() => {
        const amenitiesList = [
            "Wifi", "TV", "Kitchen", "AirConditioning", "Heating", "Washer", "Dryer", "FreeParking", "Pool", "Gym", "HotTub", "PetFriendly", "SmokingAllowed", "FamilyFriendly", "WheelchairAccessible"
        ];
        setAmenities(amenitiesList.map((amenity) => {
            return {name: amenity, selected: props.filter.amenities?.includes(amenity) || false};
        }));
    }, []);

    function clearAllSelection() {
        setAmenities(amenities.map((amenity) => {
            return {name: amenity.name, selected: false};
        }));
    }

    function onApplyClick() {
        const selectedAmenities = amenities.filter((amenity) => amenity.selected).map((amenity) => amenity.name);
        props.onApplyClick({...props.filter, amenities: selectedAmenities});
    }

    return <div className={"rooms-and-beds-dropdown dropdown-body"}>
        <div className="dropdown-content">
            <div className="dropdown-section">
                <div className="amenities-checkboxes">
                    {amenities.map((amenity, index) => {
                        const Icon = amenityIconsDictionary[amenity.name];
                        return <div key={index} className="amenity-checkbox">
                            <input type="checkbox" id={amenity.name} name={amenity.name} checked={amenity.selected} onChange={(e) => {
                                const newAmenities = [...amenities];
                                newAmenities[index].selected = e.target.checked;
                                setAmenities(newAmenities);
                            }}/>
                            <label htmlFor={amenity.name}>
                                <Icon className="amenity-icon" title={amenity.name}/>
                                <span>{amenity.name}</span>
                            </label>
                        </div>;
                    })}
                </div>
            </div>
            <div className="horizontal-divider"></div>
            <div className="dropdown-section">
                <div className="align-d-buttons">
                    <div className="cancel-button custom-width" onClick={clearAllSelection}>Clear</div>
                    <div className="white-on-black-btn custom-width margin-0" onClick={onApplyClick}>Apply</div>
                </div>
            </div>
        </div>
    </div>;
};