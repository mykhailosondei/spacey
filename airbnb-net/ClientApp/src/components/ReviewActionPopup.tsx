import React, {useEffect, useMemo} from "react";
import Cross from "./Icons/Cross";
import InputField from "./InputFIeld";
import {Ratings} from "../values/Ratings";
import Cleanliness from "./Icons/Ratings/Cleanliness";
import Communication from "./Icons/Ratings/Communication";
import Accuracy from "./Icons/Ratings/Accuracy";
import Location from "./Icons/Ratings/Location";
import CheckIn from "./Icons/Ratings/CheckIn";
import Value from "./Icons/Ratings/Value";
import {RatingSelector} from "./RatingSelector";
import {ReviewService} from "../services/ReviewService";
import {ReviewUpdateDTO} from "../DTOs/Review/ReviewUpdateDTO";
import {ReviewCreateDTO} from "../DTOs/Review/ReviewCreateDTO";
import {Simulate} from "react-dom/test-utils";
import select = Simulate.select;
import {ReviewDTO} from "../DTOs/Review/ReviewDTO";

interface ReviewActionPopupProps {
    bookingId: string;
    reviewId?: string;
    comment?: string;
    ratings?: Ratings;
    onClose: () => void;
    updateMode?: boolean
}

export const ReviewActionPopup = (props : ReviewActionPopupProps) => {
    
    const [comment, setComment] = React.useState<string>(props.comment || "");
    const [ratings, setRatings] = React.useState<Ratings>(props.ratings || {} as Ratings);
    const [invalid, setInvalid] = React.useState<boolean | null>(null);
    const [error, setError] = React.useState(false);
    const [review , setReview] = React.useState<ReviewDTO | null>(null);
    
    const reviewService = useMemo(() => {return ReviewService.getInstance()}, []);

    useEffect(() => {
        if(props.reviewId) {
            reviewService.get(props.reviewId).then((result) => {
                setReview(result.data);
                setComment(result.data.comment);
                setRatings(result.data.ratings);
            });
        }
    }, []);
    
    const avgRating = (ratings : Ratings) => {
        const result = (ratings.cleanliness + ratings.communication + ratings.accuracy + ratings.location + ratings.checkIn + ratings.value) / 6;
        return isNaN(result) ? 0 : result
    }
    
    const setRating = (rating: number, name: string) => {
        setRatings({...ratings, [name]: rating});
    }

    useEffect(() => {
        if(invalid == null){return;}
        if(comment.length > 0 && Object.keys(ratings).filter(rating => !!rating).length === 6) {
            setInvalid(false);
        }else{
            setInvalid(true);
        }
    }, [comment, ratings]);

    function onSubmit() {
        const valid = comment.length > 0 && Object.keys(ratings).filter(rating => !!rating).length === 6;
        setInvalid(!valid);
        if(valid) {
            console.log("Submit");
            if(props.updateMode) {
                reviewService.update(props.reviewId!, {comment: comment, ratings: ratings} as ReviewUpdateDTO).then((result) => {
                    if(result.status !== 200){
                        setError(true);
                    }
                    props.onClose();
                })}
            else {
                reviewService.create({bookingId: props.bookingId, comment: comment, ratings: ratings} as ReviewCreateDTO).then((result) => {
                    if(result.status !== 200){
                        setError(true);
                    }
                    props.onClose();
                });
            }
        }
    }

    return <>
        <div className={"update-popup-background"} onClick={props.onClose}></div>
        <div className={"update-popup"}>
            <div className="up-header margin-b20">
                <span onClick={props.onClose}>
                    <Cross/>
                </span>
                <h3>{props.updateMode ? "Update" : "Create"} your review</h3>
            </div>
            <div className="up-body">
                <div className="up-form">
                    <div className="up-form-row">
                        <InputField type={"text"} label={"Comment"} value={comment} onChange={(e) => setComment(e.target.value)} isValid={() => comment.length > 0} onInvalid={() => {}}></InputField>
                    </div>
                    <div className="up-form-row">
                        <div className="rating-container">
                            {(review || !props.updateMode) && <>
                                <RatingSelector name={"Cleanliness"}
                                                   rating={ratings.cleanliness}
                                                   element={<Cleanliness/>}
                                                   setRating={(rating) => setRating(rating, "cleanliness")}/>
                                <RatingSelector name={"Communication"}
                                                rating={ratings.communication}
                                                element={<Communication/>}
                                                setRating={(rating) => setRating(rating, "communication")}/>
                                <RatingSelector name={"Accuracy"}
                                                rating={ratings.accuracy}
                                                element={<Accuracy/>}
                                                setRating={(rating) => setRating(rating, "accuracy")}/>
                                <RatingSelector name={"Location"}
                                                rating={ratings.location}
                                                element={<Location/>}
                                                setRating={(rating) => setRating(rating, "location")}/>
                                <RatingSelector name={"CheckIn"}
                                                rating={ratings.checkIn}
                                                element={<CheckIn/>}
                                                setRating={(rating) => setRating(rating, "checkIn")}/>
                                <RatingSelector name={"Value"}
                                                rating={ratings.value}
                                                element={<Value/>}
                                                setRating={(rating) => setRating(rating, "value")}/>
                            </>}
                            <div className="rating-overall">Overall: {avgRating(ratings).toString().substring(0,4)}</div>
                        </div>
                    </div>
                    <div className="up-form-row">
                        <div className={"lower-button-container"}>
                            {error && <div className="error-message">An error occurred</div>}
                            <button className="clear-dates-button custom-width rap-submit" onClick={onSubmit}>{invalid == true ? "Please fill out all the fields" : "Submit"}</button>
                        </div>
                    </div>
                </div>
            </div> 
        </div>
    </>;
};