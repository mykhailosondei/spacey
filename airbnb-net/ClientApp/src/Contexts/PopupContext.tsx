import React, {createContext, useContext, useEffect, useState} from "react";
import LoginPopup from "../components/LoginPopup";
import RegisterPopup from "../components/RegisterPopup";
import PasswordPopup from "../components/PasswordPopup";

export enum PopupType{
    NONE,
    LOGIN,
    REGISTER,
    PASSWORD
}
const PopupContext = createContext<{popupType:PopupType, setPopupType:Function, userEmail:string, setUserEmail:Function}>
                                                     ({popupType:PopupType.NONE, setPopupType:()=>{}, userEmail:"", setUserEmail:()=>{}});

export default PopupContext;

export function usePopup(){
    return useContext(PopupContext);
}



export function PopupProvider(props: any){
    const [popupType, setPopupType] = useState<PopupType>(PopupType.NONE);
    const [userEmail, setUserEmail] = useState<string>("");
    
    const popupTypeValue = {
        popupType,
        setPopupType,
        userEmail,
        setUserEmail
    };
    
    function popupRender(popupType: PopupType){
        switch(popupType){
            case PopupType.NONE:
                return null;
            case PopupType.LOGIN:
                return <LoginPopup/>;
            case PopupType.PASSWORD:
                return <PasswordPopup/>;
            case PopupType.REGISTER:
                return <RegisterPopup/>;
        }
    }
    
    return (
        <PopupContext.Provider value={popupTypeValue}>
            {popupRender(popupType)}
            {props.children}
        </PopupContext.Provider>
    );
}