import {ListingCreateDTO} from "../DTOs/Listing/ListingCreateDTO";
import "../styles/PublishCreator.scss";
import {useNavigate} from "react-router-dom";
import {useEffect} from "react";
import StarFilled from "./Icons/StarFilled";

export const PublishCreator = (props: { listing: ListingCreateDTO }) => {
    
    const navigate = useNavigate();

    useEffect(() => {
        console.log(props.listing)
        if(props.listing.propertyType === undefined || props.listing.propertyType === null) {
            navigate("/create-listing/property-type");
            return;
        }
        if(!props.listing.address) {
            navigate("/create-listing/location");
            return;
        }
        if(!props.listing.amenities) {
            navigate("/create-listing/amenities");
            return;
        }
        if(!props.listing.imagesUrls || props.listing.imagesUrls.length < 5) {
            navigate("/create-listing/photos");
            return;
        }
        if(!props.listing.title) {
            navigate("/create-listing/title");
            return;
        }
        if(!props.listing.description) {
            navigate("/create-listing/description");
            return;
        }
        if(!props.listing.pricePerNight) {
            navigate("/create-listing/price");
            return;
        }
    }, []);
    
    return <div className="publish-creator">
        <div className="top-title">It's time to publish!</div>
        <div className="readonly-creator-subtitle">Here's what we'll show to guests. Before you publish, make sure to review the details.</div>
        <div className="publish-creator-content">
            <div className="listing-publish-card">
                <div className="lpc-image">
                    <img src={props.listing.imagesUrls[0].url} alt=""/>
                </div>
                <div className="lpc-bottom">
                    <div className="title-and-review">
                        <span>
                            {props.listing.title}
                        </span>
                        <span>New <StarFilled/></span>
                    </div>
                    <div className="lcp-price">
                        <b>
                            $
                            {props.listing.pricePerNight}
                        </b>
                        &nbsp;night
                    </div> 
                </div>
            </div>
        </div>
    </div>
};