import React from "react";
import {RatingsComponent as Ratings} from "./RatingsComponent";
import {Reviews} from "./Reviews";
import {ReviewDTO} from "../DTOs/Review/ReviewDTO";

const ratingsArray = [
        {
            "cleanliness": 3,
            "communication": 4,
            "checkIn": 2,
            "accuracy": 3,
            "location": 2,
            "value": 3
        },
        {
            "cleanliness": 4,
            "communication": 5,
            "checkIn": 4,
            "accuracy": 4,
            "location": 5,
            "value": 4
        },
        {
            "cleanliness": 2,
            "communication": 3,
            "checkIn": 1,
            "accuracy": 2,
            "location": 1,
            "value": 2
        },
        {
            "cleanliness": 5,
            "communication": 5,
            "checkIn": 3,
            "accuracy": 5,
            "location": 4,
            "value": 5
        },
        {
            "cleanliness": 3,
            "communication": 4,
            "checkIn": 2,
            "accuracy": 3,
            "location": 2,
            "value": 3
        },
        {
            "cleanliness": 4,
            "communication": 4,
            "checkIn": 3,
            "accuracy": 4,
            "location": 3,
            "value": 4
        },
        {
            "cleanliness": 2,
            "communication": 3,
            "checkIn": 1,
            "accuracy": 2,
            "location": 1,
            "value": 2
        },
        {
            "cleanliness": 5,
            "communication": 5,
            "checkIn": 4,
            "accuracy": 5,
            "location": 4,
            "value": 5
        },
        {
            "cleanliness": 3,
            "communication": 4,
            "checkIn": 2,
            "accuracy": 3,
            "location": 2,
            "value": 3
        },
        {
            "cleanliness": 4,
            "communication": 5,
            "checkIn": 3,
            "accuracy": 4,
            "location": 5,
            "value": 4
        }
    ]

;

interface ReviewsSectionProps {
    reviews: ReviewDTO[];
}

export const ReviewsSection = (props: ReviewsSectionProps) => {
    
    let ratingsArray = props.reviews.map((review) => {return review.ratings});
    
    return <div className="reviews-section">
        <Ratings ratingsArray={ratingsArray}></Ratings>
        <Reviews reviewsArray={props.reviews}></Reviews>
        <div className="horizontal-divider"></div>
    </div>;
};