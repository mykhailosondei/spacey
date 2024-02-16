import {useUser} from "../Contexts/UserContext";
import "../styles/HostingMainSection.scss";
import {BookingManager} from "./BookingManager";
import {useHost} from "../Contexts/HostContext";
import {useEffect} from "react";

export const HostingMainSection = () => {
    
    const {user} = useUser();
    const {host} = useHost();

    useEffect(() => {
        console.log("host: ", host);
    }, []);
    
    return (host && user) ? <>
        <div className={"main-section"}>
            <h1 className="greeting">Welcome back, {user?.name}</h1>
            <div className="finishing-listings-section"></div>
            <div className="bookings-section">
                <h2 className="section-title">Bookings</h2>
                <BookingManager host={host}></BookingManager>
            </div>
            <div className="resources-section"></div>
        </div>
    </> : <></>;
}