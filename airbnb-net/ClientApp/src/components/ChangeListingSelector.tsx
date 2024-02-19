import {useEffect, useMemo, useState} from "react";
import {ListingService} from "../services/ListingService";
import ListingDTO from "../DTOs/Listing/ListingDTO";
import Lens from "./Icons/Lens";
import {useNavigate} from "react-router-dom";

export const ChangeListingSelector = (props: { onClose: () => void }) => {

    
    const listingService = useMemo(() => { return ListingService.getInstance(); }, []);
    const navigate = useNavigate();
    
    useEffect(() => {
        document.addEventListener("click", ev => {
            const target = ev.target as HTMLElement;
            if (!target.closest(".change-listing-selector") && !target.closest(".change-listing")) {
                props.onClose();
            }
        });
        
        return () => {
            document.removeEventListener("click", () => {});
        };
    }, []);
    
    const [search, setSearch] = useState("");
    
    const [results, setResults] = useState<ListingDTO[]>([]);
    
    const handleSearchChange = (value: string) => {
        setSearch(value);
        listingService.getListingsByFilterFromToken({search: value}).then(res => {
            if (res.status === 200){
                setResults(res.data);
            }
        });
    }
    
    const handleListingClick = (listing: ListingDTO) => {
        navigate(`/hosting/listing/${listing.id}`);
        window.location.reload();
        props.onClose();
    }
    
    const goToListings = () => {
        navigate("/hosting/listings");
        props.onClose();
    }
    
    return <div className={"change-listing-selector"}>
        <div className="cls-search">
            <Lens/>
            <input className={"cls-search-input"} placeholder={"Search"} type="text" value={search} onChange={e => handleSearchChange(e.target.value)} />
        </div>
        <div className="cls-results">
            {(search.match(/^ *$/) !== null || results.length === 0) ? <div className={"cls-result"} onClick={goToListings}>All listings</div> : results.map((listing, index) => {
                return <div className="cls-result" onClick={() => handleListingClick(listing)}>{listing.title}</div> 
            } )}
        </div>
    </div>;
};