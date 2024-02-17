import React from 'react';
import {ListingCreateDTO} from "../../DTOs/Listing/ListingCreateDTO";
import "../../styles/ListingCreatePage.scss";
import Logo from "../Icons/Logo";
import {Route, Routes} from "react-router-dom";
import {CreatorWrapper} from "../CreatorWrapper";
import {PropertyTypeCreator} from "./PropertyTypeCreator";

interface ICreatePage {
    route: string;
    creatorTitles?: { title: string, subtitle: string};
    component: JSX.Element;
}

export const ListingCreatePage = () => {
    
    const [listing, setListing] = React.useState<ListingCreateDTO>({} as ListingCreateDTO);
    
    const pages: ICreatePage[] = [
        //{route: "start", component: <StartReadonlyCreator listing={listing}/>},
        {route: "property-type", component: <PropertyTypeCreator listing={listing} setListing={setListing}/>, creatorTitles: {title: "What type of property is this?", subtitle: "What type of property is this?"}},
        //{route: "location", component: <LocationCreator listing={listing} setListing={setListing}/>},
        //{route: "floor-plan", component: <FloorPlanCreator listing={listing} setListing={setListing}/>},
        //{route: "stand-out", component: <StandOutReadonlyCreator listing={listing}/>},
        //{route: "amenities", component: <AmenitiesCreator listing={listing} setListing={setListing}/>},
        //{route: "photos", component: <PhotosCreator listing={listing} setListing={setListing}/>},
        //{route: "title", component: <TitleCreator listing={listing} setListing={setListing}/>},
        //{route: "description", component: <DescriptionCreator listing={listing} setListing={setListing}/>},
        //{route: "finish-setup", component: <FinishSetupReadonlyCreator listing={listing}/>},
        //{route: "price", component: <PriceCreator listing={listing} setListing={setListing}/>},
        //{route: "publish", component: <PublishCreator listing={listing}/>, readonly: true}
    ];
    
    const [currentPage, setCurrentPage] = React.useState(0);
    
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
                <div className="black-underline-hover-btn">Back</div>
                <div className="white-on-black-btn">Next</div>
            </div>
        </div>
    </div>
}