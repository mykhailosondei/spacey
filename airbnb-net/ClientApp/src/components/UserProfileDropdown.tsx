import React from "react";
import {ReactComponent as DefaultAvatar} from "../values/svgs/default-profile.svg";
import "../styles/UserProfileDropdown.scss";
import {useAuth} from "../Contexts/AuthContext";
import {Link} from "react-router-dom";
import {useLoginPopup} from "../Contexts/LoginPopupContext";

const UserProfileDropdown : React.FC = () => {
    
    const { authUser, setAuthUser, isAuthenticated, setIsAuthenticated } = useAuth()!;
    const [isDropdownVisible, setIsDropdownVisible] = React.useState<boolean>(false);
    const { popupActivated, setPopupActivated } = useLoginPopup()!;
    
    function toggleDropdown(){
        setIsDropdownVisible(!isDropdownVisible);
    }
    
    function setPopupTrue(){
        setPopupActivated(true);
    }
    
    function returnDropdown(){
        return <div className="user-dropdown">
            <div className="user-dropdown-item">Profile</div>
            <div className="user-dropdown-item">Account</div>
            <div className="user-dropdown-divider"></div>
            <div className="user-dropdown-item">Help</div>
            {isAuthenticated ?
                <div className="user-dropdown-item">Log Out</div>
                :
                <div className="user-dropdown-item" onClick={setPopupTrue}>Log In</div>
            }
            {!isAuthenticated ?
                <div className="user-dropdown-item" onClick={setPopupTrue}>Sign up</div>
                :
                null
            }
        </div>
    }

    return <>
        <div className="userButton" onClick={toggleDropdown}>
            <div className="three-lines">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32" aria-hidden="true" role="presentation"
                     focusable="false">
                    <g fill="none">
                        <path d="M2 16h28M2 24h28M2 8h28"></path>
                    </g>
                </svg>
            </div>
            <div className="user-avatar">
                {isAuthenticated ? "auth" : <DefaultAvatar/>}
            </div>
        </div>
        {isDropdownVisible ? returnDropdown() : null}
    </>
}

export default UserProfileDropdown