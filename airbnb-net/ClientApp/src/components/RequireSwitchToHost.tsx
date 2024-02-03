import { useLocation, Navigate } from "react-router-dom";
import {AuthenticationState, useAuthState} from "../Contexts/AuthStateProvider";
import {useEffect, useMemo, useState} from "react";
import {UserService} from "../services/UserService";
import {HostService} from "../services/HostService";
import {AuthService} from "../services/AuthService";
import {useUser} from "../Contexts/UserContext";
import {useHost} from "../Contexts/HostContext";


export function RequireSwitchToHost({ children }: { children: JSX.Element }) {
    let { authenticationState } = useAuthState();
    console.log("authenticationState: ", authenticationState)
    let location = useLocation();
    
    const userService = useMemo(() => {return UserService.getInstance()}, []);
    const hostService = useMemo(() => {return HostService.getInstance()}, []);
    const authService = useMemo(() => {return AuthService.getInstance()}, []);
    const [isSwitching, setIsSwitching] = useState<boolean>(true);
    
    const {setUser} = useUser();
    const {setHost} = useHost();
    const {setAuthenticationState} = useAuthState();
    
    function switchToHostAndUpdateHostAndUser(){
        authService.switchToHost().then((result)=>{
            if(!result) {console.log("Error switching to host"); return;}

            setUser(result.data.user);
            hostService.getFromToken().then((host) => {
                setHost(host.data);
                setIsSwitching(false);
            });
            setAuthenticationState(AuthenticationState.AuthenticatedHost);
        });
    }
    
    function updateHostAndUser(){
        hostService.getFromToken().then((host) => {
            setHost(host.data);
            setIsSwitching(false);
            userService.getFromToken().then((user) => {
                setUser(user.data);
            });
        });
    }
    
    useEffect(() => {
        if(authenticationState == AuthenticationState.AuthenticatedUser) {
            switchToHostAndUpdateHostAndUser();
        }
        else if(authenticationState == AuthenticationState.AuthenticatedHost) {
            updateHostAndUser();
        }
    }, []);
    
    if ((authenticationState == AuthenticationState.Unauthenticated)) {
        return <Navigate to="/login" state={{ from: location }} replace />;
    } else {
        return (!isSwitching) ? children : <></>;
    }
}