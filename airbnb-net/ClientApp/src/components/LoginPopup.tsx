import React, {useContext, useEffect} from "react";
import "../styles/LoginPopup.scss";
import {useLoginPopup} from "../Contexts/LoginPopupContext";
import {AuthService} from "../services/AuthService";
import InputField from "./InputFIeld";
import {InputFieldProps} from "./InputFIeld";

const LoginPopup : React.FC = () => {
    
    const authService = AuthService.getInstance();
    
    const {popupActivated, setPopupActivated} = useLoginPopup();
    
    const [emailLoading, setEmailLoading] = React.useState<boolean>(false);
    const [emailTaken, setEmailTaken] = React.useState<boolean>(false);
    const [inputValue, setInputValue] = React.useState<string>("");
    const [emailInputInvalid, setEmailInputInvalid] = React.useState<boolean>(false);
    
    
    async function isEmailTaken(email: string) : Promise<boolean> {
        return await authService.isRegistered(email);
    }

    function validateEmail(input: string) : boolean {
        if (inputValue === null || inputValue === undefined || inputValue.trim() === '') {
            return true;
        }
        const index = inputValue.indexOf('@');

        const isEmail = index > 0 && index !== inputValue.length - 1 && index === inputValue.lastIndexOf('@');
        return isEmail;
    }
    
    async function $continue(){
        if (emailInputInvalid){
            return;
        }
        setEmailLoading(true);
        isEmailTaken(inputValue).then((result) => {
            setEmailTaken(result);
            console.log("email taken: " + result);
            setEmailLoading(false);
        });
    }
    
    return <>
        {popupActivated?
            <div>
                <div className="login-popup-background" onClick={() => setPopupActivated(false)}></div>
                <div className="login-popup">
                    <div className="login-header-close-button" onClick={() => setPopupActivated(false)}>
                        <div className={"back-hover-circle"}></div>
                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32"
                             style={{
                                 display: 'block',
                                 fill: 'none',
                                 height: '16px',
                                 width: '16px',
                                 stroke: 'currentcolor',
                                 strokeWidth: "3px",
                                 overflow: 'visible'
                             }}
                             aria-hidden="true" role="presentation" focusable="false">
                            <path d="m6 6 20 20M26 6 6 26"></path>
                        </svg>
                    </div>
                    <div className="login-header">
                        <div className="login-header-text"><span className={"really-bold"}>Log in</span></div>
                    </div>
                    <div className="login-body">
                        <h3>Welcome to Airbnb</h3>
                            <InputField className={"input-container"} 
                                        value={inputValue} 
                                        label={"Email"} 
                                        type={"text"} 
                                        onChange={(e)=>{setInputValue(e.target.value)}}
                                        isValid={validateEmail}
                                        onInvalid={(isInvalid)=>{setEmailInputInvalid(isInvalid)}}
                            ></InputField>
                            <button className="continue-button-container form-button" type="button" onClick={$continue} disabled={emailLoading}>
                                {!emailLoading?"Continue":""}
                                <div className={"continue-button-loading"}>
                                    <span className={"bullet left-bullet"}></span>
                                    <span className={"bullet mid-bullet"}></span>
                                    <span className={"bullet right-bullet"}></span>
                                </div>
                            </button>
                    </div>
                </div>
            </div>
            : null}
    </>
}

export default LoginPopup