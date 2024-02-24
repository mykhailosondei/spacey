// NavBar.tsx
import React from 'react';
import '../styles/NavBar.scss';
import UserProfileDropdown from "./UserProfileDropdown";
import {usePopup} from "../Contexts/PopupContext";
import {Link, useLocation, useParams, useSearchParams} from "react-router-dom";
import Cross from "./Icons/Cross";
import Lens from "./Icons/Lens";
import {NavBarDropdownGuests} from "./NavBarDropdownGuests";
import {NavBarDropdownDates} from "./NavBarDropdownDates";
import {NavBarDropdownWhere} from "./NavBarDropdownWhere";
import {useSelectedDates} from "../Contexts/SelectedDatesProvider";
import Exclude from "./Icons/Exclude";

enum NavBarButton {
    WHERE = 0,
    CHECKIN = 1,
    CHECKOUT = 2,
    GUESTS = 3,
    NONE = 4
}

interface NavBarButtonProps {
    searchMode?: {where: string, dates:string, guests: string};
}

const NavBar: React.FC<NavBarButtonProps> = (props : NavBarButtonProps) => {
    
    const searchMode = !!props.searchMode;
    
    const [isSearchBarOpen, setIsSearchBarOpen] = React.useState<boolean>(false);
    
    const [whereQuery, setWhereQuery] = React.useState<string>("");
    
    const {startDate, endDate} = useSelectedDates();
    
    const [buttonAtTarget, setButtonAtTarget] = React.useState<NavBarButton>(NavBarButton.NONE);
    
    const [guests, setGuests] = React.useState<number>(1);
    
    const location = useLocation();
    
    const setButtonAt = (button: NavBarButton) => {
        if(!isSearchBarOpen) return;
        setButtonAtTarget(button);
    }
    
    const openSearchBar = () => {
        setIsSearchBarOpen(true);
    }

    function closeSearchBar() {
        setButtonAtTarget(NavBarButton.NONE);
        setIsSearchBarOpen(false);
    }
    
    function formatDate(date: Date | null) {
        if(!date) return "";
        return date.toLocaleDateString();
    }
    
    const buttonNotNone = () => buttonAtTarget !== NavBarButton.NONE;
    
    function formatGuests(guests: number) {
        if (guests === 0) {
            return "";
        }
        if (guests === 1) {
            return "1 guest";
        }
        return guests + " guests";
    }
    
    const searchAvailable = () => {
        return whereQuery !== "" || startDate !== null || endDate !== null;
    }
    
    const smallButtonTitle = (button: NavBarButton) => {
        switch(button) {
            case NavBarButton.WHERE:
                return !searchMode ? "Anywhere" : props.searchMode?.where;
            case NavBarButton.CHECKIN || NavBarButton.CHECKOUT:
                return !searchMode ? "Any week" : props.searchMode?.dates;
            case NavBarButton.GUESTS:
                return !searchMode ? "Add guests" : props.searchMode?.guests;
        }
    }
    
    const searchQuery = () => {
        const additional = `?where=${whereQuery}&checkIn=${formatDate(startDate)}&checkOut=${formatDate(endDate)}&guests=${guests}`;
        const propertyType = new URLSearchParams(location.search).get("propertyType");
        if (propertyType !== undefined) {
            return `${additional}&propertyType=${propertyType}`;
        }
        return additional;
    }

    return (
        <div className="navbar">
            <div className="left">
                <a className="item" href="/">
                    <Exclude/>
                </a>
            </div>
            <div className="middle">
                <div className={"middle-item" + (buttonNotNone()?" full-gray":"")} style={{height: isSearchBarOpen?"60px":"48px"}} onClick={openSearchBar}>
                    <button className={"nav-btn" + (isSearchBarOpen?" gray-hover":"") + (buttonAtTarget == NavBarButton.WHERE ? " selected-nav-btn" : "")} data-index="0" onClick={()=>setButtonAt(NavBarButton.WHERE)}>
                        <div className="word w-fit">{isSearchBarOpen?"Where":smallButtonTitle(NavBarButton.WHERE)}</div>
                        {isSearchBarOpen && <input className="search-bar-input" placeholder={"Search destinations"} value={whereQuery} onChange={(e) => {setWhereQuery(e.target.value)}}></input>}
                    </button>
                    <span className="divider-bar"></span>
                    <button className={"nav-btn" + (isSearchBarOpen?" gray-hover":"") + (buttonAtTarget == NavBarButton.CHECKIN ? " selected-nav-btn" : "")} data-index="1" onClick={()=>setButtonAt(NavBarButton.CHECKIN)}>
                        <div className="word">{isSearchBarOpen?"Check-in":smallButtonTitle(NavBarButton.CHECKIN)}</div>
                        {isSearchBarOpen && <input className="search-bar-input" placeholder={"Add date"} value={formatDate(startDate)}></input>}
                    </button>
                    <span className="divider-bar"></span>
                    {isSearchBarOpen && <><button className={"nav-btn" + (isSearchBarOpen?" gray-hover":"") + (buttonAtTarget == NavBarButton.CHECKOUT ? " selected-nav-btn" : "")} data-index="1" onClick={()=>setButtonAt(NavBarButton.CHECKOUT)}>
                        <div className="check-out">Check-out</div>
                        {isSearchBarOpen && <input className="search-bar-input" placeholder={"Add date"} value={formatDate(endDate)}></input>}
                    </button>
                        <span className="divider-bar"></span></>}
                    <button className={"nav-btn add-guests" + (isSearchBarOpen?" gray-hover add-guests-big":"") + (buttonAtTarget == NavBarButton.GUESTS ? " selected-nav-btn" : "")} data-index="0" onClick={() => setButtonAt(NavBarButton.GUESTS)}>
                        <div className={"word w-fit" + (!isSearchBarOpen ? " grayword":"")}>{isSearchBarOpen?"Who":smallButtonTitle(NavBarButton.GUESTS)}</div>
                        {isSearchBarOpen && <input className="search-bar-input" value={formatGuests(guests)} placeholder={"Add guests"}></input>}
                        <div className={"search-icon-btn" + (isSearchBarOpen? " search-icon-bigger" : "") + (buttonNotNone()?" button-selected":"")}>
                            {searchAvailable() && <Link className={"link"} to={{pathname:"/search", search : searchQuery()}}/>}
                            <Lens className="search-icon"></Lens>
                            {buttonNotNone() && <div className="search-icon-text">Search</div>}
                        </div>
                    </button>
                </div>
                <NavBarDropdownWhere whereQuery={whereQuery} setWhereQuery={setWhereQuery} active={buttonAtTarget == NavBarButton.WHERE}></NavBarDropdownWhere>
                <NavBarDropdownDates 
                    active={buttonAtTarget == NavBarButton.CHECKOUT || buttonAtTarget == NavBarButton.CHECKIN}
                ></NavBarDropdownDates>
                <NavBarDropdownGuests active={buttonAtTarget == NavBarButton.GUESTS} guests={guests} setGuests={setGuests}></NavBarDropdownGuests>
                
                {isSearchBarOpen && <div className="close-search-bar" onClick={closeSearchBar}>
                    <Cross/>
                </div>}
            </div>
            <div className="right">
                <div className="host-options">
                    <Link className={"hosting"} to={"/hosting"}>
                        <div className="hostingtext">Switch to hosting</div>
                    </Link>
                    <button className="lan-button">
                        <div className="globe-logo">
                            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16" aria-hidden="true" role="presentation" focusable="false"><path d="M8 .25a7.77 7.77 0 0 1 7.75 7.78 7.75 7.75 0 0 1-7.52 7.72h-.25A7.75 7.75 0 0 1 .25 8.24v-.25A7.75 7.75 0 0 1 8 .25zm1.95 8.5h-3.9c.15 2.9 1.17 5.34 1.88 5.5H8c.68 0 1.72-2.37 1.93-5.23zm4.26 0h-2.76c-.09 1.96-.53 3.78-1.18 5.08A6.26 6.26 0 0 0 14.17 9zm-9.67 0H1.8a6.26 6.26 0 0 0 3.94 5.08 12.59 12.59 0 0 1-1.16-4.7l-.03-.38zm1.2-6.58-.12.05a6.26 6.26 0 0 0-3.83 5.03h2.75c.09-1.83.48-3.54 1.06-4.81zm2.25-.42c-.7 0-1.78 2.51-1.94 5.5h3.9c-.15-2.9-1.18-5.34-1.89-5.5h-.07zm2.28.43.03.05a12.95 12.95 0 0 1 1.15 5.02h2.75a6.28 6.28 0 0 0-3.93-5.07z"></path></svg>
                        </div>
                    </button>
                </div>
                <UserProfileDropdown></UserProfileDropdown>
            </div>
        </div>
    );
};

export default NavBar;
