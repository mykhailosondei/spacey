import React from "react";
import "../styles/CalendarSelectorPopup.scss";
import {useSelectedDates} from "../Contexts/SelectedDatesProvider";
import {useCalendarSelectorPopup} from "../Contexts/CalendarSelectorPopupProvider";
import {CalendarSelector} from "./CalendarSelector";
import ListingDTO from "../DTOs/Listing/ListingDTO";

interface CalendarSelectorPopupProps{
    listing: ListingDTO;
}

export const CalendarSelectorPopup = (props:CalendarSelectorPopupProps) => {
    
    const {startDate, setStartDate, endDate, setEndDate} = useSelectedDates();
    
    const {isOpen, setIsOpen} = useCalendarSelectorPopup();
    
    function closeDateSelector() {
        console.log("closeDateSelector: ", isOpen);
        setIsOpen(false);
    }

    function clearDates() {
        setStartDate(null);
        setEndDate(null);
    }

    return <div>
        <div className="calendar-selector-popup-background" onClick={closeDateSelector}></div>
        <div className="calendar-selector-popup">
            <div className="header-and-date-range">
                <div className="headers">
                    <div style={{fontWeight: 500, fontSize: "22px", marginBottom: "16px"}}>Select dates</div>
                    <div className="subheader">Add your travel dates</div>
                </div>
                <div className="bw-dates" style={{width: "310px"}}>
                    <div className="check-in-input">
                        <div className="date-label">Check-in</div>
                        <input className={"date-input"} value={startDate?.toLocaleDateString()} />
                    </div>
                    <div className="check-out-input">
                        <div className="date-label">Checkout</div>
                        <input className={"date-input"} value={endDate?.toLocaleDateString()} />
                    </div>
                </div>
            </div>
            <div className="calendar-selector-section">
                <CalendarSelector listing={props.listing}></CalendarSelector>
            </div>
            <div className="bottom-actions">
                <div className="cancel-button" onClick={closeDateSelector}>Close</div>
            </div>
        </div>
    </div>;
};