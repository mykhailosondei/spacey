import React, {createContext, useContext, useEffect, useState} from "react";
import LoginPopup from "../components/LoginPopup";
import {RegisterPopup} from "../components/RegisterPopup";
import {PasswordPopup} from "../components/PasswordPopup";

const LoginPopupContext = createContext<{popupActivated:boolean, setPopupActivated:Function}>({popupActivated:false, setPopupActivated:()=>{}});
const PasswordPopupContext = createContext<{passwordPopupActivated:boolean, setPasswordPopupActivated:Function}>({passwordPopupActivated:false, setPasswordPopupActivated:()=>{}});
const RegisterPopupContext = createContext<{registerPopupActivated:boolean, setRegisterPopupActivated:Function}>({registerPopupActivated:false, setRegisterPopupActivated:()=>{}});
export default LoginPopupContext;

export function useLoginPopup(){
    return useContext(LoginPopupContext);
}
export function usePasswordPopup(){
    return useContext(PasswordPopupContext);
}

export function useRegisterPopup(){
    return useContext(RegisterPopupContext);
}

export function LoginPopupProvider(props: any){
    const [popupActivated, setPopupActivated] = useState<boolean>(false);
    const [passwordPopupActivated, setPasswordPopupActivated] = useState<boolean>(false);
    const [registerPopupActivated, setRegisterPopupActivated] = useState<boolean>(false);
    
    const value = {
        popupActivated,
        setPopupActivated
    };
    
    const passwordPopupValue = {
        passwordPopupActivated,
        setPasswordPopupActivated
    };
    
    const registerPopupValue = {
        registerPopupActivated,
        setRegisterPopupActivated
    };

    useEffect(() => {
        console.log("popupActivated: " + popupActivated)
    }, [popupActivated]);
    
    return (
        <LoginPopupContext.Provider value={value}>
            <PasswordPopupContext.Provider value={passwordPopupValue}>
                <RegisterPopupContext.Provider value={registerPopupValue}>
                    <LoginPopup></LoginPopup>
                    <RegisterPopup></RegisterPopup>
                    <PasswordPopup></PasswordPopup>
                    {props.children}
                </RegisterPopupContext.Provider>
            </PasswordPopupContext.Provider>
        </LoginPopupContext.Provider>
    );
}