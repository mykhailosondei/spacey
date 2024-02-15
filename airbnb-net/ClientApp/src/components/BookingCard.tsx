import React from "react";
import "../styles/BookingCard.scss";
import {ReactComponent as DefaultAvatar} from "../values/svgs/default-profile.svg";
import {BookingDTO} from "../DTOs/Booking/BookingDTO";
import {UserDTO} from "../DTOs/User/UserDTO";
import {UserService} from "../services/UserService";
import {PropertyType} from "../values/PropertyType";
import {BookingStatus} from "../values/BookingStatus";

interface BookingCardProps {
    booking: BookingDTO;
}

export const BookingCard = (props: BookingCardProps) => {
    
    const [guest, setGuest] = React.useState<UserDTO>();
    const userService = React.useMemo(() => {return UserService.getInstance()}, []);
    
    React.useEffect(() => {
        userService.get(props.booking.userId).then((user) => {
            setGuest(user.data);
        });
    }, []);
    
    const daysLeftToCheckIn = () => {
        const today = new Date();
        const checkInDate = new Date(props.booking.checkIn);
        const diffTime = checkInDate.getTime() - today.getTime();
        const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
        if(diffDays < 0) {
            return "Passed";
        }
        else
        return `Arriving in ${diffDays} day${diffDays > 1 ? "s" : ""}`;
    }
    
    const guests = () => {
        return `${props.booking.numberOfGuests} guest${props.booking.numberOfGuests && props.booking.numberOfGuests > 1 ? "s" : ""}`;
    }
    
    return <div className={"booking-card"}>
        <div className="bc-body">
            <div className="bc-header-lines">
                <span>{daysLeftToCheckIn()}</span>
                <span>{guests()}</span>
            </div>
            <div className={"bc-guest-info"}>
                {guest && <div className="bc-info-lines">
                    <span>{guest.name}</span>
                    {props.booking.status !== BookingStatus.Cancelled ? <span>{guest.phoneNumber}</span> : <span style={{color:"red"}}>Cancelled</span>}
                </div>}
                <div className="bc-avatar">
                    <DefaultAvatar/>
                </div>
            </div>
        </div>
        {props.booking.status !== BookingStatus.Cancelled && <div className="bc-footer">
            <div className="bc-message">Message</div>
            <div className="bc-call">Call</div>
        </div>}
    </div>;
};