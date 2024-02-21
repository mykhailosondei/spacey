import React, {useEffect} from "react";
import "../styles/BookingManager.scss";
import {BookingDTO} from "../DTOs/Booking/BookingDTO";
import {HostDTO} from "../DTOs/Host/HostDTO";
import {BookingService} from "../services/BookingService";
import {BookingStatus} from "../values/BookingStatus";
import {BookingCard} from "./BookingCard";
import NoBookings from "./Icons/NoBookings";

interface ActionButton {
    name: string,
    amount: number
}

interface BookingManagerProps {
    host: HostDTO
}

export const BookingManager : React.FC<BookingManagerProps> = (props : BookingManagerProps) => {
    
    const [actionButtons, setActionButtons] = React.useState<ActionButton[]>([{name: "Current", amount: 0}, {name: "Completed", amount: 0}, {name: "Cancelled", amount: 0}]);
    const [currentAction, setCurrentAction] = React.useState(0);
    const bookingService = React.useMemo(() => {return BookingService.getInstance()}, []);
    
    const [bookings, setBookings] = React.useState<BookingDTO[]>([]);
    
    useEffect(() => {
        let status : BookingStatus|null = null;
        switch (actionButtons[currentAction].name) {
            case "Current":
                status = BookingStatus.Active;
                break;
            case "Completed":
                status = BookingStatus.Completed;
                break;
            case "Cancelled":
                status = BookingStatus.Cancelled;
                break;
        }
        if(status == null) return;
        bookingService.getFromTokenByStatus(status).then((bookings) => {
            if (bookings.status === 200) {
                setBookings(bookings.data.filter((booking) => {return !passed(booking)}));
                setActionButtons((buttons) => {
                    buttons[currentAction].amount = bookings.data.filter((booking) => {return !passed(booking)}).length;
                    return buttons;
                });
            }
        });
        
    }, [currentAction]);
        
        const passed = (booking: BookingDTO) => {
            if(booking.status !== BookingStatus.Active) return false;
            const today = new Date();
            const checkInDate = new Date(booking.checkIn);
            const diffTime = checkInDate.getTime() - today.getTime();
            const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
            return diffDays < 0;
        }
        
    return <>
        <div className={"booking-manager"}>
            <div className="action-buttons">
                {actionButtons.map((button, index) => {
                    return <button className={"action-button " + (index === currentAction ? "action-button-selected" : "")}
                                   onClick={()=>setCurrentAction(index)}>
                        <span>{button.name} {button.amount != 0 ? `(${button.amount})` :""}</span>
                    </button>
                })}
            </div>
            <div className="booking-container">
                {bookings.length > 0 ? bookings.map((booking) => {
                    return <BookingCard booking={booking}/>;
                }) :
                    <div className="no-bookings"><NoBookings/> You don’t have any guests <br/> checking out today <br/> or tomorrow.</div>}
            </div>
        </div>
    </>
}