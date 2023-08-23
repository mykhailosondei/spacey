import React, {MouseEventHandler, useEffect, useRef, useState} from 'react';
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
    const bulletsRef = useRef<HTMLDivElement>(null);
    const bulletsArrayInitialState : {id:number, scale:number, opacity:0.6|1}[]= [];
    
    
    for(let i: number = 0; i<pictures.length; i++){
        pictureArray.push(<a className="image-ref">
            <div className="image-holder">
                <div className="image-container">
                    <img src={pictures[i]} alt={i.toString()+' picture'}></img>
                </div>
            </div>
        </a>)
        bulletsArrayInitialState.push({id:i, scale: 2/3, opacity: 0.6});
    }
    
    const [bulletsArrayState, setBulletsArrayState] = useState<{id:number, scale:number, opacity:0.6|1}[]>(bulletsArrayInitialState);
    
    const [htmlBulletsArrayState, setHtmlBulletsArrayState] = useState<JSX.Element[]>(()=>{
        const htmlBulletsArrayInitValue = [];
        for (let i: number = 0; i<pictures.length; i++){
            htmlBulletsArrayInitValue.push(
                <span className={"bullet"}></span>
            )
        }
        
        return htmlBulletsArrayInitValue;
    });
    
    function renderBullets(){
        const htmlBulletsArrayValue = [];
        for (let i: number = 0; i<pictures.length; i++){
            htmlBulletsArrayValue.push(
                <span className={"bullet" +" "+ ((bulletsArrayState[i].opacity == 1) ? "opacity-high" : "opacity-low") + " " + ((bulletsArrayState[i].scale == 1)? "scale-high":((bulletsArrayState[i].scale == 5/6)? "scale-mid":"scale-low"))}></span>
            )
        }
        setHtmlBulletsArrayState(htmlBulletsArrayValue);
    }
    
    function ValidateBulletsViewDependingOnIndex(){
        const currentBulletHighlightedArrayState : {id:number, scale:number, opacity:0.6|1}[] = bulletsArrayState.map(bullet=>{
            if(currentPictureIndex==bullet.id){
                return {id:bullet.id, scale:bullet.scale, opacity:1};
            }
            else{
                return {id:bullet.id, scale:bullet.scale, opacity:0.6};
            }
        });
        const currentBulletScaleArrayState : {id:number, scale:number, opacity:0.6|1}[] = currentBulletHighlightedArrayState.map(bullet=>{
            if(currentPictureIndex<=2){
                if(bullet.id<=2) return {id:bullet.id, scale:1, opacity:bullet.opacity};
                if(bullet.id==3) return {id:bullet.id, scale:5/6, opacity:bullet.opacity};
                return {id:bullet.id, scale:2/3, opacity:bullet.opacity};
            }
            if(currentPictureIndex>=pictures.length-3){
                if(bullet.id>=pictures.length-3) return {id:bullet.id, scale:1, opacity:bullet.opacity};
                if(bullet.id==pictures.length-4) return {id:bullet.id, scale:5/6, opacity:bullet.opacity};
                return {id:bullet.id, scale:2/3, opacity:bullet.opacity};
            }
            if(bullet.id<=currentPictureIndex+1 && bullet.id>=currentPictureIndex-1) return {id:bullet.id, scale:1, opacity:bullet.opacity};
            if(bullet.id==currentPictureIndex+2 || bullet.id==currentPictureIndex-2) return {id:bullet.id, scale:5/6, opacity:bullet.opacity};
            return {id:bullet.id, scale:2/3, opacity:bullet.opacity};
        });
        console.log(JSON.stringify(currentBulletScaleArrayState));
        setBulletsArrayState(currentBulletScaleArrayState);
    }
    
    
    
    function ValidateArrowsVisibilityDependingOnIndex(){
        switch (currentPictureIndex){
            case 0: 
                setIsRightArrowVisible(true);
                setIsLeftArrowVisible(false);
                break;
            case pictures.length-1:
                setIsLeftArrowVisible(true);
                setIsRightArrowVisible(false);
                break;
            default:
                setIsRightArrowVisible(true);
                setIsLeftArrowVisible(true);
        }
    }

    useEffect(() => {
        renderBullets();
    }, [bulletsArrayState]);
    
    useEffect(()=>{
        ValidateArrowsVisibilityDependingOnIndex();
        ValidateBulletsViewDependingOnIndex();
    },[currentPictureIndex]);

    function OnListingBoxMouseLeave(){
        setIsRightArrowVisible(false);
        setIsLeftArrowVisible(false);
    }
    
    const animate = (t: any = 0) => {
        update(t);
        window.requestAnimationFrame(animate);
    };
    
    
    function leftArrowOnClickFunction(){
        //movePreviousBullet();
        movePreviousSlide();
    }

    function rightArrowOnClickFunction(){
        //moveNextBullet();
        moveNextSlide();
    }

    function moveNextBullet(){
        if(currentPictureIndex >= pictures.length-1){
            return;
        }
        animate();
        const fromValue = {margin: -11*(currentPictureIndex)}
        const toValue = {margin: -11*(currentPictureIndex+1)}
        const tween = slidingTween(fromValue, toValue, Easing.Cubic.Out, bulletsRef);
        setCurrentPictureIndex((currentPictureIndex)=>currentPictureIndex+1);
        tween.start();
    }
    
    function movePreviousBullet(){
        if(currentPictureIndex >= pictures.length-1){
            return;
        }
        animate();
        const fromValue = {margin: -11*(currentPictureIndex)}
        const toValue = {margin: -11*(currentPictureIndex-1)}
        const tween = slidingTween(fromValue, toValue, Easing.Cubic.Out, bulletsRef);
        setCurrentPictureIndex((currentPictureIndex)=>currentPictureIndex+1);
        tween.start();
    }
    
    function moveNextSlide(){
        if(currentPictureIndex >= pictures.length-1){
            return;
        }
        animate();
        const fromValue = {margin: -100*(currentPictureIndex)}
        const toValue = {margin: -100*(currentPictureIndex+1)}
        const tween = slidingTween(fromValue, toValue, Easing.Cubic.Out, pictureContainer)
        setCurrentPictureIndex((currentPictureIndex)=>currentPictureIndex+1);
        tween.start();
    }
    
    function movePreviousSlide(){
        if(currentPictureIndex <= 0){
            return;
        }
        animate();
        const fromValue = {margin: -100*(currentPictureIndex)}
        const toValue = {margin: -100*(currentPictureIndex-1)}
        const tween = slidingTween(fromValue,toValue, Easing.Cubic.Out , pictureContainer)
        setCurrentPictureIndex((currentPictureIndex)=>currentPictureIndex-1);
        tween.start();
    }
    
    
    
    function slidingTween(fromValue:{margin:number}, toValue : {margin:number}, easing:any, ref:React.RefObject<HTMLDivElement>){
        return new Tween(fromValue).to(toValue, 300)
            .onUpdate((value)=>{
                if(ref.current){
                    ref.current.style.transform = `translate(${value.margin}%)`
                }
            })
            .easing(easing);
    }
    
    
    function changeLikeStatus(){
        setLiked(!liked);
    }

    return (
        <div className="listing-box" onMouseEnter={ValidateArrowsVisibilityDependingOnIndex} onMouseLeave={OnListingBoxMouseLeave}>
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
                                    <Arrow svg={<LeftArrow />} isVisibleInitial={isLeftArrowVisible} onClickFunction={leftArrowOnClickFunction}></Arrow>
                                </div>
                                <div className="right-arrow">
                                    <Arrow svg={<RightArrow />} isVisibleInitial={isRightArrowVisible} onClickFunction={rightArrowOnClickFunction}></Arrow>
                                </div>
                            </div>
                            <div className="bottom-points">
                                <div className="justify-self-start"></div>
                                <div className="justify-self-center">
                                    <div className="bullets-holder">
                                        <div className="bullets-container">
                                            <div className="bullets" ref={bulletsRef}>
                                                {htmlBulletsArrayState}
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div className="justify-self-end"></div>
                            </div>
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
