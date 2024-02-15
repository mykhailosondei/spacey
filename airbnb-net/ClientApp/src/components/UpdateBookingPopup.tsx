import {BookingDTO} from "../DTOs/Booking/BookingDTO";
import Cross from "./Icons/Cross";
import React, {useMemo} from "react";
import InputField from "./InputFIeld";
import {convertDateToUTC} from "./Pages/BookingPage";
import {BookingUpdateDTO} from "../DTOs/Booking/BookingUpdateDTO";
import {BookingService} from "../services/BookingService";

interface UpdateBookingPopupProps {
    booking: BookingDTO;
    onClose: () => void;
}

export const UpdateBookingPopup = (props: UpdateBookingPopupProps) => {
    
    const [checkIn, setCheckIn] = React.useState<string>(convertDateToUTC(new Date(props.booking.checkIn)).toLocaleDateString("en-US", {dateStyle: "short"}));
    const [checkOut, setCheckOut] = React.useState<string>(convertDateToUTC(new Date(props.booking.checkOut)).toLocaleDateString("en-US", {dateStyle: "short"}));
    const [numberOfGuests, setNumberOfGuests] = React.useState<string>(props.booking.numberOfGuests?.toString() || "");
    const [updateState, setUpdateState] = React.useState<"untouched"|"in-process"|"successful"|"unsuccessful">("untouched");
    
    const bookingService = useMemo(() => {return BookingService.getInstance()}, []);
    
    const isCheckValid = (value : string) => {
        return !isNaN(Date.parse(value)) && Date.parse(value) > Date.now() && new RegExp(/^\d{1,2}\/\d{1,2}\/(\d{4})|(\d{2})$/).test(value);
    }
    
    const isNumberOfGuestsValid = (value : string) => {
        return !isNaN(parseInt(value)) && parseInt(value) > 0 && parseInt(value) < 5;
    }
    
    const updateBooking = () => {
        if(isCheckValid(checkIn) && isCheckValid(checkOut) && isNumberOfGuestsValid(numberOfGuests)) {
            const newBooking : BookingUpdateDTO = {
                userId: props.booking.userId,
                listingId: props.booking.listingId,
                checkIn: convertDateToUTC(new Date(checkIn)),
                checkOut: convertDateToUTC(new Date(checkOut)),
                numberOfGuests: parseInt(numberOfGuests)
            }
            setUpdateState("in-process");
            bookingService.update(props.booking.id, newBooking).then((result) => {
                if (result.status === 200) {
                    setUpdateState("successful");
                }else{
                    setUpdateState("unsuccessful");
                }
            });
        }
    }
    
    return <div>
        <div className={"update-popup-background"} onClick={props.onClose}></div>
        <div className={"update-popup"}>
            <div className="up-header margin-b20">
                    <span onClick={props.onClose}>
                        <Cross/>
                    </span>
                <h3>Update the booking</h3>
            </div>
            <div className="up-body">
                <div className="up-form">
                    <div className="up-form-row margin-t10">
                        <InputField type={"text"} label={"Check-in"} value={checkIn} onChange={(e) => {setCheckIn(e.target.value)}} isValid={(value)=>isCheckValid(value)} onInvalid={() => {}}/>
                    </div>
                    <div className="up-form-row margin-t10">
                        <InputField type={"text"} label={"Check-out"} value={checkOut} onChange={(e) => {setCheckOut(e.target.value)}} isValid={(value)=>isCheckValid(value)} onInvalid={() => {}}/>
                    </div>
                    <div className="up-form-row margin-t10">
                        <InputField type={"text"} label={"Number of guests"} value={numberOfGuests} onChange={(e) => {setNumberOfGuests(e.target.value)}} isValid={(value)=>isNumberOfGuestsValid(value)} onInvalid={() => {}}/>
                    </div>
                </div>
                <div className="up-footer">
                    {updateState === "unsuccessful" && <div className="error-message">Something went wrong</div>}
                    <div className="bw-button custom-width margin-t10" onClick={updateBooking}>
                        <span>Update</span>
                    </div>
                </div>
            </div>
        </div>
    </div>;
};