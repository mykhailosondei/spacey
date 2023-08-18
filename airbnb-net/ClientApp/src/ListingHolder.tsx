import React, { useState } from 'react';
import './ListingHolder.scss';
import ListingBox from "./ListingBox";

const ListingHolder : React.FC = () => {
    return (
        <>
            <div className={'listing-holder'}>
            <ListingBox title={"Crosslake, Minnesota, US"} distance={"420 km away"} availability={"Yes"} price={400} rating={4.96} pictures={['/mansion1.jpg', '/mansion1_1.jpg']}></ListingBox>
            <ListingBox title={"Crosslake, Minnesota, US"} distance={"420 km away"} availability={"Yes"} price={400} rating={4.96} pictures={['/mansion2.jpg']}></ListingBox>
            <ListingBox title={"Crosslake, Minnesota, US"} distance={"420 km away"} availability={"Yes"} price={400} rating={4.96} pictures={['/mansion3.jpg']}></ListingBox>
            <ListingBox title={"Crosslake, Minnesota, US"} distance={"420 km away"} availability={"Yes"} price={400} rating={4.96} pictures={['/mansion4.jpg']}></ListingBox>
                
            </div>
        </>
    )
        
    
}

export default ListingHolder;