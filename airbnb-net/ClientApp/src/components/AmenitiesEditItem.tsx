
interface AmenitiesEditItemProps {
    value: string[];
    onSubmit: (value: string[]) => void;
}

export const AmenitiesEditItem = (props: AmenitiesEditItemProps) => {
    return <div>
        <div className="amenities-edit-item">
            <div className="amenities-list">
                {props.value.map((amenity) => {
                    return <div className="value" key={amenity}>{amenity}</div>;
                })}
            </div>
            <div className="black-underline-hover-btn">Edit</div>
        </div>
        <div className="horizontal-divider width-100"></div>
    </div>;
};