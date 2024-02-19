import React, {useEffect, useLayoutEffect} from "react";
import {useParams} from "react-router-dom";
import ListingDTO from "../DTOs/Listing/ListingDTO";
import {ListingService} from "../services/ListingService";
import "../styles/ManageListingSection.scss";
import LeftArrow from "./Icons/LeftArrow";
import {ListingDetail} from "./ListingDetail";
import {StringEditItem} from "./StringEditItem";
import {AmenitiesEditItem} from "./AmenitiesEditItem";
import {PropertyEditItem} from "./PropertyEditItem";
import {CounterEditItem} from "./CounterEditItem";
import {PriceEditItem} from "./PriceEditItem";
import {ListingUpdateDTO} from "../DTOs/Listing/ListingUpdateDTO";
import {useHost} from "../Contexts/HostContext";
import {PropertyType} from "../values/PropertyType";

export function ManageListingSection() {
    
    const { id} = useParams();
    
    const [listing, setListing] = React.useState<ListingDTO | null>(null);
    
    const [currentDetailGroup, setCurrentDetailGroup] = React.useState(0);
    
    const [scrollY, setScrollY] = React.useState(0);
    
    const {host} = useHost();
    
    useLayoutEffect(() => {
        const updateScrollY = () => {
            setScrollY(window.scrollY);
        }
        window.addEventListener("scroll", updateScrollY);
        return () => window.removeEventListener("scroll", updateScrollY);
    }, []);

    useEffect(() => {
        const detailGroups = document.getElementsByClassName("listing-detail");
        let i = 0;
        let found = false;
        for(i = 0; i < detailGroups.length; i++) {
            if(detailGroups[i].getBoundingClientRect().top > -5) {
                found = true;
                break;
            }
        }
        if(found) {
            setCurrentDetailGroup(i);
        }
    }, [scrollY]);
    
    const gotoDetailGroup = (index: number) => {
        const detailGroups = document.getElementsByClassName("listing-detail");
        if(index >= 0 && index < detailGroups.length) {
            detailGroups[index].scrollIntoView({behavior: "smooth"});
        }
    }

    useEffect(() => {
        if(!listing) return;
        const detailGroups = document.getElementsByClassName("d-list-item");
        console.log(detailGroups);
        for(let i = 0; i < detailGroups.length; i++) {
            detailGroups[i].classList.remove("d-active");
        }
        detailGroups[currentDetailGroup].classList.add("d-active");
    }, [currentDetailGroup]);
    
    const listingService = React.useMemo(() => { return ListingService.getInstance()}, []);
    
    React.useEffect(() => {
        listingService.get(id!).then((listing) => {
            if(listing.status === 200) {
                setListing(listing.data);
            }
        });
    }, []);
    
    function onSubmitTitle(value: string) {
        if(!host) return;
        const validate = (value: string) => value.length < 32 && value.length > 0 && value !== listing?.title && value.match(/^ *$/) === null;
        if(!validate(value)) return;
        const newListing : ListingUpdateDTO = {...listing!, hostId: host.id, title: value, address: `${listing?.address.street}, ${listing?.address.city}, ${listing?.address.country}`};
        listingService.update(id!, newListing).then((response) => {
            if(response.status === 200) {
                setListing({...listing!, title: value});
            }
        });
    }

    function onSubmitDescription(value : string) {
        if(!host) return;
        const validate = (value: string) => value.length < 500 && value.length > 0 && value !== listing?.description && value.match(/^ *$/) === null;
        if(!validate(value)) return;
        const newListing : ListingUpdateDTO = {...listing!, hostId: host.id, description: value, address: `${listing?.address.street}, ${listing?.address.city}, ${listing?.address.country}`};
        listingService.update(id!, newListing).then((response) => {
            if(response.status === 200) {
                setListing({...listing!, description: value});
            }
        });
    }

    function onSubmitStreet(value : string) {
        if(!host) return;
        const validate = (value: string) => value.length < 100 && value.length > 0 && value !== listing?.address.street && value.match(/^ *$/) === null;
        if(!validate(value)) return;
        const newAddress : string = `${value}, ${listing?.address.city}, ${listing?.address.country}`;
        const newListing : ListingUpdateDTO = {...listing!, hostId: host.id, address: newAddress};
        listingService.update(id!, newListing).then((response) => {
            if(response.status === 200) {
                listingService.get(id!).then((listing) => {
                    setListing({
                        ...listing.data,
                        address: {street: listing.data.address.street, city: listing.data.address.city, country: listing.data.address.country}
                    });
                });
            }
        });
    }

    function onSubmitCity(value : string) {
        if (!host) return;
        const validate = (value: string) => value.length < 100 && value.length > 0 && value !== listing?.address.city && value.match(/^ *$/) === null;
        if (!validate(value)) return;
        const newAddress: string = `${listing?.address.street}, ${value}, ${listing?.address.country}`;
        const newListing: ListingUpdateDTO = {...listing!, hostId: host.id, address: newAddress};
        listingService.update(id!, newListing).then((response) => {
            if(response.status === 200) {
                listingService.get(id!).then((listing) => {
                    setListing({
                        ...listing.data,
                        address: {street: listing.data.address.street, city: listing.data.address.city, country: listing.data.address.country}
                    });
                });
            }
        });
    }

    function onSubmitCountry(value : string) {
        if (!host) return;
        const validate = (value: string) => value.length < 100 && value.length > 0 && value !== listing?.address.country && value.match(/^ *$/) === null;
        if (!validate(value)) return;
        const newAddress: string = `${listing?.address.street}, ${listing?.address.city}, ${value}`;
        const newListing: ListingUpdateDTO = {...listing!, hostId: host.id, address: newAddress};
        listingService.update(id!, newListing).then((response) => {
            if(response.status === 200) {
                listingService.get(id!).then((listing) => {
                    setListing({
                        ...listing.data,
                        address: {street: listing.data.address.street, city: listing.data.address.city, country: listing.data.address.country}
                    });
                });
            }
        });
    }

    function onSubmitGuests(value: number) {
        if (!host) return;
        const newListing: ListingUpdateDTO = {...listing!, hostId: host.id, address: `${listing?.address.street}, ${listing?.address.city}, ${listing?.address.country}`, numberOfGuests: value};
        listingService.update(id!, newListing).then((response) => {
            if(response.status === 200) {
                setListing({...listing!, numberOfGuests: value});
            }
        });
    }

    function onSubmitRooms(value: number) {
        if (!host) return;
        const newListing: ListingUpdateDTO = {...listing!, hostId: host.id, address: `${listing?.address.street}, ${listing?.address.city}, ${listing?.address.country}`, numberOfRooms: value};
        listingService.update(id!, newListing).then((response) => {
            if(response.status === 200) {
                setListing({...listing!, numberOfRooms: value});
            }
        });
    }

    function onSubmitBathrooms(value: number) {
        if (!host) return;
        const newListing: ListingUpdateDTO = {...listing!, hostId: host.id, address: `${listing?.address.street}, ${listing?.address.city}, ${listing?.address.country}`, numberOfBathrooms: value};
        listingService.update(id!, newListing).then((response) => {
            if(response.status === 200) {
                setListing({...listing!, numberOfBathrooms: value});
            }
        });
    }

    function onSubmitAmenities(value : string[]) {
        if (!host) return;
        const newListing: ListingUpdateDTO = {...listing!, hostId: host.id, address: `${listing?.address.street}, ${listing?.address.city}, ${listing?.address.country}`, amenities: value};
        listingService.update(id!, newListing).then((response) => {
            if(response.status === 200) {
                setListing({...listing!, amenities: value});
            }
        });
    }


    function onSubmitProperty(value: PropertyType) {
        if (!host) return;
        const newListing: ListingUpdateDTO = {...listing!, hostId: host.id, address: `${listing?.address.street}, ${listing?.address.city}, ${listing?.address.country}`, propertyType: value};
        listingService.update(id!, newListing).then((response) => {
            if(response.status === 200) {
                setListing({...listing!, propertyType: value});
            }
        });
    }

    function onSubmitPrice(value: number) {
        if (!host) return;
        const validate = (value: number) => value > 15 && value !== listing?.pricePerNight && value < 10000;
        if (!validate(value)) return;
        const newListing: ListingUpdateDTO = {...listing!, hostId: host.id, address: `${listing?.address.street}, ${listing?.address.city}, ${listing?.address.country}`, pricePerNight: value};
        listingService.update(id!, newListing).then((response) => {
            if(response.status === 200) {
                setListing({...listing!, pricePerNight: value});
            }
        });
    }

    return listing && <div className={"listing-update-section"}>
        <div className="lus-header">
            <div className="lush-top">
                <div className="lush-title">{listing.title}</div>
                <button className="create-button">Preview listing</button>
            </div>
            <div className="change-listing">Change listing <LeftArrow/></div> 
        </div>
        <div className="lus-body">
            <div className="lus-body-content">
                <div className="left-sticky-nav-bar">
                    <div className="listing-details lus-nav-btn">
                        Listing details
                    </div>
                    <div className="details-list">
                        <div className={`d-list-item d-active`} onClick={() => gotoDetailGroup(0)}>Photos</div>
                        <div className={`d-list-item`} onClick={() => gotoDetailGroup(1)}>Listing basics</div>
                        <div className={`d-list-item`} onClick={() => gotoDetailGroup(2)}>Amenities</div>
                        <div className={`d-list-item`} onClick={() => gotoDetailGroup(3)}>Location</div>
                        <div className={`d-list-item`} onClick={() => gotoDetailGroup(4)}>Property and rooms</div>
                        <div className={`d-list-item`} onClick={() => gotoDetailGroup(5)}>Pricing</div>
                    </div>
                </div>
                <div className="right-content">
                    <ListingDetail name={"Photos"}></ListingDetail>
                    <ListingDetail name={"Listing basics"}>
                        <StringEditItem name={"Title"} value={listing.title} isValid={(value) => value.length < 32 } onSubmit={onSubmitTitle}></StringEditItem>
                        <StringEditItem name={"Description"} value={listing.description} isValid={(value) => value.length < 500} onSubmit={onSubmitDescription}></StringEditItem>
                        <CounterEditItem name={"Guests"} value={listing.numberOfGuests} onSubmit={onSubmitGuests}></CounterEditItem>
                    </ListingDetail>
                    <ListingDetail name={"Amenities"}>
                        <AmenitiesEditItem value={listing.amenities} onSubmit={onSubmitAmenities}/>
                    </ListingDetail>
                    <ListingDetail name={"Location"}>
                        <StringEditItem name={"Street"} value={listing.address.street} onSubmit={onSubmitStreet}></StringEditItem>
                        <StringEditItem name={"City"} value={listing.address.city} onSubmit={onSubmitCity}></StringEditItem>
                        <StringEditItem name={"Country"} value={listing.address.country} onSubmit={onSubmitCountry}></StringEditItem>
                    </ListingDetail>
                    <ListingDetail name={"Property and rooms"}>
                        <PropertyEditItem name={"Property type"} value={listing.propertyType} onSubmit={onSubmitProperty}></PropertyEditItem>
                        <CounterEditItem name={"Number of rooms"} value={listing.numberOfRooms} onSubmit={onSubmitRooms}></CounterEditItem>
                        <CounterEditItem name={"Number of bathrooms"} value={listing.numberOfBathrooms} onSubmit={onSubmitBathrooms}></CounterEditItem>
                    </ListingDetail>
                    <ListingDetail name={"Pricing"}>
                        <PriceEditItem name={"Price per night"} value={listing.pricePerNight} onSubmit={onSubmitPrice}/>
                    </ListingDetail>
                </div>
            </div>
        </div>
    </div>;
}