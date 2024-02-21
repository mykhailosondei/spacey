import React, {createContext, useContext, useEffect, useMemo, useState} from "react";
import AuthUser from "../DTOs/User/AuthUser";
import {HostDTO} from "../DTOs/Host/HostDTO";
import {UserDTO} from "../DTOs/User/UserDTO";
import {AuthenticationState, useAuthState} from "./AuthStateProvider";
import {UserService} from "../services/UserService";


const UserContext = createContext<{ user: UserDTO | null, 
                                                setUser: React.Dispatch<React.SetStateAction<UserDTO | null>> }>
                                                ({ user: null,
                                                            setUser: () => {}});

export function useUser() {
    const context = useContext(UserContext);
    if (context === undefined) {
        throw new Error("useUser must be used within a UserProvider");
    }
    return context;
}


export function UserProvider(props: any) {
    const [user, setUser] = useState<UserDTO | null>(null);
    const userService = useMemo(() => {return UserService.getInstance()}, []);
    const {authenticationState} = useAuthState();
    
    const value = useMemo(() => ( {
        user,
        setUser
    }), [user]);
    
    useEffect(() => {
        if(authenticationState === AuthenticationState.AuthenticatedUser) {
            userService.getFromToken().then((user) => {
                setUser(user.data);
            });
        }
    }, [authenticationState]);
    
    return (
        <UserContext.Provider value={value}>
            {props.children}
        </UserContext.Provider>
    );
}