import NavBar from "../NavBar";
import {PropertyTypeSlider} from "../PropertyTypeSlider";
import GeneralListingHolder from "../GeneralListingHolder";
import {SwitchToUser} from "./SwitchToUser";
import {Outlet, Route, Routes} from "react-router-dom";
import React from "react";

export const MainPage = () => {
    
    return <>
        <SwitchToUser>
            <NavBar/>
            <PropertyTypeSlider/>
            <Routes>
                <Route path="/" element={<GeneralListingHolder/>}/>
                <Route path="/listings/propertyType/:propertyType" element={<GeneralListingHolder/>}/>
                <Route path="/listings/address" element={<GeneralListingHolder/>}></Route>
                <Route path="/listings/boundingBox" element={<GeneralListingHolder/>}></Route>
            </Routes>
            <Outlet/>
            
        </SwitchToUser>
    </>
}