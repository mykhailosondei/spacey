import {ReviewDTO} from "../DTOs/Review/ReviewDTO";
import {ReactComponent as DefaultAvatar} from "../values/svgs/default-profile.svg";
import React from "react";

import "../styles/Reviews.scss";
import {UserDTO} from "../DTOs/User/UserDTO";
import {UserService} from "../services/UserService";
import {Address} from "../values/Address";

interface ReviewsProps {
    reviewsArray: ReviewDTO[];
}

export const Reviews = (props:ReviewsProps) => {
    
    const [users, setUsers] = React.useState<UserDTO[]>([]);
    const userService = React.useMemo(() => {return UserService.getInstance()}, []);
    const [usersLoaded, setUsersLoaded] = React.useState<boolean>(false);
    
    React.useEffect(() => {
        userService.getMany(props.reviewsArray.map((review) => {return review.userId})).then((users) => {
            setUsers(users);
            setUsersLoaded(true);
        });
    }, []);
    
    const getUser = (userId: string) => {
        console.log(users.find((user) => {return user.id === userId}));
        return users.find((user) => {return user.id === userId});
    }
    
    const formatAddress = (address: Address) => {
        return `${address.city}, ${address.country}`;
    }
    
    const formatDate = (date: Date) => {
        if(date === undefined) {
            return "";
        }
        if(new Date().getTime() - date.getTime() < 86400000) {
            return "Today";
        }
        if(new Date().getTime() - date.getTime() < 172800000) {
            return "Yesterday";
        }
        if(new Date().getTime() - date.getTime() < 604800000) {
            return "This week";
        }
        if(new Date().getTime() - date.getTime() < 2592000000) {
            return "This month";
        }
        return date.toLocaleDateString();
    }
    
    return <div className="reviews">
        {usersLoaded && props.reviewsArray.map((review) => {
            const user = getUser(review.userId)!;
            return <div className="review">
                <div className="user-info">
                    <div className="user-image">
                        <DefaultAvatar/>
                    </div>
                    <div className="user-name-and-address">
                        <b>{user.name}</b>
                        {formatAddress(user.address) }
                    </div>
                </div>
                <div className="rating-and-date">
                    <div className="review-rating"></div>
                    <b className="review-date">{formatDate(new Date(review.createdAt))}</b>
                </div>
                <div className="review-body">
                    <p>{review.comment}</p>
                </div>
            </div>
        })}
    </div>;
};