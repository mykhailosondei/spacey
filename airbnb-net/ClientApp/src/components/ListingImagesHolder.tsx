import {ImageDTO} from "../DTOs/Image/ImageDTO";
import "../styles/ListingImagesHolder.scss";

export const ListingImagesHolder = (props: { images: ImageDTO[] }) => {
    return <>
        <div className="listing-images-holder">
            <div className="padding-hack">
                <div className="front-image listing-page-image">
                    <img src={props.images[0].url} alt="front-image"/>
                </div>
            <div className="next-two-images">
                <div className="top-image listing-page-image">
                    <img src={props.images[1].url} alt="top-image"/>
                </div>
                <div className="bottom-image listing-page-image">
                    <img src={props.images[2].url} alt="bottom-image"/>
                </div>
            </div>
            <div className="next-two-images move-further">
                <div className="top-image listing-page-image">
                    <img src={props.images[3].url} alt="top-image"/>
                </div>
                <div className="bottom-image listing-page-image">
                    <img src={props.images[4].url} alt="bottom-image"/>
                </div>
            </div>
            </div>
        </div>
    </>;
}