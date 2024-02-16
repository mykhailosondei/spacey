import React, {Component, useEffect, useMemo} from "react";
import {ListingsTable} from "./ListingsTable";
import "../styles/HostingListingsSection.scss"
import {DropdownSVG} from "../DropdownSVG";
import ListingDTO from "../DTOs/Listing/ListingDTO";
import {ListingService} from "../services/ListingService";
import axios from "axios";
import {useHost} from "../Contexts/HostContext";
import {UserService} from "../services/UserService";
import {RoomsAndBedsDropdown} from "./RoomsAndBedsDropdown";
import {AmenitiesDropdown} from "./AmenitiesDropdown";

export interface ListingFilter{
    bedrooms?: number;
    beds?: number;
    guests?: number;
    amenities?: string[];
}

export const HostingListingsSection = () => {
    const filterDropdowns = ["Rooms and beds", "Amenities"];
    const [currentDropdown, setCurrentDropdown] = React.useState(0);
    const {host} = useHost();
    
    const [filterState, setFilterState] = React.useState<ListingFilter>({});
    
    const [listingsState, setListingsState] = React.useState([] as ListingDTO[]);
    const listingService = useMemo(() => {return ListingService.getInstance()}, []);
    const userService = useMemo(() => {return UserService.getInstance()}, []);

    useEffect(() => {
        let source = axios.CancelToken.source();
        
        const loadListings = async () => {
        
        try
        {
            await listingService.getListingsByFilterFromToken(filterState).then((listings) => {
                setListingsState(listings.data);
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
    }, [filterState]);
    
    useEffect(() => {
        const handleClick = (e : MouseEvent) => {
            if(!(e.target as HTMLElement).closest("#dropdown")) {
                setCurrentDropdown(-1);
            }
        };
        
        document.addEventListener("click", handleClick);
        return () => {
            document.removeEventListener("click", handleClick);
        }
    }, []);
    
    const handleFilterChange = (filter : ListingFilter) => {
        console.log("HERE", filter, filterState);
        setFilterState(filter);
        setCurrentDropdown(-1);
    }
    
    const dropdowns : JSX.Element[] = [<RoomsAndBedsDropdown onApplyClick={handleFilterChange} filter={filterState}/> as JSX.Element, <AmenitiesDropdown onApplyClick={handleFilterChange} filter={filterState}/> as JSX.Element];

    function clearFilter() {
        setFilterState({});
    }

    function isFilterEmpty() : boolean {
        return Object.keys(filterState).length == 0;
    }

    return <>
        <div className="listings-section">
            <div className="filter-actions-container">
                <div className="amount-and-create-button">
                    <div className="listings-amount">
                        <h1>
                            {listingsState.length} listing{listingsState.length != 1 ? "s" : ""}
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
                            return <div id={"dropdown"} style={{position:"relative"}}>
                                <div
                                    className={"filter-dropdown " + (currentDropdown == index ? "filter-dropdown-selected" : "")}
                                    onClick={() => setCurrentDropdown(index)}>
                                    {dropdown} <DropdownSVG/>
                                </div>
                                {currentDropdown == index ? dropdowns[index] : <></>}
                            </div>
                        })}
                        <div className={"filter-dropdown clear-filters" + (isFilterEmpty() ? "" : " cf-highlight")} onClick={clearFilter}>Clear filters</div>
                    </div>
                </div>
            </div>
            <ListingsTable listings={listingsState}></ListingsTable>
        </div>
    </>
}