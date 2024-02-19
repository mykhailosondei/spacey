import {useCallback, useState} from "react";
import InputField from "./InputFIeld";

interface PriceEditItemProps {
    name: string;
    value: number;
    onSubmit: (value: number) => void;
}

export const PriceEditItem = (props: PriceEditItemProps) => {
    const [editMode, setEditMode] = useState(false);

    const [price, setPrice] = useState<number>(props.value ?? 0);
    
    const [error, setError] = useState(false);
    
    const formatValue = (value: number) => {
        return `$${value}`;
    }

    const validatePrice = (value: string) => {
        return !isNaN(Number(value)) && Number(value) > 15 && Number(value) < 10000;
    }

    const changePrice = (value: string) => {
        if (isNaN(Number(value))) return;
        if(Number(value) > 10000) value = "10000";
        setPrice(Number(value));
    }


    function handleSave() {
        if (validatePrice(price.toString())) {
            props.onSubmit(price);
            setEditMode(false);
        } else {
            setError(true);
            setTimeout(() => {setEditMode(false); setError(false)}, 1000);
        }
    }

    return !editMode ? <div className="price-edit-item">
            <div className="name-title-and-edit">
                <div className="name-title">{props.name}</div>
                <div className="black-underline-hover-btn" onClick={()=>setEditMode(true)}>Edit</div>
                </div>
            <div className="value">{formatValue(props.value)}</div>
        <div className="horizontal-divider width-100"></div>
    </div> : <div className={"price-edit-mode"}>
        <div className="listing-detail-title margin-b20">{props.name}</div>
        <span className="big-dollar">$</span>
        <input type="text" className={"big-price input-field " + (validatePrice(price.toString()) ? "" : "invalid")}
               value={price} onChange={(e) => changePrice(e.target.value)}/>
        <div className={"buttons"}>
            <div className="black-underline-hover-btn" onClick={() => setEditMode(false)}>Cancel</div>
            <div className="error-message" style={{visibility: error ? "visible" : "hidden"}}>Price must be in the range from 15 to 10000</div>
            <div className="white-on-black-btn" onClick={() => {
                handleSave();
            }
            }>Save</div>
        </div>
    </div>;
};