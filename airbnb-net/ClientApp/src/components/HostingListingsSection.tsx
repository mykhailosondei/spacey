import React, {useEffect, useMemo} from "react";
import {ListingsTable} from "./ListingsTable";
import "../styles/HostingListingsSection.scss"
import {DropdownSVG} from "../DropdownSVG";
import ListingDTO from "../DTOs/Listing/ListingDTO";
import {AuthService} from "../services/AuthService";
import {ListingService} from "../services/ListingService";
import axios from "axios";
import {useHost} from "../Contexts/HostContext";
import {useUser} from "../Contexts/UserContext";
import {UserService} from "../services/UserService";

export const HostingListingsSection = () => {
    const filterDropdowns = ["Rooms and beds", "Amenities", "Status", "More filters"];
    const [currentDropdown, setCurrentDropdown] = React.useState(0);
    const {host} = useHost();
    
    const [listingsState, setListingsState] = React.useState([] as ListingDTO[]);
    const listingService = useMemo(() => {return ListingService.getInstance()}, []);
    const userService = useMemo(() => {return UserService.getInstance()}, []);

    useEffect(() => {
        let source = axios.CancelToken.source();
        
        const loadListings = async () => {
        
        try
        {
            console.log("host: ", host)
            let getListingsForHostPromise = Promise.all(host!.listingsIds.map((listingId) => {
                return listingService.get(listingId);
            }));
            await getListingsForHostPromise.then((listings) => {
                setListingsState(listings);
            });
        }
        catch (e) 
        {
            if(axios.isCancel(e)) {
                console.log("Request cancelled");
            }
            else throw e;
        }
        }
        loadListings();
        
        return () => {
            source.cancel();
        }
    }, []);
    
    useEffect(() => {
        const handleClick = (e : MouseEvent) => {
            if(!(e.target as HTMLElement).closest(".filter-dropdown")) {
                setCurrentDropdown(-1);
            }
        };
        
        document.addEventListener("click", handleClick);
        return () => {
            document.removeEventListener("click", handleClick);
        }
    }, []);
    
    return <>
        <div className="listings-section">
            <div className="filter-actions-container">
                <div className="amount-and-create-button">
                    <div className="listings-amount">
                        <h1>
                            {1} listing
                        </h1> 
                    </div>
                    <button className="create-button">
                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32"
                             style={{
                                 display: 'block',
                                 height: '16px',
                                 width: '16px',
                                 fill: 'rgb(34, 34, 34)',
                             }}
                             aria-hidden="true" role="presentation" focusable="false">
                            <path d="M18 4v10h10v4H18v10h-4V18H4v-4h10V4z"></path>
                        </svg>
                        Create Listing
                    </button>
                </div>
                <div className="filter-actions">
                    <div className="search-input">
                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32" style={{
                            display: 'block',
                            fill: 'none',
                            height: '16px',
                            width: '16px',
                            stroke: 'currentColor',
                            strokeWidth: '3px',
                            overflow: 'visible',
                        }}
                             aria-hidden="true" role="presentation" focusable="false">
                            <path fill="none" d="M13 24a11 11 0 1 0 0-22 11 11 0 0 0 0 22zm8-3 9 9"></path>
                        </svg>
                        <input type="text" className="listing-filter-input" placeholder={"Search listings"}/>
                    </div>
                    <div className="filter-dropdowns">
                        {filterDropdowns.map((dropdown, index) => {
                            return <div className={"filter-dropdown " + (currentDropdown == index ? "filter-dropdown-selected" : "") }
                                        onClick={() => setCurrentDropdown(index)}>
                                {dropdown} <DropdownSVG/>
                            </div>
                        })}
                    </div>
                </div>
            </div>
            <ListingsTable listings={listingsState}></ListingsTable>
        </div>
    </>
}