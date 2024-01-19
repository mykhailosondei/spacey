import React from "react";
import "../styles/LoginPopup.scss";
import {BookingService} from "../services/BookingService";

export interface InputFieldProps {
    label: string;
    type: string;
    value: string;
    onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
    error?: string;
    className?: string;
    disabled?: boolean;
    isValid: (input:string)=>boolean;
    onInvalid: (isInvalid:boolean)=>void;
}

const InputField: React.FC<InputFieldProps> = (props) => {
    const [inputValue, setInputValue] = React.useState<string>(props.value);
    const [inputInvalid, setInputInvalid] = React.useState<boolean>(false);

    React.useEffect(() => {
        setInputValue(props.value);
    }, [props.value]);

    React.useEffect(() => {
        validateInput();
    }, [inputValue]);

    function validateInput() : void {
        setInputInvalid(!props.isValid(inputValue));
        props.onInvalid(!props.isValid(inputValue));
    }
    

    return <div className={"input-container " + (inputInvalid?"invalid-container":"")}>
        <label className={"input-label " + (inputValue != ''? "active " : "")}>{props.label}</label>
        <div className="input-field-holder">
            <input
                className={"input-field" + (inputInvalid ? " input-invalid" : "")}
                type={props.type}
                value={inputValue}
                onChange={(e) => props.onChange(e)}
                disabled={props.disabled}
            />
        </div>
    </div>;
};

export default InputField;