import NavBar from "../NavBar";
import React, {useEffect, useMemo} from "react";
import {ReactComponent as DefaultAvatar} from "../../values/svgs/default-profile.svg";
import "../../styles/UserProfilePage.scss";
import Globe from "../Icons/Globe";
import {useParams} from "react-router-dom";
import {UserService} from "../../services/UserService";
import {UserDTO} from "../../DTOs/User/UserDTO";
import {BookingService} from "../../services/BookingService";
import {ListingService} from "../../services/ListingService";
import {UpdatePopup} from "../UpdatePopup";

export const UserProfilePage: React.FC = () => {
    
    const {id} = useParams();
    
    const [user, setUser] = React.useState<UserDTO | null>(null);
    const [yearNames, setYearNames] = React.useState<{year:number, name:string}[]>([]);
    const [isMe, setIsMe] = React.useState(false);
    const [updatePopupOpen, setUpdatePopupOpen] = React.useState(false);
    
    const userService : UserService = useMemo(() => {return UserService.getInstance()}, []);
    const bookingService = useMemo(() => {return BookingService.getInstance()}, []);
    const listingService = useMemo(() => {return ListingService.getInstance()}, []);
    
    useEffect(()=> {
        if(!id) return;
        userService.get(id).then((user) => {
            if(!user) throw new Error("User not found");
            setUser(user.data);
            bookingService.getMany(user.data.bookingsIds).then((bookings) => {
                if(!bookings) return;
                bookings.forEach((booking) => {
                    listingService.get(booking.listingId).then((listing) => {
                        if(!listing) return;
                        setYearNames((prev) => {
                            const year = new Date(booking.checkIn).getFullYear();
                            if(prev.some((yearName) => yearName.year === year)) return prev;
                            return [...prev, {year, name: `${listing.data.address.city}, ${listing.data.address.country}`}];
                        });
                    });
                });
            });
        });
        userService.getFromToken().then((user) => {
            if(!user) return;
            setIsMe(user.data.id === id);
        });
    }, []);
    
    const dateJoinedFormatted = () => {
        if(!user) return {count: 0, unit: "days"};
        const diff = new Date().getTime() - new Date(user.createdAt).getTime();
        const days = Math.floor(diff / (1000 * 60 * 60 * 24));
        if(days < 30) {
            if(days.toString().endsWith("1")) return {count: days, unit: "day"};
            return {count: days, unit: "days"};
        }
        if(days < 365) {
            const months = Math.floor(days / 30);
            if(months.toString().endsWith("1")) return {count: months, unit: "month"};
            return {count: months, unit: "months"};
        }
        const years = Math.floor(days / 365);
        if(years.toString().endsWith("1")) return {count: years, unit: "year"};
        return {count: years, unit: "years"};
    }

    function openUpdatePopup() {
        setUpdatePopupOpen(true);
    }
    
    function closeUpdatePopup() {
        setUpdatePopupOpen(false);
    }
    
    const LivesIn = () => {
        if (!user || !user.address || !user.address.country) return (<></>);
        if(!user.address.city){
            return <div className="lives-in">
                <Globe></Globe>
                Lives in {user.address.country}
            </div>;
        }
        return <div className="lives-in">
            <Globe></Globe>
            Lives in {user.address.city}, {user.address.country}
        </div>;
    }
    
    const Description = () => {
        if(!user || !user.description) 
            return <>
                <div className="header-medium">Description</div>
                <div className="reg-text-med">No description</div>
            </>;
        return <div className="up-description">
            <div className="header-medium">Description</div>
            {user.description}
        </div>;
    }

    return user && <div>
        {updatePopupOpen && <UpdatePopup user={user} onClose={closeUpdatePopup}></UpdatePopup>}
        <div className="up-nav-wrapper page-navbar-wrapper"><NavBar></NavBar></div>
        <div className="user-profile-page">
            <div className="up-profile-card-space">
                <div className="up-profile-card">
                    <div className="up-name-and-avatar">
                        <div className="up-avatar">
                            {user.avatar?.url ? <img src={user.avatar.url} alt={""}></img> : <DefaultAvatar></DefaultAvatar>}
                        </div>
                        <div className="up-name">
                            <div className="header-medium">{user.name}</div>
                        </div>
                    </div>
                    <div className="up-date-joined-and-reviews">
                        {user.reviewsIds.length > 0 && <div className="up-reviews">
                            <div className="header-medium" style={{margin: 0}}>{user.reviewsIds.length}</div>
                            <div className="reg-text-small">reviews</div>
                            <div className="horizontal-divider"></div>
                        </div>}
                        <div className="header-medium" style={{margin: 0}}>{dateJoinedFormatted().count}</div>
                        <div className="reg-text-small">{dateJoinedFormatted().unit} on the platform</div>
                    </div>
                </div>
            </div>
            <div className="up-content">
                <div className="up-about">
                    <div className="header-large">About {user.name}</div>
                    {isMe && <button className="create-button custom-width margin-b20" onClick={openUpdatePopup}>Edit profile</button>}
                    <LivesIn/>
                    <Description/>
                </div>
                <div className="horizontal-divider"></div>
                <div className="header-medium">Previous destinations</div>
                <div className="up-listings">
                    {yearNames.length > 0 ? yearNames.map((yearName) => {
                        return <div className="up-listing">
                            <div className="upl-year">{yearName.year}</div>
                            <div className="upl-location">{yearName.name}</div>
                        </div>
                    }) : <div className="reg-text-med">No previous destinations</div>}
                </div>
            </div>
        </div>
    </div>;
};