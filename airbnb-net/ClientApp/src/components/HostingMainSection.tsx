import {useUser} from "../Contexts/UserContext";
import "../styles/HostingMainSection.scss";
import {BookingManager} from "./BookingManager";

export const HostingMainSection = () => {
    
    const {user} = useUser();
    
    return <>
        <div className={"main-section"}>
            <h1 className="greeting">Welcome back, {user?.name}</h1>
            <div className="finishing-listings-section"></div>
            <div className="bookings-section">
                <h2 className="section-title">Bookings</h2>
                <BookingManager bookings={[]}></BookingManager>
            </div>
            <div className="resources-section"></div>
        </div>
    </>;
}