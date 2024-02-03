import React, {useEffect, useRef, useState} from 'react';
import '../styles/ListingBox.scss';
import Arrow from "./Arrow";
import {ReactComponent as LeftArrow} from '../values/svgs/left-arrow.svg'
import {ReactComponent as RightArrow} from '../values/svgs/right-arrow.svg'
import {Easing, Tween, update} from "@tweenjs/tween.js";
import ListingDTO from "../DTOs/Listing/ListingDTO";

interface ListingBoxProps {
    listing: ListingDTO;
}

const ListingBox: React.FC<ListingBoxProps> = (props) => {
    const [currentPictureIndex, setCurrentPictureIndex] = useState(0);
    const [liked, setLiked] = useState(false);
    const [isLeftArrowVisible, setIsLeftArrowVisible] = useState(false);
    const [isRightArrowVisible, setIsRightArrowVisible] = useState(false);
    const bulletsArrayInitialState : {id:number, scale:number, opacity:0.6|1}[]= [];
    const [bulletsArrayState, setBulletsArrayState] = useState<{id:number, scale:number, opacity:0.6|1}[]>(bulletsArrayInitialState);
    const [htmlBulletsArrayState, setHtmlBulletsArrayState] = useState<JSX.Element[]>(()=>{
        const htmlBulletsArrayInitValue = [];
        for (let i: number = 0; i<props.listing.imagesUrls.length; i++){
            htmlBulletsArrayInitValue.push(
                <span className={"bullet-load"}></span>
            )
        }
        
        return htmlBulletsArrayInitValue;
    });
    
    const pictureArray  = [];
    
    const bulletsRef = useRef<HTMLDivElement>(null);
    const pictureContainer = useRef<HTMLDivElement>(null);
    
    
    for(let i: number = 0; i<props.listing.imagesUrls.length; i++){
        pictureArray.push(<a className="image-ref">
            <div className="image-holder">
                <div className="image-container">
                    <img src={props.listing.imagesUrls[i].url} alt={i.toString()+' picture'}></img>
                </div>
            </div>
        </a>)
        bulletsArrayInitialState.push({id:i, scale: 2/3, opacity: 0.6});
    }
    
    function renderBullets(){
        const htmlBulletsArrayValue = [];
        for (let i: number = 0; i<props.listing.imagesUrls.length; i++){
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
            if(currentPictureIndex>=props.listing.imagesUrls.length-3){
                if(bullet.id>=props.listing.imagesUrls.length-3) return {id:bullet.id, scale:1, opacity:bullet.opacity};
                if(bullet.id==props.listing.imagesUrls.length-4) return {id:bullet.id, scale:5/6, opacity:bullet.opacity};
                return {id:bullet.id, scale:2/3, opacity:bullet.opacity};
            }
            if(bullet.id<=currentPictureIndex+1 && bullet.id>=currentPictureIndex-1) return {id:bullet.id, scale:1, opacity:bullet.opacity};
            if(bullet.id==currentPictureIndex+2 || bullet.id==currentPictureIndex-2) return {id:bullet.id, scale:5/6, opacity:bullet.opacity};
            return {id:bullet.id, scale:2/3, opacity:bullet.opacity};
        });
        setBulletsArrayState(currentBulletScaleArrayState);
    }
    
    
    
    function ValidateArrowsVisibilityDependingOnIndex(){
        let rightArrowResult : boolean;
        let leftArrowResult : boolean;
        switch (currentPictureIndex){
            case 0: 
                rightArrowResult = props.listing.imagesUrls.length != 1;
                leftArrowResult = false;
                break;
            case props.listing.imagesUrls.length-1:
                rightArrowResult = false;
                leftArrowResult = true;
                break;
            default:
                rightArrowResult = true;
                leftArrowResult = true;
        }
        setIsRightArrowVisible(rightArrowResult);
        setIsLeftArrowVisible(leftArrowResult);
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
    
    const animate = () => {
        update();
        window.requestAnimationFrame(animate);
    };
    
    
    function leftArrowOnClickFunction(){
        movePreviousBullet();
        movePreviousSlide();
    }

    function rightArrowOnClickFunction(){
        moveNextBullet();
        moveNextSlide();
    }

    function moveNextBullet(){
        if(currentPictureIndex < 2){
            return;
        }
        if(currentPictureIndex>=props.listing.imagesUrls.length-3){
            return;
        }
        animate();
        const fromValue = {margin: -11*(currentPictureIndex - 2)}
        const toValue = {margin: -11*(currentPictureIndex - 1)}
        const tween = slidingTween(fromValue, toValue, Easing.Cubic.Out, bulletsRef, 'px');
        tween.start();
    }
    
    function movePreviousBullet(){
        if(currentPictureIndex <= 2){
            return;
        }
        if(currentPictureIndex>props.listing.imagesUrls.length-3){
            return;
        }
        animate();
        const fromValue = {margin: -11*(currentPictureIndex - 2)}
        const toValue = {margin: -11*(currentPictureIndex - 3)}
        const tween = slidingTween(fromValue, toValue, Easing.Cubic.Out, bulletsRef, 'px');
        tween.start();
    }
    
    function moveNextSlide(){
        if(currentPictureIndex >= props.listing.imagesUrls.length-1){
            return;
        }
        animate();
        const fromValue = {margin: -100*(currentPictureIndex)}
        const toValue = {margin: -100*(currentPictureIndex + 1)}
        const tween = slidingTween(fromValue, toValue, Easing.Cubic.Out, pictureContainer, '%')
        setCurrentPictureIndex((currentPictureIndex)=>currentPictureIndex+1);
        tween.start();
    }
    
       
    function movePreviousSlide(){
        if(currentPictureIndex <= 0){
            return;
        }
        animate();
        const fromValue = {margin: -100*(currentPictureIndex)}
        const toValue = {margin: -100*(currentPictureIndex - 1)}
        const tween = slidingTween(fromValue,toValue, Easing.Cubic.Out , pictureContainer, '%')
        setCurrentPictureIndex((currentPictureIndex)=>currentPictureIndex-1);
        tween.start();
    }
    
    
    function slidingTween(fromValue:{margin:number}, toValue : {margin:number}, easing:any, ref:React.RefObject<HTMLDivElement>, translateMetric:string){
        return new Tween(fromValue).to(toValue, 300)
            .onUpdate((value)=>{
                if(ref.current){
                    ref.current.style.transform = `translate(${value.margin}${translateMetric})`
                }
            })
            .easing(easing);
    }
    
    
    function changeLikeStatus(){
        setLiked(!liked);
    }

    const distance = () => {
        return "420 km";
    };
    const availability = () => {
        return "Available";
    };

    const ratings = () => {
        return "4.5 (18)";
    }

    function redirectToListingPage() {
        window.location.href = `/listing/${props.listing.id}`;
    }

    return (
        <div className="listing-box" onMouseEnter={ValidateArrowsVisibilityDependingOnIndex} onMouseLeave={OnListingBoxMouseLeave} onClick={redirectToListingPage}>
            <div className="picture-slider">
                <div className="pictures-controls-and-window">
                    <div className="picture-controls">
                        <div className="content-scroller-box">
                            <div className="like-grid">
                                <div className="empty-part"></div>
                                <div className="like-holder">
                                    <div className="like"><button className="like-button" onClick={changeLikeStatus}>
                                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32" style={{display: 'block', fill: (liked ? 'rgb(85, 26, 139)' : 'rgba(0,0,0,0.5)'), height: '24px' ,width: '24px' ,stroke: 'white' ,strokeWidth: '2px' ,overflow: 'visible'}} aria-hidden="true" role="presentation" focusable="false"><path d="M16 28c7-4.73 14-10 14-17a6.98 6.98 0 0 0-7-7c-1.8 0-3.58.68-4.95 2.05L16 8.1l-2.05-2.05a6.98 6.98 0 0 0-9.9 0A6.98 6.98 0 0 0 2 11c0 7 7 12.27 14 17z"></path></svg>   
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
                <div className="title">{props.listing.title}</div>
                <span className="distance">{distance()}</span>
                <span className="availability">{availability()}</span>
                <b className="price">${props.listing.pricePerNight} <span className='font-weight-normal'>per night</span></b>
                <div className="rating">
                    <span className="star">&#9733;</span>
                    {ratings()}
                </div>
            </div>
        </div>
    );
};

export default ListingBox;
