import {useState} from "react";
import InputField from "./InputFIeld";

interface StringEditItemProps {
    name: string;
    value: string;
    isValid?: (value : string) => boolean
    onSubmit: (value: string) => void;
}


export const StringEditItem = (props: StringEditItemProps) => {
    
    const [editMode, setEditMode] = useState(false);
    
    const [value, setValue] = useState(props.value);
    
    const formatValue = (value: string) => {
        if(value.length > 32) {
            return value.substring(0, 32) + "...";
        }
        return value;
    }

    function validateInput(value: string) {
        if(props.isValid) {
            return props.isValid(value);
        }
        return true;
    }
    
    const handleSave = () => {
        if(validateInput(value)) {
            props.onSubmit(value);
            setEditMode(false);
        }
    }

    return !editMode ? <div className="string-edit-item">
            <div className="name-title-and-edit">
                <div className="name-title">{props.name}</div>
                <div className="black-underline-hover-btn" onClick={() => setEditMode(true)}>Edit</div>
            </div>
            <div className="value">{formatValue(props.value)}</div>
            <div className="horizontal-divider width-100"></div>
    </div> : <div className={"string-edit-mode"}>
        <div className="listing-detail-title margin-b20">{props.name}</div>
        <InputField value={value} isValid={(value) => validateInput(value)} onChange={(e) => setValue(e.target.value)} label={""} type={"text"} onInvalid={()=>{}}></InputField>
        <div className={"buttons"}>
            <div className="black-underline-hover-btn" onClick={() => setEditMode(false)}>Cancel</div>
            <div className="white-on-black-btn" onClick={handleSave}>Save</div>
        </div>
    </div>;
};