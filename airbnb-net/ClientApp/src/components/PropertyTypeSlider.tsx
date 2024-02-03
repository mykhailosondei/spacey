import "../styles/PropertyTypeSlider.scss";
import PropertyTypeItem from "./PropertyTypeItem";
import {createContext, useEffect, useLayoutEffect, useState} from "react";
import {propertyTypeDictionary} from "../values/PropertyTypeTitles";
import {createWriteStream} from "node:fs";
import {PropertyType} from "../values/PropertyType";

export const PropertyTypeSlider : React.FC = () => {
    const propertyTypes = ["House", "Apartment", "Condo", "Townhouse", "Studio", "Mansion", "Cottage", "Castle", "Treehouse", "Boat", "RV", "Tent", "Villa", "Bungalow", "Loft", "Farmhouse", "Chalet", "Cabin", "Other"];
    const [selectedPropertyTypeIndex, setSelectedPropertyTypeIndex] = useState<number>(0);
    
    const [scrollerWindowWidth, setScrollerWindowWidth] = useState<number>(0);
    const [scrollerItemsWidth, setScrollerItemsWidth] = useState<number>(0);
    const [scrollerOffset, setScrollerOffset] = useState<number>(0);
    
    useEffect(() => {
        console.log("Window width: " + scrollerWindowWidth);
    }, [scrollerWindowWidth]);

    useLayoutEffect(() => {
        function updateSize() {
            const boundingBox = document.getElementsByClassName("scroller-grid")[0].getBoundingClientRect();
            setScrollerWindowWidth(boundingBox.right - boundingBox.left);
        }
        window.addEventListener('resize', updateSize);
        updateSize();
        return () => window.removeEventListener('resize', updateSize);
    }, []);

    useEffect(() => {
        const scrollerItemsCount = propertyTypes.length;
        const propItemsCollection : Element[] = Array.prototype.slice.call(document.getElementsByClassName("items-holder"));
        const scrollerItemsWidth = propItemsCollection.reduce((accumulator, currentValue) => accumulator + currentValue.getBoundingClientRect().right - currentValue.getBoundingClientRect().left, 0);
        setScrollerItemsWidth(scrollerItemsWidth + 32*(scrollerItemsCount-1));
        }, []);

    useEffect(() => {
        const propItemsCollection : Element[] = Array.prototype.slice.call(document.getElementsByClassName("items-holder"));
        for (var i = 0; i < propItemsCollection.length; i++){
            var element = propItemsCollection[i] as HTMLElement;
            element.style.transform = "translateX(-" + scrollerOffset + "px)";
        }
        console.log("Scroller offset: " + scrollerOffset);
    }, [scrollerOffset]);
    
    function updateIndex(title : string){
        setSelectedPropertyTypeIndex(propertyTypes.indexOf(title));
    }
    
    const propertyTypeItems = propertyTypes.map((propertyType, index) => {
       return <PropertyTypeItem onClick={updateIndex} title={propertyType} isSelected={index === selectedPropertyTypeIndex} href={'/'}></PropertyTypeItem> 
    });

    function scrollRight() {
        var newOffset = scrollerOffset + Math.min(scrollerWindowWidth * 0.7, scrollerItemsWidth - scrollerWindowWidth - scrollerOffset);
        setScrollerOffset(newOffset);
    }

    function scrollLeft() {
        var newOffset = scrollerOffset - Math.min(scrollerWindowWidth * 0.7, scrollerOffset);
        setScrollerOffset(newOffset);
    }
    
    const isLeftArrowVisible = () =>{ return scrollerOffset > 0;}
    
    const isRightArrowVisible = () =>{ return scrollerOffset < scrollerItemsWidth - scrollerWindowWidth;}

    return <>
        <div className={'property-type-slider'}>
            <div className={'side-padding'}>
                <div className={'prop-type-content-and-filter'}>
                    <div style={{display:"contents"}}>
                        <div className={'chipsbar-grid'}>
                            <div className="chipsbar-grid-content">
                                <div className="scroller">
                                    <div className="left-scroller-arrow-container" style={isLeftArrowVisible() ? {}:{display:"none"}}>
                                        <div className="fading-left-container">
                                            <button className="left-scroller-arrow-button" onClick={scrollLeft}>
                                                <span>
                                                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32"
                                                            style={{
                                                                display: 'block',
                                                                fill: 'none',
                                                                height: '12px',
                                                                width: '12px',
                                                                stroke: 'currentcolor',
                                                                strokeWidth: "5.3333px",
                                                                overflow: 'visible'
                                                            }}
                                                            aria-hidden="true" role="presentation" focusable="false">
                                                                                                    <path fill="none"
                                                                                                          d="M20 28 8.7 16.7a1 1 0 0 1 0-1.4L20 4"></path>
                                                                                                </svg>
                                                </span>
                                            </button>
                                        </div>
                                    </div>
                                    <div className="scroller-content">
                                        <div className="scroller-content-inner">
                                            <div className="scroller-grid">
                                                {propertyTypeItems}
                                            </div>
                                        </div>
                                    </div>
                                    <div className="right-scroller-arrow-container">
                                        <div className="fading-right-container">
                                            <button className="right-scroller-arrow-button" onClick={scrollRight} style={isRightArrowVisible() ? {}:{display:"none"}}>
                                                <span>
                                                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32"
                                                            style={{
                                                                display: 'block',
                                                                fill: 'none',
                                                                height: '12px',
                                                                width: '12px',
                                                                stroke: 'currentcolor',
                                                                strokeWidth: "5.3333px",
                                                                overflow: 'visible'
                                                            }}
                                                            aria-hidden="true" role="presentation" focusable="false">
                                                                                                    <path fill="none"
                                                                                                          d="m12 4 11.3 11.3a1 1 0 0 1 0 1.4L12 28"></path>
                                                                                                </svg>
                                                </span>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                                <div className="filter">
                                    <div className="filter-button-left-padding">
                                        <div className="filter-button-holder">
                                            <button className="filter-button">
                                                <span className="filter-button-content">
                                                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32"
                                                         className="filter-button-icon"
                                                         aria-hidden="true" role="presentation" focusable="false"><path
                                                        fill="none"
                                                        d="M7 16H3m26 0H15M29 6h-4m-8 0H3m26 20h-4M7 16a4 4 0 1 0 8 0 4 4 0 0 0-8 0zM17 6a4 4 0 1 0 8 0 4 4 0 0 0-8 0zm0 20a4 4 0 1 0 8 0 4 4 0 0 0-8 0zm0 0H3"></path></svg>
                                                    <span className="filter-button-text">Filters</span>
                                                </span>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </>;
}