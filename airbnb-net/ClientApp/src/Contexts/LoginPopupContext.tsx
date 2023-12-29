import {createContext, useContext, useEffect, useState} from "react";
import {types} from "sass";
import Boolean = types.Boolean;

const LoginPopupContext = createContext<{popupActivated:boolean, setPopupActivated:Function}>({popupActivated:false, setPopupActivated:()=>{}});
export default LoginPopupContext;

export function useLoginPopup(){
    return useContext(LoginPopupContext);
}

export function LoginPopupProvider(props: any){
    const [popupActivated, setPopupActivated] = useState<boolean>(false);
    const value = {
        popupActivated,
        setPopupActivated
    };

    useEffect(() => {
        console.log("popupActivated: " + popupActivated)
    }, [popupActivated]);
    
    return (
        <LoginPopupContext.Provider value={value}>
            {props.children}
        </LoginPopupContext.Provider>
    );
}