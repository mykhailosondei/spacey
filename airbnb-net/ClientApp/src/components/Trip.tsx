import "../styles/Trip.scss";
import {BookingDTO} from "../DTOs/Booking/BookingDTO";
import React, {useEffect, useMemo, useState} from "react";
import {BookingService} from "../services/BookingService";
import {ListingService} from "../services/ListingService";
import ListingDTO from "../DTOs/Listing/ListingDTO";
import {PropertyType} from "../values/PropertyType";
import {HostService} from "../services/HostService";
import {UserService} from "../services/UserService";

interface TripProps {
    bookingId: string;
}

export const Trip = (props: TripProps) => {
    
    const [booking, setBooking] = useState<BookingDTO | null>(null);
    const [listing, setListing] = useState<ListingDTO | null>(null);
    const [userName, setUserName] = useState<string>("");
    
    const bookingService = useMemo(() => {return BookingService.getInstance()}, []);
    const listingService = useMemo(() => {return ListingService.getInstance()}, []);
    const userService = useMemo(() => {return UserService.getInstance()}, []);

    useEffect(() => {
        bookingService.get(props.bookingId).then((result) => {
            setBooking(result);
            listingService.get(result.listingId).then((listingResult) => {
                setListing(listingResult);
                userService.get(listingResult.host.userId).then((host) => {
                    setUserName(host.name);
                });
            });
        });
    }, []);
    
    function monthRange() {
        if(booking?.checkIn && booking?.checkOut) {
            const start = new Date(booking.checkIn);
            const end = new Date(booking.checkOut);
            if (start.getMonth() === end.getMonth() && start.getFullYear() === end.getFullYear()) {
                const month = start.toLocaleDateString("eng", {month: "short"});
                return `${month}`;
            }else{
                const startMonth = start.toLocaleDateString("eng", {month: "short"});
                const endMonth = end.toLocaleDateString("eng", {month: "short"});
                return `${startMonth} - ${endMonth}`;
            }
        }
    }
    
    function dateRange() {
        if(booking?.checkIn && booking?.checkOut) {
            const start = new Date(booking.checkIn);
            const end = new Date(booking.checkOut);
            if (start.getMonth() === end.getMonth() && start.getFullYear() === end.getFullYear()) {
                return `${start.getDate()} - ${end.getDate()}`;
            }else{
                const startMonth = start.toLocaleDateString("eng", {month: "short"});
                const endMonth = end.toLocaleDateString("eng", {month: "short"});
                return `${startMonth} ${start.getDate()} - ${endMonth} ${end.getDate()}`;
            }
        }
    }
    
    function yearRange() {
        if(booking?.checkIn && booking?.checkOut) {
            const start = new Date(booking.checkIn);
            const end = new Date(booking.checkOut);
            if (start.getFullYear() === end.getFullYear()) {
                return `${start.getFullYear()}`;
            }else{
                return `${start.getFullYear()} - ${end.getFullYear()}`;
            }
        }
    }

    function timeLeft() {
        if(booking?.checkOut) {
            const end = new Date(booking.checkOut);
            const now = new Date();
            const diff = end.getTime() - now.getTime();
            const days = Math.floor(diff / (1000 * 60 * 60 * 24));
            if(days < 0) {
                return `Trip ended`;
            }
            return `${days} days left`;
        }
    }

    return (listing && booking && userName) ? <div className="trip">
        <div className="trip-info">
            <div className="flex-top-part">
                <div className="header-large">{listing?.address.city}</div>
                <div className={"reg-text-small"}>{PropertyType[listing!.propertyType]} hosted by {userName}</div>
            </div>
            <div className="horizontal-divider"></div>
            <div className="flex-bottom-part">
                <div className="flex-left-part">
                    <div className="tp-dates">
                        <div className="reg-text-med">{monthRange()}</div>
                        <div className="reg-text-med">{dateRange()}</div>
                        <div className="reg-text-small">{yearRange()}</div>
                    </div>
                </div>
                
                <div className="vertical-divider"></div>
                <div className="flex-right-part">
                    <div className="reg-text-med">{listing.address.street}</div>
                    <div className="reg-text-med">{listing.address.city}</div>
                    <div className="reg-text-small">{listing.address.country}</div>
                </div>
            </div>
        </div>
        <div className="tp-image-section">
            <img src="https://placehold.co/400x400" alt="Joshua Tree"></img>
            <div className="days-left">{timeLeft()}</div>
        </div>
    </div> : <></>;
};