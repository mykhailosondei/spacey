import React from "react";
import "../styles/BookingManager.scss";
import {BookingDTO} from "../DTOs/Booking/BookingDTO";

interface ActionButton {
    name: string,
    amount: number
}

interface BookingManagerProps {
    bookings: BookingDTO[]
}

export const BookingManager : React.FC<BookingManagerProps> = (props : BookingManagerProps) => {
    let actionButtons : ActionButton[] = [{name: "Upcoming", amount: 0}, {name: "Current", amount: 0}, {name: "Past", amount: 0}];
    const [currentAction, setCurrentAction] = React.useState(0);
    
    return <>
        <div className={"booking-manager"}>
            <div className="action-buttons">
                {actionButtons.map((button, index) => {
                    return <button className={"action-button " + (index === currentAction ? "action-button-selected" : "")}
                                   onClick={()=>setCurrentAction(index)}>
                        <span>{button.name} ({button.amount})</span>
                    </button>
                })}
            </div>
            <div className="booking-container">
                {props.bookings.map((booking) => {
                    return <></>;
                })}
            </div>
        </div>
    </>
}