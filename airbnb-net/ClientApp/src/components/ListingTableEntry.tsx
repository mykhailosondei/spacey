import React from 'react';
import "../styles/ListingTableEntry.scss";
import ListingDTO from "../DTOs/Listing/ListingDTO";
import {Address} from "../values/Address";

interface ListingTableEntryProps {
    listing: ListingDTO;
}

export const ListingTableEntry = (props:ListingTableEntryProps) => {
    
    function columnImage() {
        if(props.listing.imagesUrls[0]){
            return <img className="listing-table-entry-image" src={props.listing.imagesUrls[0].url} alt={"Listing image"}/>
        }
        else {
            return <img className="listing-table-entry-image" src={"https://i.imgur.com/6tGnYMY.png"} alt={"Listing image"}/>
        }
    }
    
    return <tr className={"listing-row"}>
        <td className="listing-table-entry-data-cell" data-column={"LISTING"}>
            {columnImage()}
            <b>{props.listing.title}</b>
        </td>
        <td className="listing-table-entry-data-cell" data-column={"PRICE"}>{PriceFromListing(props.listing)}</td>
        <td className="listing-table-entry-data-cell" data-column={"LOCATION"}>{LocationNameFromAddress(props.listing.address)}</td>
        <td className="listing-table-entry-data-cell" data-column={"BEDROOMS"}>{props.listing.numberOfRooms}</td>
        <td className="listing-table-entry-data-cell" data-column={"BATHS"}>{props.listing.numberOfBathrooms}</td>
        <td className="listing-table-entry-data-cell" data-column={"GUESTS"}>{props.listing.numberOfGuests}</td>
    </tr>;
}

const LocationNameFromAddress = (address: Address) => {
    const cityName = address.city[0].toUpperCase() + address.city.slice(1).toLowerCase();
    const countryName = address.country[0].toUpperCase() + address.country.slice(1).toLowerCase();
    
    return cityName + ", " + countryName
}

const PriceFromListing = (listing: ListingDTO) => {
    return "$" + listing.pricePerNight;
}