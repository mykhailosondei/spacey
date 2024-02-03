import NavBar from "../NavBar";
import React, {useEffect, useMemo} from "react";
import {ReactComponent as DefaultAvatar} from "../../values/svgs/default-profile.svg";
import "../../styles/UserProfilePage.scss";
import Globe from "../Icons/Globe";
import {useParams} from "react-router-dom";
import {UserService} from "../../services/UserService";
import {UserDTO} from "../../DTOs/User/UserDTO";
import {BookingService} from "../../services/BookingService";
import {BookingDTO} from "../../DTOs/Booking/BookingDTO";
import ListingDTO from "../../DTOs/Listing/ListingDTO";
import {ListingService} from "../../services/ListingService";

export const UserProfilePage: React.FC = () => {
    
    const {id} = useParams();
    
    const [user, setUser] = React.useState<UserDTO | null>(null);
    const [yearNames, setYearNames] = React.useState<{year:number, name:string}[]>([]);
    
    const userService : UserService = useMemo(() => {return UserService.getInstance()}, []);
    const bookingService = useMemo(() => {return BookingService.getInstance()}, []);
    const listingService = useMemo(() => {return ListingService.getInstance()}, []);
    
    useEffect(()=> {
        if(!id) return;
        userService.get(id).then((user) => {
            if(!user) return;
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
    
    return user && <div>
        <div className="up-nav-wrapper page-navbar-wrapper"><NavBar></NavBar></div>
        <div className="user-profile-page">
            <div className="up-profile-card-space">
                <div className="up-profile-card">
                    <div className="up-name-and-avatar">
                        <div className="up-avatar">
                            <DefaultAvatar></DefaultAvatar>
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
                    <div className="lives-in">
                        <Globe></Globe>
                        Lives in {user.address.city}, {user.address.country}
                    </div>
                    <div className="up-description">
                        <div className="header-medium">Description</div>
                        {user.description}
                    </div>
                </div>
                <div className="horizontal-divider"></div>
                <div className="header-medium">Previous destinations</div>
                <div className="up-listings">
                    {yearNames.map((yearName) => {
                        return <div className="up-listing">
                            <div className="upl-year">{yearName.year}</div>
                            <div className="upl-location">{yearName.name}</div>
                        </div>
                    })}
                </div>
            </div>
        </div>
    </div>;
};