import "../styles/InputArea.scss";
import React from "react";

interface InputAreaParams {
    value: string;
    onChange: (e: React.ChangeEvent<HTMLTextAreaElement>) => void;
    isValid?: (value: string) => boolean;
    onInvalid?: () => void;
}

export const InputArea = (props: InputAreaParams) => {
    
    const textAreaRef = React.useRef<HTMLTextAreaElement>(null);
    
    const validateInput = (value: string) => {
        if (props.isValid) {
            return props.isValid(value);
        }
    };
    
    React.useEffect(() => {
        const element = textAreaRef.current;
        if (element){
            element.style.height = "auto";
            element.style.height = (element.scrollHeight) + "px";
        }
        
    }, [props.value]);
    
    return <textarea ref={textAreaRef} className={"text-input-area" + (validateInput(props.value) ? "" : " invalid")} onChange={props.onChange} value={props.value}/>;
};