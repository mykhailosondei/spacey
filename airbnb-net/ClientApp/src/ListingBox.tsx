import React, {MouseEventHandler, useRef, useState} from 'react';
import './ListingBox.scss';
import {ReactComponent as Like} from './like.svg'
import Arrow from "./Arrow";
import {ReactComponent as LeftArrow} from './left-arrow.svg'
import {ReactComponent as RightArrow} from './right-arrow.svg'
import {Easing, Group, Tween, update} from "@tweenjs/tween.js";

import {calculateNewValue, wait} from "@testing-library/user-event/dist/utils";
import {ReactComponent} from "*.svg";

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
    const [liked, setLiked] = useState(false);
    const [isLeftArrowVisible, setIsLeftArrowVisible] = useState(false);
    const [isRightArrowVisible, setIsRightArrowVisible] = useState(false);
    const pictureContainer = useRef<HTMLDivElement>(null);
    
    function OnListingBoxMouseEnter(){
        switch (currentPictureIndex){
            case 0: 
                setIsRightArrowVisible(true);
                break;
            case pictures.length-1:
                setIsLeftArrowVisible(true);
                break;
            default:
                setIsRightArrowVisible(true);
                setIsLeftArrowVisible(true);
        }
    }

    function OnListingBoxMouseLeave(){
        setIsRightArrowVisible(false);
        setIsLeftArrowVisible(false);
    }
    
    for(let i: number = 0; i<pictures.length; i++){
        pictureArray.push(<a className="image-ref">
            <div className="image-holder">
                <div className="image-container">
                    <img src={pictures[i]} alt={i.toString()+' picture'}></img>
                </div>
            </div>
        </a>)
    }
    
    const animate = (t: any = 0) => {
        update(t);
        window.requestAnimationFrame(animate);
    };
    
    function moveNextSlide(){
        if(currentPictureIndex >= pictures.length-1){
            return;
        }
        animate();
        
        const value = {margin: -100*(currentPictureIndex+1)}
        const tween = new Tween({margin: -100*currentPictureIndex}).to(value, 300)
            .onUpdate((value)=>{
                if(pictureContainer.current){
                    pictureContainer.current.style.transform = `translate(${value.margin}%)`
                }
            })
            .easing(Easing.Cubic.Out);
        setCurrentPictureIndex(currentPictureIndex+1);
        tween.start();
    }
    
    function movePreviousSlide(){
        if(currentPictureIndex <= 0){
            return;
        }
        animate();

        const value = {margin: -100*(currentPictureIndex-1)}
        const tween = new Tween({margin: -100*(currentPictureIndex)}).to(value, 300)
            .onUpdate((value)=>{
                if(pictureContainer.current){
                    pictureContainer.current.style.transform = `translate(${value.margin}%)`
                }
            })
            .easing(Easing.Cubic.Out);
        setCurrentPictureIndex(currentPictureIndex-1);
        tween.start();
    }
    
    function changeLikeStatus(){
        setLiked(!liked);
    }

    return (
        <div className="listing-box" onMouseEnter={OnListingBoxMouseEnter} onMouseLeave={OnListingBoxMouseLeave}>
            <div className="picture-slider">
                <div className="pictures-controls-and-window">
                    <div className="picture-controls">
                        <div className="content-scroller-box">
                            <div className="like-grid">
                                <div className="empty-part"></div>
                                <div className="like-holder">
                                    <div className="like"><button className="like-button" onClick={changeLikeStatus}>
                                        <Like></Like>    
                                    </button></div>
                                </div>
                            </div>
                            <div className="arrows-grid">
                                <div className="left-arrow">
                                    <Arrow svg={<LeftArrow />} isVisibleInitial={isLeftArrowVisible} onClickFunction={movePreviousSlide}></Arrow>
                                </div>
                                <div className="right-arrow">
                                    <Arrow svg={<RightArrow />} isVisibleInitial={isRightArrowVisible} onClickFunction={moveNextSlide}></Arrow>
                                </div>
                            </div>
                            <div className="bottom-points"></div>
                        </div>
                    </div>
                    <div className="window">
                        <div className="aspect-ratio">
                            <div className="window-dimension">
                                <div className="picture-container" ref={pictureContainer}>
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
