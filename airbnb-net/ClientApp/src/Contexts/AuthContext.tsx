import React, {createContext, useContext, useState} from "react";
import AuthUser from "../DTOs/User/AuthUser";

const AuthContext = createContext<{authUser: AuthUser | null, setAuthUser: React.Dispatch<React.SetStateAction<AuthUser | null>>, isAuthenticated: boolean, setIsAuthenticated: React.Dispatch<React.SetStateAction<boolean>> }|undefined>(undefined);

export function useAuth() {
    return useContext(AuthContext);
}

export function AuthProvider(props: any) {
    const [authUser, setAuthUser] = useState<AuthUser | null>(null);
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
    
    const value = {
        authUser,
        setAuthUser,
        isAuthenticated,
        setIsAuthenticated
    };
    
    return (
        <AuthContext.Provider value={value}>
            {props.children}
        </AuthContext.Provider>
    );
}