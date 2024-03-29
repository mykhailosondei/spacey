import ListingDTO from "../DTOs/Listing/ListingDTO";
import {PropertyType} from "../values/PropertyType";
import "../styles/ListingInfo.scss";
import React, {useMemo} from "react";
import {HostDTO} from "../DTOs/Host/HostDTO";
import {HostService} from "../services/HostService";
import {UserDTO} from "../DTOs/User/UserDTO";
import {UserService} from "../services/UserService";
import {ReactComponent as DefaultAvatar} from "../values/svgs/default-profile.svg";
import {Link} from "react-router-dom";

export const ListingInfo = (props: { listing: ListingDTO }) => {
    
    const [host, setHost] = React.useState<HostDTO>({} as HostDTO);
    const [user, setUser] = React.useState<UserDTO>({} as UserDTO);
    const hostService = useMemo(() => {return  HostService.getInstance()}, []);
    const userService = useMemo(() => {return  UserService.getInstance()}, []);
    const [isLoaded, setIsLoaded] = React.useState<boolean>(false);
    
    React.useEffect(() => {
        console.log("ListingPage.tsx: listing: ", props.listing);
        hostService.get(props.listing.host.id).then((host) => {
            if(host){
                setHost(host.data);
                props.listing.host = host.data;
                userService.get(host.data.userId).then((user) => {
                    if(user){
                        setUser(user.data);
                        setIsLoaded(true);
                    }
                    else{
                        console.log("User not found");
                    }
                });
            }
            else{
                console.log("Host not found");
            }
        });
    }, []);
    
    return <>
        <div className="listing-info-component">
            <div className="listing-info-title-and-location">
                <h2>{PropertyType[props.listing.propertyType]} in {addressToString(props.listing.address)}</h2>
            </div>
            <div className="listing-bedrooms-and-bathrooms">
                {props.listing.numberOfGuests} guests · {props.listing.numberOfRooms} bedrooms · {props.listing.numberOfBathrooms} bathrooms
            </div>
            <div className="horizontal-divider"></div>
            <div className="host-info">
                {isLoaded ?
                    <div>
                        <div>
                            {user.avatar?.url ? <img className="host-profile-picture" src={isLoaded ? user.avatar?.url : ""} 
                                                     alt="avatar"/> : <div className="host-profile-picture"><DefaultAvatar/></div>}
                            <Link style={{position:"absolute", width: "100%", height: "100%", inset:0}} to={`/user/${user.id}`}/> 
                        </div>
                        
                        <div className="host-info-text">
                            <b className="host-name">
                                Hosted by {user.name}
                            </b>
                            <div className="host-joined-date">
                                Joined in {new Date(user.createdAt).getFullYear()}
                            </div>
                        </div>
                    </div> : <></>}
            </div>
            <div className="horizontal-divider"></div>
            <pre className="listing-description">
                {props.listing.description}
            </pre>
            <div className="show-more">
                Show more
                <svg viewBox="0 0 18 18" role="presentation" aria-hidden="true" focusable="false"
                     style={{
                         height: '12px',
                         width: '12px',
                         display: 'block',
                         fill: '#222222',
                     }}><path
                    d="m4.29 1.71a1 1 0 1 1 1.42-1.41l8 8a1 1 0 0 1 0 1.41l-8 8a1 1 0 1 1 -1.42-1.41l7.29-7.29z"
                    fill-rule="evenodd"></path></svg>
            </div>
            <div className="horizontal-divider"></div>
        </div>
    </>;
}

const addressToString = (address: { street: string, city: string, country: string }) => {
    if (!address.city) return `${address.country}`;
    return `${address.city}, ${address.country}`;
}