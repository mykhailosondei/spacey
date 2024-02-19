
interface PriceEditItemProps {
    name: string;
    value: number;
    onSubmit: (value: number) => void;
}

export const PriceEditItem = (props: PriceEditItemProps) => {
    return <div className="price-edit-item">
            <div className="name-title-and-edit">
                <div className="name-title">{props.name}</div>
                <div className="black-underline-hover-btn">Edit</div>
                </div>
            <div className="value">${props.value}</div>
        <div className="horizontal-divider width-100"></div>
    </div>;
};