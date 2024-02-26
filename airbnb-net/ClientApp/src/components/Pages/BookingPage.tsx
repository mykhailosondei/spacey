import React, {useEffect, useMemo} from "react";
import {useLocation} from "react-router-dom";
import NavBar from "../NavBar";
import "../../styles/BookingPage.scss";
import {ListingService} from "../../services/ListingService";
import ListingDTO from "../../DTOs/Listing/ListingDTO";
import {UserService} from "../../services/UserService";
import {PropertyType} from "../../values/PropertyType";
import LeftArrow from "../Icons/LeftArrow";
import {BookingService} from "../../services/BookingService";

export function convertDateToUTC(date:Date) { return new Date(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds()); }

export const BookingPage = () => {
    
    const location = useLocation();
    const urlParams = new URLSearchParams(location.search);
    const listingId = urlParams.get('listingId');
    const startDate = convertDateToUTC(new Date(urlParams.get('checkIn')?.toString() || ""));
    const endDate = convertDateToUTC(new Date(urlParams.get('checkOut')?.toString() || ""));
    const numberOfGuests = urlParams.get('guests');
    
    const [listing, setListing] = React.useState<ListingDTO | null>(null);
    
    const [hostName, setHostName] = React.useState<string>("");
    
    const [bookingButtonState, setBookingButtonState] = React.useState<"untouched"|"in-process"|"successful"|"unsuccessful">("untouched");
    
    
    const bookingService = useMemo(() => {return BookingService.getInstance()}, []);
    const listingService = useMemo(() => {return ListingService.getInstance()}, []);
    const userService = useMemo(() => {return UserService.getInstance()}, []);

    useEffect(() => {
        if(listingId) {
            listingService.get(listingId).then((listing) => {
                setListing(listing.data);
                userService.get(listing.data.host.userId).then((user) => {
                    setHostName(user.data.name);
                });
            });
        }
    }, []);
    
    function displayDates(startDate: Date, endDate: Date) {
        if(!(startDate && endDate)) return;
        if (startDate.getMonth() === endDate.getMonth() && startDate.getFullYear() === endDate.getFullYear()) {
            const month = startDate.toLocaleDateString("eng", {month: "short"});
            return `${month} ${startDate.getDate()} - ${endDate.getDate()}`;
        }else{
            const start = startDate.toLocaleDateString("eng", {dateStyle: "medium"});
            const end = endDate.toLocaleDateString("eng", {dateStyle: "medium"});
            return `${start} - ${end}`;
        }
    }

    function amountOfNights() {
        if(startDate && endDate) {
            return Math.floor((endDate.getTime() - startDate.getTime())/(1000*60*60*24));
        }
        return 0;
    }

    function netTotal() {
        if(startDate && endDate && listing) {
            return listing.pricePerNight * amountOfNights();
        }
        return 0;
    }

    function Total() {
        return Math.floor(netTotal()) + Math.floor(netTotal() * 0.1) + Math.floor(netTotal() * 0.05);
    }
    
    function goBack() {
        window.history.back();
    }

    function handleConfirmClick() {
        if(listingId && startDate && endDate && numberOfGuests) {
            setBookingButtonState("in-process");
            bookingService.create({checkIn:startDate, checkOut:endDate, listingId:listingId,numberOfGuests:parseInt(numberOfGuests)}).then((response) => {
                if(response.status === 200) {
                    setBookingButtonState("successful");
                }else{
                    setBookingButtonState("unsuccessful");
                }
            });
        }
    }

    return <div>
        <div className="booking-navbar-wrapper"><NavBar></NavBar></div>
        <div className={"booking-page"}>
            <div className="confirmation-slider">
                <h1 className="header-large">Confirm
                    <div className="arrow-back" onClick={goBack}>
                        <LeftArrow></LeftArrow>
                    </div>
                </h1>
                <div className="booking-details">
                    <h2 className="header-medium">Your trip</h2>
                    <div className="booking-details-list">
                        <div className="booking-detail">
                            <div className="header-small">Dates</div>
                            <div className="booking-detail-value">{displayDates(startDate, endDate)}</div>
                        </div>
                        <div className="booking-detail">
                            <div className="header-small">Guests</div>
                            <div
                                className="booking-detail-value">{numberOfGuests} guest{parseInt(numberOfGuests!) >= 1 ? "s" : ""}</div>
                        </div>
                    </div>
                </div>
                <div className="horizontal-divider"></div>
                <div className="ground-rules">
                    <h1 className={"header-medium"}>Ground rules</h1>
                    <p>We ask every guest to remember a few simple things about what makes a great guest.</p>
                    <ul>
                        <li>Follow the house rules</li>
                        <li>Treat your Host’s home like your own</li>
                    </ul>
                </div>
                <div className="horizontal-divider"></div>
                <div>
                    <div className={`bw-button custom-width ${bookingButtonState}`} onClick={handleConfirmClick}>
                        {bookingButtonState === "untouched" && <span>Confirm and book</span>}
                        {bookingButtonState === "in-process" && <span>Booking...</span>}
                        {bookingButtonState === "successful" && <span>Confirmed</span>}
                        {bookingButtonState === "unsuccessful" && <span>Unsuccessful</span>}
                    </div>
                </div>
            </div>
            {listing && <div className="bp-listing-details-card-space">
                <div className="bp-listing-details-card">
                    <div className="bp-listing-info-section">
                        <div className="image-section">
                            <img className="listing-image" src={listing.imagesUrls[0].url} alt=""/>
                        </div>
                        <div className="bp-listing-info">
                            <div className="bp-listing-type">{PropertyType[listing.propertyType]}</div>
                            <div className="bp-listing-title">{listing.title}</div>
                            <div className="bp-listing-host">Hosted by {hostName}</div>
                        </div>
                    </div>
                    <div className="horizontal-divider"></div>
                    <div className="price-details">
                        <div className="price-header">Price details</div>
                        <div className="bw-calculations">
                            <div className="bw-calculation">
                                <div
                                    className="calculation-value">${listing.pricePerNight} x {amountOfNights()} nights
                                </div>
                                <div className="calculation-total">${netTotal()}</div>
                            </div>
                            <div className="bw-calculation">
                                <div className="calculation-value">Cleaning fee</div>
                                <div className="calculation-total">${netTotal() * 0.1}</div>
                            </div>
                            <div className="bw-calculation">
                                <div className="calculation-value">Service fee</div>
                                <div className="calculation-total">${netTotal() * 0.05}</div>
                            </div>
                        </div>
                        <div className="horizontal-divider"></div>
                        <div className="bw-calculation">
                            <div className="calculation-value calc-total">Total</div>
                            <div className="calculation-total calc-total">${Total()}</div>
                        </div>
                    </div>
                </div>
            </div>}
        </div>
    </div>;
};