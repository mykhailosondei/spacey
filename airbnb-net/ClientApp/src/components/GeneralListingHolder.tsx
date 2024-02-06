import React, {useMemo, useState} from 'react';
import '../styles/ListingHolder.scss';
import ListingBox from "./ListingBox";
import {ListingService} from "../services/ListingService";
import ListingDTO from "../DTOs/Listing/ListingDTO";
import {useLocation} from "react-router-dom";
import {HttpResponse} from "../services/HttpCustomClient";
import {log} from "node:util";

const GeneralListingHolder : React.FC = () => {
    
    const [listings, setListings] = useState<ListingDTO[]>([]);
    
    const listingService = useMemo(() => {return  ListingService.getInstance()}, []);
    
    const location = useLocation();
    
    let apiURL : string = "";
    
    let fetchPromise : Promise<HttpResponse<ListingDTO[]>>;
    //refactor this and paste code below if statements
    if(location.pathname.includes("propertyType")) {
        const propertyType = location.pathname.split("/").pop();
        if(propertyType) {
            fetchPromise = listingService.getByPropertyType(propertyType);
        }
    }else if(location.pathname.includes("boundingBox")) {
        const searchParams = new URLSearchParams(location.search);
        fetchPromise = listingService.getByBoundingBox(parseInt(searchParams.get("x1")!), parseInt(searchParams.get("y1")!), parseInt(searchParams.get("x2")!), parseInt(searchParams.get("y2")!));
    }else if(location.pathname.includes("search")) {
        const searchParams = new URLSearchParams(location.search);
        fetchPromise = listingService.getBySearch(searchParams.get("where")!, searchParams.get("checkIn")!, searchParams.get("checkOut")!, parseInt(searchParams.get("guests")!));
    }
    else {
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