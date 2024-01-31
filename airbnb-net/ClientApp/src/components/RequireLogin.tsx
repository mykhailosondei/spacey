import {useAuthState} from "../Contexts/AuthStateProvider";
import {Navigate} from "react-router-dom";

export const RequireLogin = ({children}: { children: JSX.Element }) => {
    
    const {authenticationState} = useAuthState();

    console.log("authenticationState: ", authenticationState);
    
    if (authenticationState === 0) {
        return <Navigate to={"/login"} state={{from: window.location.pathname}} replace={true}/>;
    }
    
    return children;
};