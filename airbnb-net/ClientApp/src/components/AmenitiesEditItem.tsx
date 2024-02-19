import InputField from "./InputFIeld";
import React, {useEffect, useState} from "react";
import {AmenitiesDropdown} from "./AmenitiesDropdown";
import {ListingFilter} from "./HostingListingsSection";
import {amenityIconsDictionary} from "../values/AmenityIconsDictionary";

interface AmenitiesEditItemProps {
    value: string[];
    onSubmit: (value: string[]) => void;
}

export const AmenitiesEditItem = (props: AmenitiesEditItemProps) => {
    
    const [editMode, setEditMode] = useState(false);
    

    const [amenities, setAmenities] = React.useState<{name: string, selected: boolean}[]>([]);
    useEffect(() => {
        const amenitiesList = [
            "Wifi", "TV", "Kitchen", "AirConditioning", "Heating", "Washer", "Dryer", "FreeParking", "Pool", "Gym", "HotTub", "PetFriendly", "SmokingAllowed", "FamilyFriendly", "WheelchairAccessible"
        ];
        setAmenities(amenitiesList.map((amenity) => {
            return {name: amenity, selected: props.value.includes(amenity)};
        }));
    }, []);

    const handleSave = () => {
        const amenitiesValue = amenities.filter((amenity) => amenity.selected).map((amenity) => amenity.name);
        props.onSubmit(amenitiesValue);
        setEditMode(false);
    }
    
    return !editMode ? <div>
        <div className="amenities-edit-item">
            <div className="amenities-list">
                {props.value.map((amenity) => {
                    return <div className="value" key={amenity}>{amenity}</div>;
                })}
            </div>
            <div className="black-underline-hover-btn" onClick={() => setEditMode(true)}>Edit</div>
        </div>
        <div className="horizontal-divider width-100"></div>
    </div> : <div className={"string-edit-mode"}>
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
        <div className={"buttons"}>
            <div className="black-underline-hover-btn" onClick={() => setEditMode(false)}>Cancel</div>
            <div className="white-on-black-btn" onClick={handleSave}>Save</div>
        </div>
    </div>;
};