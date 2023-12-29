import React, {useContext, useEffect} from "react";
import "../styles/LoginPopup.scss";
import {useLoginPopup} from "../Contexts/LoginPopupContext";

const LoginPopup : React.FC = () => {
    
    const {popupActivated, setPopupActivated} = useLoginPopup();
    
    const [emailLoading, setEmailLoading] = React.useState<boolean>(false);
    
    console.log("popupActivated from popup: " + popupActivated)

    async function delay(number: number) {
        return new Promise(resolve => setTimeout(resolve, number));
    }

    async function $continue(){
        setEmailLoading(true);
        await delay(3000);
        setEmailLoading(false);
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
                        <div className="input-container">
                            <form action="">
                                <div className="input-label">Email address</div>
                                <div className={"input-field-holder"}><input className="input-field" type="text"/></div>
                            </form>
                        </div>
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