import {PropertyType} from "../values/PropertyType";

interface PropertyEditItemProps {
    name: string;
    value: PropertyType;
    onSubmit: (value: PropertyType) => void;
}

export const PropertyEditItem = (props: PropertyEditItemProps) => {
    return <div className="property-edit-item">
            <div className="name-title-and-edit">
                <div className="name-title">{props.name}</div>
                <div className="black-underline-hover-btn">Edit</div>
            </div>
            <div className="value">{PropertyType[props.value]}</div>
        <div className="horizontal-divider width-100"></div>
    </div>;
};