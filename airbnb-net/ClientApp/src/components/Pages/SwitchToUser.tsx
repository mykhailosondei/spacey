import {ReactNode, useEffect, useMemo, useState} from "react";
import {AuthenticationState, useAuthState} from "../../Contexts/AuthStateProvider";
import {AuthService} from "../../services/AuthService";
import {useUser} from "../../Contexts/UserContext";
import {UserService} from "../../services/UserService";

export const SwitchToUser = (props: { children: ReactNode }) => {

    const {authenticationState, setAuthenticationState} = useAuthState();
    const authService = useMemo(() => AuthService.getInstance(), []);
    const userService = useMemo(() => UserService.getInstance(), []);
    const {setUser} = useUser();
    const [isSwitching, setIsSwitching] = useState<boolean>(true);

    useEffect(() => {
        if(authenticationState === AuthenticationState.AuthenticatedHost){
            console.log("I AM HERE");
            authService.switchToUser().then((result) => {
                console.log(result);
                if(result){
                    console.log("Switched to user");
                    setUser(result.data.user);
                    setAuthenticationState(AuthenticationState.AuthenticatedUser);
                    setIsSwitching(false);
                }else {
                    console.log("Failed to switch to user")
                    setIsSwitching(false);
                }
            });
        }else if (authenticationState === AuthenticationState.AuthenticatedUser){
            userService.getFromToken().then((user) => {
                setUser(user.data);
                setIsSwitching(false);
            });
        }else {
            setIsSwitching(false);
        }
    }, []);
    
    return <>
         {!isSwitching?props.children:null}
    </>;
}