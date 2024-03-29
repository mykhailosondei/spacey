import React, {useEffect} from "react";
import {ReactComponent as DefaultAvatar} from "../values/svgs/default-profile.svg";
import "../styles/UserProfileDropdown.scss";
import {PopupType, usePopup} from "../Contexts/PopupContext";
import {AuthenticationState, useAuthState} from "../Contexts/AuthStateProvider";
import {useUser} from "../Contexts/UserContext";
import {AuthService} from "../services/AuthService";

const UserProfileDropdown : React.FC = () => {
    
    const { user, setUser} = useUser()!;
    const { authenticationState, setAuthenticationState } = useAuthState()!;
    const [ isDropdownVisible, setIsDropdownVisible] = React.useState<boolean>(false);
    const { popupType, setPopupType } = usePopup()!;
    const authService = React.useMemo(() => {return AuthService.getInstance()}, []);
    
    function toggleDropdown(){
        setIsDropdownVisible(!isDropdownVisible);
    }

    useEffect(() => {
        document.addEventListener('click', (e) => {
            let isIntersecting : boolean;
            let buttonBbox = document.querySelector('.userButton')?.getBoundingClientRect();
            let bbox = document.querySelector('.user-dropdown')?.getBoundingClientRect();
            if(!bbox) return;
            let isIntersectingButton = e.clientX >= buttonBbox!.left && e.clientX <= buttonBbox!.right && e.clientY >= buttonBbox!.top && e.clientY <= buttonBbox!.bottom;
            if(isIntersectingButton) return;
            isIntersecting = e.clientX >= bbox!.left && e.clientX <= bbox!.right && e.clientY >= bbox!.top && e.clientY <= bbox!.bottom;
            if (!isIntersecting) {
                setIsDropdownVisible(false);
            }
        });
        return () => {
            document.removeEventListener('click', (e) => {
                
            });
        }
    }, []);

    useEffect(() => {
        document.body.style.overflow = popupType != PopupType.NONE ? 'hidden' : 'unset';
    }, [popupType]);
    
    function setPopupLogin(){
        setPopupType(PopupType.LOGIN);
    }

    function logOut() {
        setUser(null);
        setAuthenticationState(AuthenticationState.Unauthenticated);
        authService.logout();
    }
    
    function isAuthenticated(){
        return authenticationState != AuthenticationState.Unauthenticated;
    }


    function goToProfile() {
        if (user) {
            window.location.href = `/user/${user.id}`;
        }
        else {
            setPopupLogin();
        }
    }

    function goToTrips() {
        if (user) {
            window.location.href = `/trips`;
        }
        else {
            setPopupLogin();
        }
    }

    function goToMessages() {
        if (user) {
            window.location.href = `/messages`;
        }
        else {
            setPopupLogin();
        }
    }

    function returnDropdown(){
        return <div className="user-dropdown">
            <div className="user-dropdown-item" onClick={goToProfile}>Profile</div>
            <div className="user-dropdown-item" onClick={goToMessages}>Messages</div> 
            {isAuthenticated() && <div className="user-dropdown-item" onClick={goToTrips}>Trips</div>}
            <div className="user-dropdown-divider"></div>
            {isAuthenticated() ?
                <div className="user-dropdown-item" onClick={logOut}>Log Out</div>
                :
                <div className="user-dropdown-item" onClick={setPopupLogin}>Log In</div>
            }
            {!isAuthenticated() ?
                <div className="user-dropdown-item" onClick={setPopupLogin}>Sign up</div>
                :
                null
            }
        </div>
    }
    
    function userAvatar(){
        console.log("USER AVATAR");
        console.log(user);
        if(!user) return <DefaultAvatar/>
        if (user!.avatar){
            return <img src={user!.avatar.url} alt="user avatar" className="user-avatar-image"/>
        } else
        {
            return <DefaultAvatar/>
        }
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
                {userAvatar()}
            </div>
        </div>
        {isDropdownVisible ? returnDropdown() : null}
    </>
}

export default UserProfileDropdown