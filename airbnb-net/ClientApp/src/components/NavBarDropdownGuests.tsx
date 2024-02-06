import React from "react";

interface NavBarDropdownGuestsProps {
    active: boolean;
    guests: number;
    setGuests: (guests: number) => void;
}

export const NavBarDropdownGuests = (props : NavBarDropdownGuestsProps) => {
    return props.active? <div className={"nav-dropdown right-40 nav-guests"}>
        Guests
        <div className="guest-counter">
            <button className="guest-counter-button" onClick={() => props.setGuests(props.guests-1)} disabled={props.guests==0}>-</button>
            <div className="guest-counter-number">{props.guests}</div>
            <button className="guest-counter-button" onClick={() => props.setGuests(props.guests+1)} disabled={props.guests==4}>+</button>
        </div>
    </div>: <></>;
};