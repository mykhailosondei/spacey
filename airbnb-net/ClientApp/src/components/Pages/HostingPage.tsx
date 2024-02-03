import {Outlet, Route, Routes, useLocation} from "react-router-dom";
import React, {useEffect, useMemo} from "react";
import {HostingNavBar} from "../HostingNavBar";
import {HostingInboxSection} from "../HostingInboxSection";
import {HostingListingsSection} from "../HostingListingsSection";
import {HostingMainSection} from "../HostingMainSection";
import {HostProvider, useHost} from "../../Contexts/HostContext";
import {useUser} from "../../Contexts/UserContext";
import {AuthService} from "../../services/AuthService";
import {HostService} from "../../services/HostService";

export const HostingPage = () => {
    let location = useLocation();
    let currentDestination = 0;
    if(location.pathname.includes("listings")) {
        currentDestination = 1;
    }
    else if(location.pathname.includes("inbox")) {
        currentDestination = 2;
    }
    
    const authService = useMemo(() => {return AuthService.getInstance()}, []);
    const hostService = useMemo(() => {return HostService.getInstance()}, []);
    
    const {setHost} = useHost();
    
    useEffect(() => {
        authService.switchToHost().then(()=>{
               hostService.getFromToken().then((host) => {setHost(host.data)});
            }
        )
    }, []);
    
    return <>
        <HostingNavBar destinations={[{name: "Today", url: "/hosting"},
            {name: "Listings", url: "/hosting/listings"},
            {name: "Inbox", url: "/hosting/inbox"}]}
                                      currentDestination={currentDestination}></HostingNavBar>
            <Routes>
                <Route path="" element={<HostingMainSection/>}/>
                <Route path="/listings" element={<HostingListingsSection/>}/>
                <Route path="/inbox" element={<HostingInboxSection/>}/>
            </Routes>
            <Outlet/>
    </>
}