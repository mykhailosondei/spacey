import React, {useMemo} from "react";
import InputField from "./InputFIeld";
import "../styles/LoginPopup.scss";
import "../styles/PasswordPopup.scss";
import {PopupType, usePopup} from "../Contexts/PopupContext";
import {AuthService} from "../services/AuthService";
import {AuthenticationState, useAuthState} from "../Contexts/AuthStateProvider";
import {useHost} from "../Contexts/HostContext";
import {useUser} from "../Contexts/UserContext";

const PasswordPopup : React.FC = () => {
    const [passwordInput, setPasswordInput] = React.useState<string>("");
    const {popupType, setPopupType, userEmail} = usePopup();
    const [passwordCheckLoading, setPasswordCheckLoading] = React.useState<boolean>(false);
    const {setAuthenticationState} = useAuthState();
    const {setUser} = useUser();
    let authService : AuthService = useMemo(()=> AuthService.getInstance(),[]);
    
    function $continue() {
        if (passwordInput === null || passwordInput === undefined || passwordInput.trim() === '') {
            return;
        }
        setPasswordCheckLoading(true);
        authService.login({email:userEmail, password:passwordInput}).then((result) => {
            console.log("login result: ", result);
            if (result) {
                setPopupType(PopupType.NONE);
                setAuthenticationState(AuthenticationState.AuthenticatedUser);
                setUser(result.data.user);
            }else
            {
                handleIncorrectPassword();
            }
            setPasswordCheckLoading(false);
        });
    }
    
    function handleIncorrectPassword() {
        
    }

    return <>
        <div className="login-popup-background" onClick={() => setPopupType(PopupType.LOGIN)}></div>
        <div className="password-popup" >
            <div className="login-header-back-button" onClick={() => setPopupType(PopupType.LOGIN)}>
                <div className="back-hover-circle"></div>
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32"
                     style={{display: "block", fill: "none", height: "16px", width: "16px", stroke: "currentcolor", strokeWidth: "3px", overflow: "visible"}}
                     aria-hidden="true" role="presentation" focusable="false">
                    <path fill="none" d="M20 28 8.7 16.7a1 1 0 0 1 0-1.4L20 4"></path>
                </svg>
            </div>
            <div className="login-header">
                <div className="login-header-text"><span className={"really-bold"}>Log in</span></div>
            </div>
            <div className="login-body">
                <InputField type={"text"} label={"Password"} value={passwordInput} onChange={(e)=>{setPasswordInput(e.target.value)}} className={"input-container"} isValid={(s)=>true} onInvalid={(bool)=>{}}></InputField>
                <button className="continue-button-container form-button" type="button" onClick={$continue}
                        disabled={passwordCheckLoading}>
                    {!passwordCheckLoading ? "Log in" : ""}
                    <div className={"continue-button-loading"}>
                        <span className={"bullet-load left-bullet"}></span>
                        <span className={"bullet-load mid-bullet"}></span>
                        <span className={"bullet-load right-bullet"}></span>
                    </div>
                </button>
            </div>
        </div>
    </>;
}

export default PasswordPopup;