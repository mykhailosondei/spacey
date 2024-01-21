import React, {useEffect, useMemo} from "react";
import {UserService} from "../services/UserService";
import {HostService} from "../services/HostService";
import {AuthService} from "../services/AuthService";

export enum AuthenticationState {
    Unauthenticated,
    AuthenticatedUser,
    AuthenticatedHost
}

const AuthStateContext = React.createContext<{
    authenticationState: AuthenticationState,
    setAuthenticationState: React.Dispatch<React.SetStateAction<AuthenticationState>>
}>({
    authenticationState: AuthenticationState.Unauthenticated,
    setAuthenticationState: () => {}
});

export function useAuthState() {
    const context = React.useContext(AuthStateContext);
    if (context === undefined) {
        throw new Error("useAuthState must be used within a AuthStateProvider");
    }
    
    return context;
}

export function AuthStateProvider(props: any) {
    const [authenticationState, setAuthenticationState] = React.useState<AuthenticationState>(AuthenticationState.Unauthenticated);
    const authService = useMemo(() => {return AuthService.getInstance()}, []);
    const [isCheckingRole, setIsCheckingRole] = React.useState<boolean>(false);
    
    function setState(newState: AuthenticationState) {
        setIsCheckingRole(false);
        console.log("Setting state to: " + newState);
        setAuthenticationState(newState);
    }
    
    useEffect(() => {
        setIsCheckingRole(true);
        authService.isInRole("Host").then((result) => {
            if(result) {
                setState(AuthenticationState.AuthenticatedHost);
            }
            else {
                authService.isInRole("User").then((result) => {
                    if(result) {
                        setState(AuthenticationState.AuthenticatedUser);
                    }else {
                        setState(AuthenticationState.Unauthenticated);
                        localStorage.removeItem("token");
                    }
                });
            }
        });
    }, []);
    
    const value = useMemo(() => ({
        authenticationState,
        setAuthenticationState
    }), [authenticationState]);
    
    return (
        <AuthStateContext.Provider value={value}>
            {!isCheckingRole ? props.children : <></>}
        </AuthStateContext.Provider>
    );
}