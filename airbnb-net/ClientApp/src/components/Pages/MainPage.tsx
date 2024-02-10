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
            </Routes>
            <Outlet/>
            
        </SwitchToUser>
    </>
}