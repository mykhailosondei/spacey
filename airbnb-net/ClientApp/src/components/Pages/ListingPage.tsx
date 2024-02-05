import React, {useMemo} from "react";
import ListingDTO from "../../DTOs/Listing/ListingDTO";
import NavBar from "../NavBar";
import "../../styles/ListingPage.scss";
import {ListingImagesHolder} from "../ListingImagesHolder";
import {ListingInfo} from "../ListingInfo";
import {BookingWindow} from "../BookingWindow";
import {HostDTO} from "../../DTOs/Host/HostDTO";
import {HostService} from "../../services/HostService";
import {ListingAmenities} from "../ListingAmenities";
import {CalendarSelector} from "../CalendarSelector";
import CalendarSelectorPopupProvider from "../../Contexts/CalendarSelectorPopupProvider";
import {ReviewsSection} from "../ReviewsSection";
import {ListingService} from "../../services/ListingService";
import {BookingService} from "../../services/BookingService";
import {ReviewService} from "../../services/ReviewService";
import {ReviewDTO} from "../../DTOs/Review/ReviewDTO";
import {useParams} from "react-router-dom";
import Heart from "../Icons/Heart";
import {UserService} from "../../services/UserService";


export const ListingPage = () => {
    
    const {id} = useParams();
    
    const [listing, setListing] = React.useState<ListingDTO>({} as ListingDTO);
    const [listingLoaded, setListingLoaded] = React.useState<boolean>(false);
    
    const [saved, setSaved] = React.useState<boolean>(false);
    
    const [host, setHost] = React.useState<HostDTO>({} as HostDTO);
    const hostService = useMemo(() => {return  HostService.getInstance()}, []);
    
    const [reviews, setReviews] = React.useState<ReviewDTO[]>([]);
    const [reviewsLoaded, setReviewsLoaded] = React.useState<boolean>(false);
    
    const listingService = useMemo(() => {return  ListingService.getInstance()}, []);
    const bookingService = useMemo(() => {return  BookingService.getInstance()}, []);
    const userService = useMemo(() => {return  UserService.getInstance()}, []);
    const reviewsService = useMemo(() => {return  ReviewService.getInstance()}, []);
    
    React.useEffect(() => {
        listingService.get(id!).then((listing) => {
            setListing(listing.data);
            setListingLoaded(true);
            userService.getFromToken().then((user) => {
                if(user){
                    setSaved(listing.data.likedUsersIds.includes(user.data.id));
                } 
            });
            bookingService.getMany(listing.data.bookingsIds).then((bookings) => {
                let reviews = bookings.map((booking) => {
                   if (booking.review) {
                          return booking.review;
                   }else {
                       return {} as ReviewDTO;
                   }
                }).filter((review) => {return (review.id !== undefined)});
                setReviews(reviews);
                setReviewsLoaded(true);
            });
        });
        userService.getFromToken().then((user) => {
           if(user.data){
                setSaved(user.data.likedListingsIds.includes(id!));
           }
        });
    }, []);

    function handleSaveClick() {
        setSaved(!saved);
        if(saved) {
            listingService.unlike(id!).then((response) => {
                if(response.status === 200) return;
                setSaved(true);
            });
        }else {
            listingService.like(id!).then((response) => {
                if(response.status === 200) return;
                setSaved(false);
            });
        }
    }

    return listingLoaded ? <>
    <div className={"page-navbar-wrapper"}><NavBar></NavBar></div>
        <div className={"listing-page"}>
            <div className="title-and-save-button">
                <h1 className="listing-title">{listing.title}</h1>
                <button className="save-button" onClick={handleSaveClick}>
                    <Heart fill={saved ? "red" : "none"}></Heart>
                    Save
                </button>
            </div>
            <ListingImagesHolder images={listing.imagesUrls}></ListingImagesHolder>
            <div className="listing-info-and-booking-window">
                <div className="listing-left-side">
                    <ListingInfo listing={listing}></ListingInfo>
                    <ListingAmenities amenities={listing.amenities}></ListingAmenities>
                    <CalendarSelector listing={listing}></CalendarSelector>
                </div>
                <CalendarSelectorPopupProvider><BookingWindow listing={listing}></BookingWindow></CalendarSelectorPopupProvider>
            </div>
                {reviewsLoaded && <ReviewsSection reviews={reviews}></ReviewsSection>}
            {/*<HostInfoSection></HostInfoSection>*/}
        </div>
    </> : <></>;
}