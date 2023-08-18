import React, {MouseEventHandler, useState} from 'react';
import './ListingBox.scss';
import {ReactComponent as Like} from './like.svg'

interface ListingBoxProps {
    title: string;
    distance: string;
    availability: string;
    price: number;
    rating: number;
    pictures: string[] | string;
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
    const pictureArray  = [];
    for(let i: number = 0; i<pictures.length; i++){
        pictureArray.push(<a className="image-ref">
            <div className="image-holder">
                <div className="image-container">
                    <img src={pictures[i]} alt={i.toString()+' picture'}></img>
                </div>
            </div>
        </a>)
    }

    const handleNextPicture = () => {
        setCurrentPictureIndex((prevIndex) =>
            prevIndex === pictures.length - 1 ? 0 : prevIndex + 1
        );
    };
    
    function alert_bop () {
        alert("bop")
    }

    return (
        <div className="listing-box">
            <div className="picture-slider">
                <div className="pictures-controls-and-window">
                    <div className="picture-controls">
                        <div className="content-scroller-box">
                            <div className="like-grid">
                                <div className="empty-part"></div>
                                <div className="like-holder">
                                    <div className="like"><button className="like-button" onClick={alert_bop}>
                                        <Like></Like>    
                                    </button></div>
                                </div>
                            </div>
                            <div className="arrows-grid"></div>
                            <div className="bottom-points"></div>
                        </div>
                    </div>
                    <div className="window">
                        <div className="aspect-ratio">
                            <div className="window-dimension">
                                <div className="picture-container">
                                    {pictureArray}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div className="listing-info">
                <div className="title">{title}</div>
                <span className="distance">{distance}</span>
                <span className="availability">{availability}</span>
                <b className="price">${price} <span className='font-weight-normal'>per night</span></b>
                <div className="rating">
                    <span className="star">&#9733;</span>
                    {rating}
                </div>
            </div>
        </div>
    );
};

export default ListingBox;
