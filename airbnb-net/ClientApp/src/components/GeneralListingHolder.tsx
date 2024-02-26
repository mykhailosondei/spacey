import React, {useEffect, useLayoutEffect, useMemo, useState} from 'react';
import '../styles/ListingHolder.scss';
import ListingBox from "./ListingBox";
import {ListingService} from "../services/ListingService";
import ListingDTO from "../DTOs/Listing/ListingDTO";
import {useLocation, useNavigate} from "react-router-dom";
import {HttpResponse} from "../services/HttpCustomClient";
import {log} from "node:util";
import {SearchConfig} from "../values/SearchConfig";
import {useMapResults} from "../Contexts/MapResultsProvider";

interface GeneralListingHolderProps {
    searchConfig?: SearchConfig;
}


const GeneralListingHolder : React.FC<GeneralListingHolderProps> = (props:GeneralListingHolderProps) => {
    
    const holder = React.useRef<HTMLDivElement>(null);
    
    const [listingsPerRow, setListingsPerRow] = useState<number>(6);
    
    const [listings, setListings] = useState<ListingDTO[]>([]);
    
    const listingService = useMemo(() => {return  ListingService.getInstance()}, []);
    
    const [gettingMoreListings, setGettingMoreListings] = useState<boolean>(false);
    const windowWidth = window.innerWidth;
    
    const location = useLocation();
    
    let apiURL : string = "";

    useLayoutEffect(() => {
        const handleResize = () => {
            setListingsPerRow(Math.ceil(window.innerWidth / 350));
        }
        window.addEventListener('resize', handleResize);
        setListingsPerRow(Math.ceil(window.innerWidth / 350));
        return () => {
            window.removeEventListener('resize', handleResize);
        }
    }, []);
    
    const price = (pricePerNight: number) => {
        return `$${pricePerNight} CAD`;
    }

    function getMoreListings() {
        if(!gettingMoreListings){
            setGettingMoreListings(true);
            if(props.searchConfig){
                listingService.getBySearch(props.searchConfig, listings.length, listings.length + 2 * listingsPerRow).then((response) => {
                    if(response.status === 200) {
                        setListings((listings) => [...listings, ...response.data]);
                        setGettingMoreListings(false);
                    }
                });
            }
            else {
                listingService.getAll(listings.length, listings.length + 2 * listingsPerRow).then((response) => {
                    if (response.status === 200) {
                        setListings((listings) => [...listings, ...response.data]);
                        setGettingMoreListings(false);
                    }
                });
            }
        }
    }
    
    const firstRender = React.useRef(true);

    useEffect(() => {
        if(firstRender.current){
            firstRender.current = false;
            return;
        }
        const handleScroll = () => {
            if(holder?.current){
                const holderBottom = holder.current.getBoundingClientRect().bottom;
                const listingBoxHeight = holder.current.children[0].getBoundingClientRect().height;
                const viewportHeight = visualViewport?.height || window.innerHeight;
                
                const difference = holderBottom - viewportHeight;
                if(difference < 3 * listingBoxHeight / 2){
                    console.log(listings.length);
                    getMoreListings();
                    window.removeEventListener('scroll', handleScroll);
                    window.removeEventListener('resize', handleScroll);
                }
            }
        }
        window.addEventListener('scroll', handleScroll);
        window.addEventListener('resize', handleScroll);
    }, [listings.length]);
    
    React.useEffect(() => {
        console.log("new fetching");
        if(props.searchConfig){
            listingService.getBySearch(props.searchConfig, 0, listingsPerRow * 3).then((listings) => {
                if(listings.status === 200) {
                    setListings(listings.data);
                }
            });
        }
        else {
            listingService.getAll(0, listingsPerRow * 3).then((listings) => {
                if(listings.status === 200) {
                    setListings(listings.data);
                }
            });
        }
        
    }, [location]);
    
    return (
        <>
            <div ref={holder} className={'listing-holder'} style={{gridTemplateColumns: `repeat(${listingsPerRow}, minmax(0, 1fr))`}}>
                {listings.map((listing, index) => {
                    return <ListingBox key={index} listing={listing}/>
                })}
            </div>
        </>
    )
        
    
}

export default GeneralListingHolder;