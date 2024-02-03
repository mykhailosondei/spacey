import NavBar from "../NavBar";
import React, {useEffect, useMemo} from "react";
import {Trip} from "../Trip";
import {UserService} from "../../services/UserService";
import {UserDTO} from "../../DTOs/User/UserDTO";

export const TripsPage = () => {
    
    const userService = useMemo(() => {return UserService.getInstance()}, []);
    const [user, setUser] = React.useState<UserDTO | null>(null);

    useEffect(() => {
        userService.getFromToken().then((result) => {
            setUser(result.data);
        });
    }, []);
    
    return user ? <div>
        <div className="booking-navbar-wrapper"><NavBar></NavBar></div>
        <div className="trips-page">
            <div className={"header-large"}>Trips</div>
            <div className="trip-list">
                {
                    user.bookingsIds.map((bookingId) => {
                        return <Trip bookingId={bookingId}></Trip>
                    })
                }
            </div>
        </div>
    </div> : <></>;
};