import * as React from 'react';
import {Ratings} from "../values/Ratings";

import "../styles/Ratings.scss";
import Cleanliness from "../values/svgs/Ratings/src/components/Icons/Ratings/Cleanliness";
import Communication from "../values/svgs/Ratings/src/components/Icons/Ratings/Communication";
import Location from "../values/svgs/Ratings/src/components/Icons/Ratings/Location";
import CheckIn from "../values/svgs/Ratings/src/components/Icons/Ratings/CheckIn";
import Value from "../values/svgs/Ratings/src/components/Icons/Ratings/Value";
import {Accordion} from "react-bootstrap";
import Accuracy from "../values/svgs/Ratings/src/components/Icons/Ratings/Accuracy";

interface RatingsProps {
    ratingsArray: Ratings[];
}

export const RatingsComponent = (props:RatingsProps) => {
    
    const getOverallRating = (ratings:Ratings) => {
        const sum = ratings.cleanliness + ratings.communication + ratings.location + ratings.checkIn + ratings.value + ratings.accuracy;
        return sum / 6;
    }
    
    const getAllOverallRatings = (ratingsArray: Ratings[]) => {
        let sum = 0;
        ratingsArray.forEach(rating => {
            sum += getOverallRating(rating);
        });
        return sum / props.ratingsArray.length;
    }
    
    const getMeanOfRatings = (ratingsArray: Ratings[], qualityName:"Cleanliness"|"Communication"|"Location"|"Check-in"|"Value"|"Accuracy") => {
        let sum = 0;
        switch (qualityName) {
            case "Cleanliness":
                ratingsArray.forEach(rating => {
                    sum += rating.cleanliness;
                });
                break;
            case "Communication":
                ratingsArray.forEach(rating => {
                    sum += rating.communication;
                });
                break;
            case "Location":
                ratingsArray.forEach(rating => {
                    sum += rating.location;
                });
                break;
            case "Check-in":
                ratingsArray.forEach(rating => {
                    sum += rating.checkIn;
                });
                break;
            case "Value":
                ratingsArray.forEach(rating => {
                    sum += rating.value;
                });
                break;
            case "Accuracy":
                ratingsArray.forEach(rating => {
                    sum += rating.accuracy;
                });
                break;
        }
        return sum / ratingsArray.length;
    }
    
    const percentageOfOverallRatingThatIsWholeNumber = (desiredOverallRating:number, ratingsArray: Ratings[]) => {
        const wholeNumber = Math.ceil(desiredOverallRating);
        let overallRatingsCountMatchesWholeNumber = 0;
        ratingsArray.forEach(rating => {
            const overallRating = getOverallRating(rating);
            if (Math.ceil(overallRating) === wholeNumber) {
                overallRatingsCountMatchesWholeNumber++;
            }
        });
        return overallRatingsCountMatchesWholeNumber / ratingsArray.length;
    }
    
    return <div className="ratings">
        <div className="horizontal-divider"></div>
        <div className="overall-rating-header">
            <div className="overall-rating">{getAllOverallRatings(props.ratingsArray).toString().slice(0,4)}</div>
            <div className="overall-subheader">Guest rating</div> 
        </div>
        <div className="ratings-holder">
            <div className="ratings-breakdown">
                <div className="overall-rating-graph rc-rating">
                <div className="rating-header">Overall rating</div>
                <div className="ratings-bar">
                    5
                    <div className="ratings-bar-graphic"
                         style={{width: `${percentageOfOverallRatingThatIsWholeNumber(5, props.ratingsArray) * 100}%`}}></div>
                    </div>
                <div className="ratings-bar">
                    4
                    <div className="ratings-bar-graphic"
                         style={{width: `${percentageOfOverallRatingThatIsWholeNumber(4, props.ratingsArray) * 100}%`}}></div>
                    </div>
                <div className="ratings-bar">
                    3
                    <div className="ratings-bar-graphic"
                         style={{width: `${percentageOfOverallRatingThatIsWholeNumber(3, props.ratingsArray) * 100}%`}}></div>
                </div>
                <div className="ratings-bar">
                    2
                    <div className="ratings-bar-graphic"
                         style={{width: `${percentageOfOverallRatingThatIsWholeNumber(2, props.ratingsArray) * 100}%`}}></div>
                </div>
                <div className="ratings-bar">
                    1
                    <div className="ratings-bar-graphic"
                         style={{width: `${percentageOfOverallRatingThatIsWholeNumber(1, props.ratingsArray) * 100}%`}}></div>
                </div>
            </div>
                <div className="rc-rating">
                    <div className="rating-header">Cleanliness</div>
                    <div className="rating-value">{getMeanOfRatings(props.ratingsArray, "Cleanliness").toString().slice(0,3)}</div>
                    <Cleanliness></Cleanliness>
                </div>
                <div className="rc-rating">
                    <div className="rating-header">Communication</div>
                    <div className="rating-value">{getMeanOfRatings(props.ratingsArray, "Communication").toString().slice(0,3)}</div>
                    <Communication></Communication>
                </div>
                <div className="rc-rating">
                    <div className="rating-header">Location</div>
                    <div className="rating-value">{getMeanOfRatings(props.ratingsArray, "Location").toString().slice(0,3)}</div>
                    <Location></Location>
                </div>
                <div className="rc-rating">
                    <div className="rating-header">Check-in</div>
                    <div className="rating-value">{getMeanOfRatings(props.ratingsArray, "Check-in").toString().slice(0,3)}</div>
                    <CheckIn></CheckIn>
                </div>
                <div className="rc-rating">
                    <div className="rating-header">Value</div>
                    <div className="rating-value">{getMeanOfRatings(props.ratingsArray, "Value").toString().slice(0,3)}</div>
                    <Value></Value>
                </div>
                <div className="rc-rating">
                    <div className="rating-header">Accuracy</div>
                    <div className="rating-value">{getMeanOfRatings(props.ratingsArray, "Accuracy").toString().slice(0,3)}</div>
                    <Accuracy></Accuracy>
                </div>
            </div>
        </div>
        <div className="horizontal-divider"></div>
    </div>;
};