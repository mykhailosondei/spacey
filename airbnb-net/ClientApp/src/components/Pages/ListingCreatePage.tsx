import React, {useEffect, useMemo} from 'react';
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
import {ListingService} from "../../services/ListingService";

interface ICreatePage {
    route: string;
    creatorTitles?: { title: string, subtitle: string};
    component: JSX.Element;
    validation?: (listing: ListingCreateDTO) => boolean;
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
    const [isPublishing, setIsPublishing] = React.useState(false);
    
    const listingService = useMemo(() => { return ListingService.getInstance()}, []);
    
    const pages: ICreatePage[] = [
        {route: "start", component: <StartReadonlyCreator listing={listing}/>},
        {route: "property-type", 
            component: <PropertyTypeCreator listing={listing} setListing={setListing}/>, 
            creatorTitles: {title: "What type of property best describe the place?", subtitle: ""},
            validation: (listing) => listing.propertyType !== undefined},
        {route: "location",
            component: <LocationCreator listing={listing} setListing={setListing}/>,
            creatorTitles: {title: "Where is your property located?", subtitle: "Your property's location is never shared with third parties."},
            validation: (listing) => listing.address !== undefined && listing.address.match(/^ *$/) === null},
        {route: "floor-plan", 
            component: <FloorPlanCreator listing={listing} setListing={setListing}/>, 
            creatorTitles: {title: "Share some basics about your place", subtitle: "Add a floor plan"},
            validation: (listing) => listing.numberOfGuests > 0 && listing.numberOfRooms > 0},
        {route: "stand-out", component: <StandOutReadonlyCreator listing={listing}/>},
        {route: "amenities", 
            component: <AmenitiesCreator listing={listing} setListing={setListing}/>,
            creatorTitles: {title: "What amenities do you offer?", subtitle: "Select all that apply."},
            validation: (listing) => listing.amenities !== undefined},
        {route: "photos", component: <PhotosCreator listing={listing} setListing={setListing}/>, creatorTitles: {title: "Photos", subtitle: "Showcase your space. You can always add more later."}},
        {route: "title", 
            component: <TitleCreator listing={listing} setListing={setListing}/>, 
            creatorTitles: {title: "Now, let's give your place a title", subtitle: "Make sure it's clear and descriptive. You can always change it later."},
            validation: (listing) => listing.title !== undefined && listing.title.match(/^ *$/) === null && listing.title.length <= 32},
        {route: "description", 
            component: <DescriptionCreator listing={listing} setListing={setListing}/>,
            creatorTitles: {title: "Describe your place", subtitle: "Help guests imagine staying at your place. Highlight what makes it special."},
            validation: (listing) => listing.description !== undefined && listing.description.match(/^ *$/) === null && listing.description.length <= 500},
        {route: "finish-setup", component: <FinishSetupReadonlyCreator listing={listing}/>},
        {route: "price", 
            component: <PriceCreator listing={listing} setListing={setListing}/>,
            creatorTitles: {title: "Now, set your price", subtitle: "You can change it anytime."},
            validation: (listing) => listing.pricePerNight !== undefined && listing.pricePerNight > 15 && listing.pricePerNight < 10000},
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

    
    const handleNextClick = () => {
        if (currentPage === pages.length - 1) {
            console.log("Publishing...");
            setIsPublishing(true);
            listingService.create(listing).then(() => {
                setIsPublishing(false);
                navigate("/hosting/listings");
            });
            return;
        }
        navigate(pages[currentPage + 1].route);
        setCurrentPage(currentPage + 1);
    }
    
    const goToPreviousPage = () => {
        if (currentPage === 0) return;
        navigate(pages[currentPage - 1].route);
        setCurrentPage(currentPage - 1);
    }

    function isNextDisabled() {
        if(currentPage === pages.length - 1) return false;
        if (isPublishing) return true;
        if(pages[currentPage].validation) {
            console.log(pages[currentPage].validation!(listing));
            return !pages[currentPage].validation!(listing);
        }
        return false;
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
                <Routes location={location} key={location.pathname}>
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
                <div className="lcpf-progress-bar-fill" style={{width: `${100 * currentPage / (pages.length - 1)}%`}}></div>
            </div>
            <div className="lcp-footer-content">
                <div className="black-underline-hover-btn" onClick={goToPreviousPage}>Back</div>
                <button className={`white-on-black-btn next-button ${currentPage === pages.length - 1 ? "publish" : ""} ${isPublishing ? "publishing" : ""}`} onClick={handleNextClick} disabled={isNextDisabled()}>{currentPage === pages.length - 1 ? "Publish" : "Next"}</button>
            </div>
        </div>
    </div>
}