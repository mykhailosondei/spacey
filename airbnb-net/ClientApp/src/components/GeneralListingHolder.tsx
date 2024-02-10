import React, {useMemo, useState} from 'react';
import '../styles/ListingHolder.scss';
import ListingBox from "./ListingBox";
import {ListingService} from "../services/ListingService";
import ListingDTO from "../DTOs/Listing/ListingDTO";
import {useLocation, useNavigate} from "react-router-dom";
import {HttpResponse} from "../services/HttpCustomClient";
import {log} from "node:util";
import {SearchConfig} from "../values/SearchConfig";

interface GeneralListingHolderProps {
    searchConfig?: SearchConfig;
}

const GeneralListingHolder : React.FC<GeneralListingHolderProps> = (props:GeneralListingHolderProps) => {
    
    const [listings, setListings] = useState<ListingDTO[]>([]);
    
    const listingService = useMemo(() => {return  ListingService.getInstance()}, []);
    
    const location = useLocation();
    
    let apiURL : string = "";
    
    let fetchPromise : Promise<HttpResponse<ListingDTO[]>>;
    
    if(props.searchConfig) {
        fetchPromise = listingService.getBySearch(props.searchConfig);
    } else {
        fetchPromise = listingService.getAll();
    }
    
    
    React.useEffect(() => {
        fetchPromise.then((listings) => {
            if(listings.status === 200) {
                setListings(listings.data);
            }
        });
    }, [location]);
    
    return (
        <>
            <div className={'listing-holder'}>
                {listings.map((listing, index) => {
                    return <ListingBox key={index} listing={listing}/>
                })}
            </div>
        </>
    )
        
    
}

export default GeneralListingHolder;