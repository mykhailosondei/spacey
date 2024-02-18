import React, {useEffect} from 'react';
import {ListingCreateDTO} from "../../DTOs/Listing/ListingCreateDTO";
import "../../styles/ListingCreatePage.scss";
import Logo from "../Icons/Logo";
import {Route, Routes, useLocation, useNavigate} from "react-router-dom";
import {CreatorWrapper} from "../CreatorWrapper";
import {PropertyTypeCreator} from "../PropertyTypeCreator";
import {LocationCreator} from "../LocationCreator";
import {FloorPlanCreator} from "../FloorPlanCreator";
import {StandOutReadonlyCreator} from "../StandOutReadonlyCreator";
import {AmenitiesCreator} from "../AmenitiesCreator";
import {TitleCreator} from "../TitleCreator";
import {DescriptionCreator} from "../DescriptionCreator";
import {StartReadonlyCreator} from "../StartReadonlyCreator";
import {FinishSetupReadonlyCreator} from "../FinishSetupReadonlyCreator";
import {PriceCreator} from "../PriceCreator";
import {PublishCreator} from "../PublishCreator";

interface ICreatePage {
    route: string;
    creatorTitles?: { title: string, subtitle: string};
    component: JSX.Element;
}

function PhotosCreator(props: {
    setListing: (value: (((prevState: ListingCreateDTO) => ListingCreateDTO) | ListingCreateDTO)) => void,
    listing: ListingCreateDTO
}) {
    return <div></div>
}

export const ListingCreatePage = () => {
    
    const [listing, setListing] = React.useState<ListingCreateDTO>({} as ListingCreateDTO);
    
    const navigate = useNavigate();
    const location = useLocation();
    
    const pages: ICreatePage[] = [
        {route: "start", component: <StartReadonlyCreator listing={listing}/>},
        {route: "property-type", component: <PropertyTypeCreator listing={listing} setListing={setListing}/>, creatorTitles: {title: "What type of property best describe the place?", subtitle: ""}},
        {route: "location", component: <LocationCreator listing={listing} setListing={setListing}/>, creatorTitles: {title: "Where is your property located?", subtitle: "Your property's location is never shared with third parties."}},
        {route: "floor-plan", component: <FloorPlanCreator listing={listing} setListing={setListing}/>, creatorTitles: {title: "Share some basics about your place", subtitle: "Add a floor plan"}},
        {route: "stand-out", component: <StandOutReadonlyCreator listing={listing}/>},
        {route: "amenities", component: <AmenitiesCreator listing={listing} setListing={setListing}/>, creatorTitles: {title: "What amenities do you offer?", subtitle: "Select all that apply."}},
        {route: "photos", component: <PhotosCreator listing={listing} setListing={setListing}/>, creatorTitles: {title: "Photos", subtitle: "Showcase your space. You can always add more later."}},
        {route: "title", component: <TitleCreator listing={listing} setListing={setListing}/>, creatorTitles: {title: "Now, let's give your place a title", subtitle: "Make sure it's clear and descriptive. You can always change it later."}},
        {route: "description", component: <DescriptionCreator listing={listing} setListing={setListing}/>, creatorTitles: {title: "Describe your place", subtitle: "Help guests imagine staying at your place. Highlight what makes it special."}},
        {route: "finish-setup", component: <FinishSetupReadonlyCreator listing={listing}/>},
        {route: "price", component: <PriceCreator listing={listing} setListing={setListing}/>, creatorTitles: {title: "Now, set your price", subtitle: "You can change it anytime."}},
        {route: "publish", component: <PublishCreator listing={listing}/>}
    ];
    
    useEffect(() => {
        const lastRoute = location.pathname.split("/").pop();
        const index = pages.findIndex((page) => page.route == lastRoute);
        if(index === -1) {
            navigate(pages[0].route);
            setCurrentPage(0);
        }   else {
            setCurrentPage(index);
        }
    }, [location.pathname]);
    
    const [currentPage, setCurrentPage] = React.useState(0);

    
    const goToNextPage = () => {
        if (currentPage === pages.length - 1) return;
        navigate(pages[currentPage + 1].route);
        setCurrentPage(currentPage + 1);
    }
    
    const goToPreviousPage = () => {
        if (currentPage === 0) return;
        navigate(pages[currentPage - 1].route);
        setCurrentPage(currentPage - 1);
    }
    
    return <div className={"listing-create-page"}>
        <div className="lcp-header">
            <div className="lcp-header-content">
                <Logo/>
                <div className="black-underline-hover-btn">Save & exit</div> 
            </div>
        </div>
        <div className="lcp-body">
            <div className="lcp-body-content">
                <Routes>
                {pages.map((page) => {
                    return <Route 
                        path={page.route} 
                        element={!page.creatorTitles ? page.component : 
                            <CreatorWrapper 
                                title={page.creatorTitles.title} 
                                subtitle={page.creatorTitles.subtitle}>{page.component}
                            </CreatorWrapper>}>
                    </Route> 
                })}
                </Routes>
            </div>
        </div>
        <div className="lcp-footer">
            <div className="lcpf-progress-bar">
                <div className="lcpf-progress-bar-fill" style={{width: "10%"}}></div>
            </div>
            <div className="lcp-footer-content">
                <div className="black-underline-hover-btn" onClick={goToPreviousPage}>Back</div>
                <div className="white-on-black-btn" onClick={goToNextPage}>Next</div>
            </div>
        </div>
    </div>
}