import {Conversation} from "../DTOs/Conversation/Conversation";
import {ReactComponent as DefaultAvatar} from "../values/svgs/default-profile.svg";
import StarEmpty from "./Icons/StarEmpty";
import Logo from "./Icons/Logo";
import Exclude from "./Icons/Exclude";
import Luggage from "./Icons/Luggage";
import {useMemo, useState} from "react";
import {BookingDTO} from "../DTOs/Booking/BookingDTO";
import {HostDTO} from "../DTOs/Host/HostDTO";
import ListingDTO from "../DTOs/Listing/ListingDTO";
import {BookingService} from "../services/BookingService";
import {UserService} from "../services/UserService";
import {HostService} from "../services/HostService";
import {ListingService} from "../services/ListingService";
import {UserDTO} from "../DTOs/User/UserDTO";
import {BookingStatus} from "../values/BookingStatus";
import {Link} from "react-router-dom";

export const ConversationDetails = (props: { conversation?: Conversation, isHostMode? : boolean }) => {
    const [booking, setBooking] = useState<BookingDTO | null>(null);
    const [host, setHost] = useState<HostDTO | null>(null);
    const [listing, setListing] = useState<ListingDTO | null>(null);
    const [displayUser, setDisplayUser] = useState<UserDTO | null>(null);
    
    const bookingService = useMemo(() => {return BookingService.getInstance()}, []);
    const userService = useMemo(() => {return UserService.getInstance()}, []);
    const hostService = useMemo(() => {return HostService.getInstance()}, []);
    const listingService = useMemo(() => {return ListingService.getInstance()}, []);
    
    useMemo(() => {
        console.log(props.conversation);
        if(props.conversation) {
            bookingService.get(props.conversation.bookingId).then((response) => {
                if(response.status === 200)
                {
                    setBooking(response.data);
                    listingService.get(response.data.listingId).then((response) => {
                        if(response.status === 200)
                            setListing(response.data);
                    });
                }
            });
            hostService.get(props.conversation.hostId).then((response) => {
                if(response.status === 200){
                    setHost(response.data);
                    if(!props.isHostMode)
                    userService.get(response.data.userId).then((response) => {
                        if(response.status === 200)
                            setDisplayUser(response.data);
                    });
                }
            });
            if(props.isHostMode)
                userService.get(props.conversation.userId).then((response) => {
                    if(response.status === 200)
                        setDisplayUser(response.data);
                });
        }
    }, [props.conversation]);

    function smallDate(s: string) {
        return new Date(s).toLocaleDateString("en-US", {month: "short", day: "numeric"});
    }

    const getDates = () => {
        return `${smallDate(booking!.checkIn.toString())} - ${smallDate(booking!.checkOut.toString())}`;
    }
    
    const getTrips = () => {
        if (!displayUser?.bookingsIds || !displayUser?.bookingsIds.length) return "No trips yet";
        if (displayUser.bookingsIds.length === 1) return "1 trip";
        return `${displayUser.bookingsIds.length} trips`;
    }
    
    const getRating = () => {
        if(!host || !host.rating) return "No rating yet";
        return `${host.rating}`;
    }
    
    const getJoined = () => {
        if(!displayUser) return "No joined date";
        return `Joined Spacey in ${new Date(displayUser.createdAt).getFullYear()}`;
    }
    
    const getGuests = () => {
        if(!booking) return "No guests";
        if (booking.numberOfGuests === 1) return "1 guest";
        return `${booking.numberOfGuests} guests`;
    }
    
    return (booking && host && displayUser && listing) && <div className={"guest-conversation-details messages-window"}>
        <div className="mw-header">
            Reservation details
        </div>
        <div className="gcd-body">
            <div className="gcd-general-info">
                <div className="gcd-info">
                    <div className={"gray-bold"}>{BookingStatus[booking.status]}</div>
                    <div className="ch-message-name">{displayUser.name}</div>
                    <div className="gray-text">{listing.title}</div>
                    <div className="gray-text">{getDates()}</div>
                    <div className="gray-text">{getGuests()} / ${booking.totalPrice}</div>
                </div>
                <div className="gcd-avatar-holder">
                    <DefaultAvatar/>
                </div>
            </div>
            <div className="horizontal-divider"></div>
            <div className="gcd-about-host">
                <div className="ch-message-name-bigger">About {displayUser.name}</div>
                <div className="gcd-about-section"><Luggage/> {getTrips()}</div>
                {!props.isHostMode && <div className="gcd-about-section"><StarEmpty/> {getRating()} </div>}
                <div className="gcd-about-section"><Exclude/> {getJoined()}</div>
                <Link to={`/user/${displayUser.id}`} target={"_blank"} className="profile-link">Show profile</Link>
            </div>
            <div className="horizontal-divider"></div>
            <div className="gcd-booking-details">
                <div className="ch-message-name-bigger">Booking details</div>
                <div className="booking-details">
                    <div className="booking-detail">
                        <div className="booking-detail-title">Guests</div>
                        <div className="booking-detail-value">{getGuests()}</div>
                    </div> 
                    <div className="horizontal-divider"></div>
                    <div className="booking-detail">
                        <div className="booking-detail-title">Check-in</div>
                        <div className="booking-detail-value">{smallDate(booking.checkIn.toString())}</div>
                    </div>
                    <div className="horizontal-divider"></div>
                    <div className="booking-detail">
                        <div className="booking-detail-title">Check-out</div>
                        <div className="booking-detail-value">{smallDate(booking.checkOut.toString())}</div>
                    </div>
                    <Link to={"/trips"} target={"_blank"} className="profile-link">See trip</Link> 
                </div>
            </div>
        </div>
    </div>;
};