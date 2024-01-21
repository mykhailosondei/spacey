import React, {createContext, useContext, useEffect, useMemo, useState} from "react";
import {HostDTO} from "../DTOs/Host/HostDTO";
import {HostService} from "../services/HostService";
import {AuthenticationState, useAuthState} from "./AuthStateProvider";
import {UserService} from "../services/UserService";

const HostContext = createContext<{ host: HostDTO | null, setHost: React.Dispatch<React.SetStateAction<HostDTO | null>> }>({ host: null, setHost: () => {} });

export function useHost() {
    const context = useContext(HostContext);
    if (context === undefined) {
        throw new Error("useHost must be used within a HostProvider");
    }
    
    return context;
}

export function HostProvider(props: any) {
    const [host, setHost] = useState<HostDTO | null>(null);
    const hostService = useMemo(() => {return HostService.getInstance()}, []);
    const userService = useMemo(() => {return UserService.getInstance()}, []);
    
    const {authenticationState} = useAuthState();
    
    const value = useMemo(() => ({
        host,
        setHost
    }), [host]);

    return (
        <HostContext.Provider value={value}>
            {props.children}
        </HostContext.Provider>
    );
}