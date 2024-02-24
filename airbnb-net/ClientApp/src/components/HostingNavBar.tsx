import {Link} from "react-router-dom";
import UserProfileDropdown from "./UserProfileDropdown";
import "../styles/HostingNavBar.scss";
import React from "react";
import Exclude from "./Icons/Exclude";

interface HostingNavBarProps {
    destinations: HostingNavBarDestination[];
    currentDestination: number;
}

interface HostingNavBarDestination {
    name: string;
    url: string;
}

export const HostingNavBar = (props:HostingNavBarProps) => {
    return (
        <>
            <div className="hosting-nav-bar">
                <a className="item" href="/">
                    <Exclude/>
                </a>
                <div className="hosting-nav-bar-destinations">
                    {props.destinations.map((destination, index) => {
                        return <div>
                        <Link to={destination.url}
                              className={`hosting-nav-bar-destination ${index === props.currentDestination ? 'hosting-nav-bar-destination-selected' : ''}`}>
                            <span>{destination.name}</span>
                            <div className="destination-underline"></div> 
                        </Link>
                        </div>    
                    })}
                </div>
                <div style={{display:'flex', justifyContent:'flex-end'}}><UserProfileDropdown></UserProfileDropdown></div>
            </div>
        </>
    )
}