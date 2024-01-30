import NavBar from "../NavBar";
import {PopupType, usePopup} from "../../Contexts/PopupContext";
import {useEffect} from "react";

export const LoginPage = () => {
    const {popupType, setPopupType} = usePopup();

    useEffect(() => {
        setPopupType(PopupType.LOGIN);
    }, []);
    
    return <>
        <NavBar/>
    </>
}