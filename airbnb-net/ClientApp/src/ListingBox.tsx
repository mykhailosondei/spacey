import React, { useState } from 'react';
import './ListingBox.scss';

interface ListingBoxProps {
    title: string;
    distance: string;
    availability: string;
    price: number;
    rating: number;
    pictures: string[];
}

const ListingBox: React.FC<ListingBoxProps> = ({
                                                   title,
                                                   distance,
                                                   availability,
                                                   price,
                                                   rating,
                                                   pictures,
                                               }) => {
    const [currentPictureIndex, setCurrentPictureIndex] = useState(0);

    const handlePreviousPicture = () => {
        setCurrentPictureIndex((prevIndex) =>
            prevIndex === 0 ? pictures.length - 1 : prevIndex - 1
        );
    };

    const handleNextPicture = () => {
        setCurrentPictureIndex((prevIndex) =>
            prevIndex === pictures.length - 1 ? 0 : prevIndex + 1
        );
    };

    return (
        <div className="listing-box">
            <div className="picture-slider">
                <div className="picture-container">
                    <img src={pictures[currentPictureIndex]} alt="Listing" />
                <div className="arrow-container">
                    <div className="arrow arrow-left" onClick={handlePreviousPicture}>
                        &lt;
                    </div>
                    <div className="arrow arrow-right" onClick={handleNextPicture}>
                        &gt;
                    </div>
                </div>
                </div>
            </div>
            <div className="listing-info">
                <h2 className="title">{title}</h2>
                <p className="distance">{distance}</p>
                <p className="availability">{availability}</p>
                <p className="price">${price}</p>
                <div className="rating">
                    <span className="star">&#9733;</span>
                    {rating}
                </div>
            </div>
        </div>
    );
};

export default ListingBox;
