import React, {Suspense} from "react";
import "./../styles/ListingAmenities.scss";

const importSVG = (svgFileName:string) =>
    React.lazy(() =>
        import(`./Icons/${svgFileName}`).then((module) => ({
            default: module.default,
        }))
    );


export const ListingAmenities = (props: { amenities: string[] }) => {

    
    
    return <div>
        <h3>What this place offers</h3>
        <div className={"amenities"}>
            {props.amenities.map((amenity) => {
                if(amenity == "TV") amenity = "Tv";
                const SvgIcon = importSVG(amenity);
                return <div className={"amenity"}>
                    <Suspense fallback={<></>}><SvgIcon></SvgIcon></Suspense>
                    <span className={"amenity-name"}>{amenity}</span>
                </div>
            })}
        </div>
            <div className="create-button custom-width">
                Show all {props.amenities.length} amenities
            </div>
    </div>;
}