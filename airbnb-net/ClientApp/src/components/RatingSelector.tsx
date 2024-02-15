import Cleanliness from "./Icons/Ratings/Cleanliness";
import React from "react";
import StarFilled from "./Icons/StarFilled";
import StarEmpty from "./Icons/StarEmpty";

interface RatingSelectorProps {
    name: string;
    element: JSX.Element;
    rating: number;
    setRating: (rating: number) => void;
}

export function RatingSelector(props: RatingSelectorProps) {
    
    const [rating, setRating] = React.useState<number>(props.rating);
    
    const _setRating = ( rating: number) => {
        setRating(rating);
        props.setRating(rating);
    }
    
    const stars = [];
    for (let i = 1; i <= 5; i++) {
        stars.push(<span key={i} className={"rating-star"} onMouseEnter={() => _setRating(i)} style={{right: `${220 - i*40}px`}} onClick={() => props.setRating(i)}>
            {i <= rating ? <StarFilled/> : <StarEmpty/>}
        </span>);
    }
    
    return <div className={"rating-selector"}>
        <div className="select-rating-container">
            {props.name}
            {props.element}
        </div>
        <div className="star-container">
            {stars}
        </div>
        <div className={"current-rating"}>
            <StarFilled/>
            {rating}</div>
    </div>;
}