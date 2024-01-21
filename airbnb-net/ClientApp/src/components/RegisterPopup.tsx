import React, {useEffect, useMemo} from 'react';
import '../styles/RegisterPopup.scss';
import '../styles/LoginPopup.scss';
import {PopupType, usePopup} from "../Contexts/PopupContext";
import InputField from "./InputFIeld";
import {RegisterUserDTO} from "../DTOs/User/RegisterUserDTO";
import {AuthService} from "../services/AuthService";
import {AuthenticationState, useAuthState} from "../Contexts/AuthStateProvider";
import {useHost} from "../Contexts/HostContext";
import {useUser} from "../Contexts/UserContext";

const RegisterPopup : React.FC = () => {
    
    const {popupType, setPopupType, userEmail, setUserEmail} = usePopup();
    const {user, setUser} = useUser();
    const {authenticationState, setAuthenticationState} = useAuthState();
    
    const [fullNameInputValue, setFullNameInputValue] = React.useState<string>("");
    const [birthdateInputValue, setBirthdateInputValue] = React.useState<string>("");
    const [passwordInputValue, setPasswordInputValue] = React.useState<string>("");
    const [passwordVisible, setPasswordVisible] = React.useState<boolean>(false);
    
    const [passwordValidation_cantContainEmailOrName, setPasswordValidation_cantContainEmailOrName] = React.useState<boolean>(false);
    const [passwordValidation_containsNumberOrSymbol, setPasswordValidation_containsNumberOrSymbol] = React.useState<boolean>(false);
    const [passwordValidation_AtLeast8Characters, setPasswordValidation_AtLeast8Characters] = React.useState<boolean>(false);
    
    const [emailInputInvalid, setEmailInputInvalid] = React.useState<boolean>(false);
    const [birthdateInputInvalid, setBirthdateInputInvalid] = React.useState<boolean>(false);
    
    const authService : AuthService = useMemo(() => AuthService.getInstance(), []);
    
    
    function togglePasswordVisibility() {
        setPasswordVisible(!passwordVisible);
    }
    
    function validatePassword() : boolean {
        let isValid = true;
        if (passwordInputValue.includes(userEmail) || passwordInputValue.includes(fullNameInputValue)) {
            setPasswordValidation_cantContainEmailOrName(true);
            isValid = false;
            console.log("cantContainEmailOrName")
        }else
        {
            setPasswordValidation_cantContainEmailOrName(false);
        }
        if (!/[0-9]/.test(passwordInputValue) && !/[!@#$%^&*]/.test(passwordInputValue)) {
            setPasswordValidation_containsNumberOrSymbol(true);
            console.log("containsNumberOrSymbol")
            isValid = false;
        }else
        {
            setPasswordValidation_containsNumberOrSymbol(false);
        }
        if (passwordInputValue.length < 8) {
            setPasswordValidation_AtLeast8Characters(true);
            console.log("AtLeast8Characters")
            isValid = false;
        }else
        {
            setPasswordValidation_AtLeast8Characters(false);
        }
        console.log("isValid: " + isValid)
        return isValid;
    }
    
    function $continue() {
        if(!passwordValidation_AtLeast8Characters && 
            !passwordValidation_containsNumberOrSymbol &&
            !passwordValidation_cantContainEmailOrName &&
            !emailInputInvalid &&
            !birthdateInputInvalid)
        {
            const registerUserDTO : RegisterUserDTO = {
                name: fullNameInputValue,
                email: userEmail,
                password: passwordInputValue,
                birthdate: new Date(birthdateInputValue)
            }
            
            authService.register(registerUserDTO).then((result)=>{
                console.log("register result: ");
                console.log(result);
                if(result){
                    setPopupType(PopupType.NONE);
                    setUser(result.user);
                    setAuthenticationState(AuthenticationState.AuthenticatedUser);
                }
            });
        }
        
    }

    function validateEmail(input: string) : boolean {
        if (userEmail === null || userEmail === undefined || userEmail.trim() === '') {
            return true;
        }
        const index = userEmail.indexOf('@');

        const isEmail = index > 0 && index !== userEmail.length - 1 && index === userEmail.lastIndexOf('@');
        return isEmail;
    }

    function validateBirthdate(input: string) : boolean {
        if(birthdateInputValue === null || birthdateInputValue === undefined || birthdateInputValue.trim() === ''){return true;}
        const date = new Date(input);
        const now = new Date();
        console.log("birthdate: " + date.getTime() + " now: " + now.getTime());
        return now.getTime() - date.getTime() > 568025136000;
    }

    useEffect(() => {
        validatePassword();
    }, [passwordInputValue]);

    useEffect(() => {
        console.log("birthdateInputValue: " + birthdateInputValue);
    }, [birthdateInputValue]);
    
    return <>
        <div className="login-popup-background" onClick={() => setPopupType(PopupType.LOGIN)}></div>
        <div className="register-popup">
            <div className="login-header-back-button" onClick={() => setPopupType(PopupType.LOGIN)}>
                <div className="back-hover-circle"></div>
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32"
                     style={{
                         display: "block",
                         fill: "none",
                         height: "16px",
                         width: "16px",
                         stroke: "currentcolor",
                         strokeWidth: "3px",
                         overflow: "visible"
                     }}
                     aria-hidden="true" role="presentation" focusable="false">
                    <path fill="none" d="M20 28 8.7 16.7a1 1 0 0 1 0-1.4L20 4"></path>
                </svg>
            </div>
            <div className="login-header">
                <div className="login-header-text"><span className={"really-bold"}>Register</span></div>
            </div>
            <div className="register-body">
                <InputField className={"fullname-input"} type={"text"} label={"Full name"} isValid={() => true}
                            onInvalid={() => {
                            }} value={fullNameInputValue} onChange={(e) => {
                    setFullNameInputValue(e.target.value)
                }} lowerText={"Make sure it matches your government issued ID"}></InputField>
                <InputField className={"birthdate-input"} type={"date"} label={"Birth date"} isValid={validateBirthdate}
                            onInvalid={(isInvalid) => {
                                setBirthdateInputInvalid(isInvalid)
                            }} value={birthdateInputValue} onChange={(e) => {
                    setBirthdateInputValue(e.target.value)
                }} lowerText={"You may register if you are more than 18 years old"}></InputField>
                <InputField className={"email-input"} type={"email"} label={"Email"} isValid={validateEmail}
                            onInvalid={(isInvalid) => {
                                setEmailInputInvalid(isInvalid)
                            }} value={userEmail} onChange={(e) => {
                    setUserEmail(e.target.value)
                }} lowerText={"We will email you trip info and receipts"}></InputField>
                <InputField className={"password-input"} type={passwordVisible ? "text" : "password"} label={"Password"}
                            isValid={() => true} onInvalid={() => {
                }} value={passwordInputValue} onChange={(e) => {
                    setPasswordInputValue(e.target.value)
                }}>
                    <button className={"toggle-password-visibility-btn"}
                            onClick={togglePasswordVisibility}>{passwordVisible ? "Hide" : "Show"}</button>
                </InputField>
                <div className={"password-validation"}>
                    <div
                        className={"password-validation-item " + (passwordValidation_cantContainEmailOrName ? "invalid" : "")}>Password
                        can't contain your email or name
                    </div>
                    <div
                        className={"password-validation-item " + (passwordValidation_containsNumberOrSymbol ? "invalid" : "")}>Password
                        must contain at least one number or symbol
                    </div>
                    <div
                        className={"password-validation-item " + (passwordValidation_AtLeast8Characters ? "invalid" : "")}>Password
                        must be at least 8 characters
                    </div>
                </div>
                <button className="continue-button-container form-button" type="button" onClick={$continue}>
                    Register
                </button>
            </div>
        </div>
    </>;
}

export default RegisterPopup;