import {PropertyType} from "../values/PropertyType";
import {useState} from "react";

interface PropertyEditItemProps {
    name: string;
    value: PropertyType;
    onSubmit: (value: PropertyType) => void;
    unavailable?: boolean;
}

export const PropertyEditItem = (props: PropertyEditItemProps) => {
    
    const [editMode, setEditMode] = useState(false);
    
    return !editMode ?  <div className="property-edit-item">
            <div className="name-title-and-edit">
                <div className="name-title">{props.name}</div>
                <div className="black-underline-hover-btn" onClick={() => setEditMode(true)}>Edit</div>
            </div>
            <div className="value">{PropertyType[props.value]}</div>
        <div className="horizontal-divider width-100"></div>
    </div> :
        <div className={"string-edit-mode"}>
            <div className="listing-detail-title margin-b20">{props.name}</div>
            <select className={"selector"} disabled={!!props.unavailable} value={props.value} onChange={(e) => props.onSubmit(parseInt(e.target.value))}>
                {Object.keys(PropertyType).filter(key=> !isNaN(parseInt(key))).map((key) => {
                    return <option key={key} value={key}>{PropertyType[parseInt(key)]}</option>;
                })}
            </select>
            <div className={"buttons"}>
                <div className="black-underline-hover-btn" onClick={() => setEditMode(false)}>Cancel</div>
            </div>
        </div>;
};