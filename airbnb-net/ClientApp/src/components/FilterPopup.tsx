import Lens from "./Icons/Lens";
import {useState} from "react";
import ListingDTO from "../DTOs/Listing/ListingDTO";
import {ListingService} from "../services/ListingService";
import { useMemo } from "react";

export function FilterPopup(props: { onClose: (listingId?: string) => void }) {
    
    const [search, setSearch] = useState("");
    const [results, setResults] = useState<ListingDTO[]>([]);
    const [chosenListing, setChosenListing] = useState<ListingDTO | null>(null);
    
    const listingService = useMemo(() => { return ListingService.getInstance(); }, []);
    
    const handleSearchChange = (value: string) => {
        setSearch(value);
         listingService.getListingsByFilterFromToken({search: value}).then(res => {
             if (res.status === 200){
                 setResults(res.data);
             }
         });
    }
    
    const handleListingClick = (listing: ListingDTO) => {
        setChosenListing(listing);
    }
    
    const deselectListing = () => {
        setChosenListing(null);
    }

    function onClose() {
        if (chosenListing) {
            props.onClose(chosenListing.id);
        }
        else {
            props.onClose();
        }
    }

    return <div className="filter-popup">
        {!chosenListing ? <div className="cls-search">
            <Lens/>
            <input className={"cls-search-input"} placeholder={"Search"} type="text" value={search}
                   onChange={e => handleSearchChange(e.target.value)}/>
        </div> : <div className="selected-listing">
            <div className={"selected-listing-text"}>Listing: </div>
            <div className="cls-result" onClick={deselectListing}>{chosenListing.title}</div>
        </div>}
        {!chosenListing ? <div className="cls-results">
            {results.map((listing, index) => {
                return <div className="cls-result" onClick={() => handleListingClick(listing)}>{listing.title}</div>
            })}
        </div> : <></>}
        <div className="white-on-black-btn margin-t10" onClick={onClose}>Close</div>
    </div>;
}