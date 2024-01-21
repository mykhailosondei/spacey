import React from "react";
import {ListingTableEntry} from "./ListingTableEntry";
import "../styles/ListingsTable.scss";
import ListingDTO from "../DTOs/Listing/ListingDTO";

interface ListingsTableProps {
    listings: ListingDTO[];
}

export const ListingsTable = (props:ListingsTableProps) => {
    return <>
        <table className="listings-table">
            <thead className="listings-table-header">
            <tr>
                <th className="listing-table-header-cell" data-column={"LISTING"}>Listing</th>
                <th className="listing-table-header-cell" data-column={"PRICE"}>Price</th>
                <th className="listing-table-header-cell" data-column={"LOCATION"}>Location</th>
                <th className="listing-table-header-cell" data-column={"BEDROOMS"}>Bedrooms</th>
                <th className="listing-table-header-cell" data-column={"BATHS"}>Beds</th>
                <th className="listing-table-header-cell" data-column={"GUESTS"}>Guests</th>
            </tr>
            </thead>
            <tbody className="listings-table-body">
                {props.listings.map((listing) => {return <ListingTableEntry listing={listing}></ListingTableEntry>})}
            </tbody>
        </table>
    </>;
}