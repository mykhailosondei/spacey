import React from "react";
import "../styles/Dropdown.scss";
import {ListingFilter} from "./HostingListingsSection";

interface RoomsAndBedsDropdownProps {
    onApplyClick : (filter: ListingFilter) => void;
    filter: ListingFilter;
}

export const RoomsAndBedsDropdown = (props: RoomsAndBedsDropdownProps) => {
    
    const [bedrooms, setBedrooms] = React.useState(props.filter.bedrooms || 0);
    const [beds, setBeds] = React.useState(props.filter.beds || 0);
    const [guests, setGuests] = React.useState(props.filter.guests || 0);

    function clearAllSelection() {
        setBedrooms(0);
        setBeds(0);
        setGuests(0);
    }

    return <div className={"rooms-and-beds-dropdown dropdown-body"}>
            <div className="dropdown-content">
                <div className="dropdown-section">
                    <div className={"counter-holder"}>
                        Bedrooms
                        <div className="guest-counter">
                            <button className="guest-counter-button" onClick={() => setBedrooms(bedrooms - 1)}
                                    disabled={bedrooms == 0}>-
                            </button>
                            {bedrooms}
                            <button className="guest-counter-button" onClick={() => setBedrooms(bedrooms + 1)}
                                    disabled={bedrooms == 4}>+
                            </button>
                        </div>
                    </div>
                    <div className={"counter-holder"}>
                        Beds
                        <div className="guest-counter">
                            <button className="guest-counter-button" onClick={() => setBeds(beds - 1)}
                                    disabled={beds == 0}>-
                            </button>
                            {beds}
                            <button className="guest-counter-button" onClick={() => setBeds(beds + 1)}
                                    disabled={beds == 4}>+
                            </button>
                        </div>
                    </div>
                    <div className={"counter-holder"}>
                        Guests
                        <div className="guest-counter">
                            <button className="guest-counter-button" onClick={() => setGuests(guests - 1)}
                                    disabled={guests == 0}>-
                            </button>
                            {guests}
                            <button className="guest-counter-button" onClick={() => setGuests(guests + 1)}
                                    disabled={guests == 4}>+
                            </button>
                        </div>
                    </div>
                </div>
                <div className="horizontal-divider"></div>
                <div className="dropdown-section">
                    <div className="align-d-buttons">
                        <div className="cancel-button custom-width" onClick={clearAllSelection}>Clear</div>
                        <div className="clear-dates-button custom-width margin-0" onClick={() => props.onApplyClick({...props.filter, bedrooms: bedrooms, beds: beds, guests: guests})}>Apply</div>
                    </div>
                </div>
            </div>
    </div>;
};