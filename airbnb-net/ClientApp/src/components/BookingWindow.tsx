import "../styles/BookingWindow.scss";
import React from "react";
import {useSelectedDates} from "../Contexts/SelectedDatesProvider";
import {CalendarSelectorPopup} from "./CalendarSelectorPopup";
import {useCalendarSelectorPopup} from "../Contexts/CalendarSelectorPopupProvider";
import ListingDTO from "../DTOs/Listing/ListingDTO";
import {Link, Navigate, useNavigate} from "react-router-dom";
import {AuthenticationState, useAuthState} from "../Contexts/AuthStateProvider";
import loginPopup from "./LoginPopup";
import {PopupType, usePopup} from "../Contexts/PopupContext";

interface BookingWindowProps {
    listing: ListingDTO;
}

export const BookingWindow = (props:BookingWindowProps) => {
    
    const {isOpen, setIsOpen} = useCalendarSelectorPopup();
    
    const {startDate, endDate} = useSelectedDates();
    
    const {setPopupType} = usePopup();
    
    const navigate = useNavigate();
    
    const {authenticationState} = useAuthState();
    
    const [numberOfGuests, setNumberOfGuests] = React.useState<number>(1);
    
    function openDateSelector() {
        console.log("openDateSelector: ", isOpen);
        setIsOpen(true);
    }
    
    function amountOfNights() {
        if(startDate && endDate) {
            return Math.floor((endDate.getTime() - startDate.getTime())/(1000*60*60*24));
        }
        return 0;
    }
    
    function netTotal(multiplier: number = 1) {
        if(startDate && endDate) {
            const string = (props.listing.pricePerNight * amountOfNights() * multiplier).toString();
            return string.substring(0, string.indexOf('.') === -1 ? 8 : string.indexOf('.') + 3);
        }
        return "0";
    }
    
    function Total() {
        return netTotal(1.15);
    }
    
    
    const isUserLoggedIn = ()=> authenticationState === AuthenticationState.AuthenticatedUser;

    function handleReserve() {
        if(isUserLoggedIn()) {
            navigate(`/booking?checkIn=${startDate!.toLocaleDateString()}&checkOut=${endDate!.toLocaleDateString()}&guests=${numberOfGuests}&listingId=${props.listing.id}`);
        }else {
            setPopupType(PopupType.LOGIN);
        }
    }

    return <>
        <div className="booking-window-space">
            <div className="booking-window">
                <div className="bw-price">
                    <span className={"bw-price-amount"}>${props.listing.pricePerNight} CAD</span>
                    <span>night</span>
                </div>
                <div className="bw-dates-and-guests">
                    {isOpen ? <CalendarSelectorPopup listing={props.listing}></CalendarSelectorPopup> : null}
                    <div className="bw-dates">
                        <div className="check-in-input">
                            <div className="date-label">Check-in</div>
                            <input className={"date-input"} value={startDate?.toLocaleDateString()} disabled/>
                        </div>
                        <div className="check-out-input">
                            <div className="date-label">Checkout</div>
                            <input className={"date-input"} value={endDate?.toLocaleDateString()} disabled/>
                        </div>
                        <div className="bw-dates-overlay" onClick={openDateSelector}></div>
                    </div>
                    <div className="bw-guests">
                        <div className={"dropdown-items"}>
                            <div className="date-label">Guests</div>
                            <div className="guests-input">{numberOfGuests} guest</div>
                        </div>
                        <div className="guest-counter">
                            <button className="guest-counter-button" onClick={() => setNumberOfGuests(numberOfGuests-1)} disabled={numberOfGuests==1}>-</button>
                            <button className="guest-counter-button" onClick={() => setNumberOfGuests(numberOfGuests+1)} disabled={numberOfGuests==4}>+</button>
                        </div>
                        
                    </div>
                </div>
                {endDate && startDate ? <div onClick={handleReserve} className="bw-button">Reserve</div> : <div className="bw-button" onClick={openDateSelector}>Select dates</div>}
                <div className="bw-remark">You won't be charged yet.</div>
                {endDate && startDate ? <div>
                    <div className="bw-calculations">
                        <div className="bw-calculation">
                            <div
                                className="calculation-value">${props.listing.pricePerNight} x {amountOfNights()} nights
                            </div>
                            <div className="calculation-total">${netTotal()}</div>
                        </div>
                        <div className="bw-calculation">
                            <div className="calculation-value">Cleaning fee</div>
                            <div className="calculation-total">${netTotal(0.1)}</div>
                        </div>
                        <div className="bw-calculation">
                            <div className="calculation-value">Service fee</div>
                            <div className="calculation-total">${netTotal(0.15)}</div>
                        </div>
                    </div>
                    <div className="horizontal-divider"></div>
                    <div className="bw-calculation">
                        <div className="calculation-value calc-total">Total</div>
                        <div className="calculation-total calc-total">${Total()}</div>
                    </div>
                </div> : <></>}
            </div>
        </div>
    </>;
};