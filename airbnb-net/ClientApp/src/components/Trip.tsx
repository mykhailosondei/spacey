import "../styles/Trip.scss";
import {BookingDTO} from "../DTOs/Booking/BookingDTO";
import React, {useEffect, useMemo, useState} from "react";
import {BookingService} from "../services/BookingService";
import {ListingService} from "../services/ListingService";
import ListingDTO from "../DTOs/Listing/ListingDTO";
import {PropertyType} from "../values/PropertyType";
import {UserService} from "../services/UserService";
import Trash from "./Icons/Trash";
import {ReviewActionPopup} from "./ReviewActionPopup";
import {ReviewService} from "../services/ReviewService";
import {BookingStatus} from "../values/BookingStatus";
import {UpdateBookingPopup} from "./UpdateBookingPopup";

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
    const reviewService = useMemo(() => {return ReviewService.getInstance()}, []);

    const [actionPopupVisible, setActionPopupVisible] = useState<boolean>(false);
    const [bookingUpdatePopupVisible, setBookingUpdatePopupVisible] = useState<boolean>(false);
    const [reviewDeletionInProgress, setReviewDeletionInProgress] = useState<boolean>(false);
    const [bookingCancellationInProgress, setBookingCancellationInProgress] = useState<boolean>(false);
    
    useEffect(() => {
        bookingService.get(props.bookingId).then((result) => {
            setBooking(result.data);
            listingService.get(result.data.listingId).then((listingResult) => {
                setListing(listingResult.data);
                userService.get(listingResult.data.host.userId).then((host) => {
                    setUserName(host.data.name);
                });
            });
        });
    }, [actionPopupVisible, reviewDeletionInProgress, bookingCancellationInProgress]);
    
    const reviewExists = () : boolean => {
        console.log(!!booking?.review);
        return !!booking?.review;
    }
    
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
        if (booking?.status !== BookingStatus.Active) return "Booking cancelled";
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

    function openPopup() {
        setActionPopupVisible(true);   
    }
    
    function closePopup() {
        setActionPopupVisible(false);
        setBookingUpdatePopupVisible(false);
    }

    function deleteReview() {
        setReviewDeletionInProgress(true);
        if(booking?.review) {
            reviewService.delete(booking?.review.id).then(() => {
                setReviewDeletionInProgress(false);
            });
        }
    }
    
    function cancelBooking() {
        setBookingCancellationInProgress(true);
        if(booking) {
            bookingService.cancel(booking.id).then(() => {
                setBookingCancellationInProgress(false);
            });
        }
    }
    
    function updateBooking() {
        setBookingUpdatePopupVisible(true);
    }

    function getUpdateMode() {
        return reviewExists();
    }

    return (listing && booking && userName) ? <>
        {bookingUpdatePopupVisible &&
            <UpdateBookingPopup booking={booking} onClose={closePopup}></UpdateBookingPopup>}
        {actionPopupVisible &&
            <ReviewActionPopup bookingId={booking.id} reviewId={booking.review?.id} onClose={closePopup}
                               updateMode={getUpdateMode()}></ReviewActionPopup>}
        <div className="trip">
            <div className="action-review-container left-n15 flex-vertical">
                {booking.status === BookingStatus.Active &&
                    <div className="action-review-button" onClick={cancelBooking}>
                        Cancel Booking
                    </div>}
                <button className="action-review-button margin-t10" onClick={updateBooking}
                        disabled={booking.status !== BookingStatus.Active}>
                    Edit Booking
                </button>
            </div>
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
            <div className="action-review-container">
                {!reviewExists() ? <div className="action-review-button" onClick={openPopup}>
                        Create Review
                    </div> :
                    <>
                        <div className="action-review-button" onClick={openPopup}>
                            Edit Review
                        </div>
                        <div className="delete-review-button" onClick={deleteReview}>
                            <Trash/>
                        </div>
                    </>
                }
            </div>
        </div>
    </> : <></>;
};