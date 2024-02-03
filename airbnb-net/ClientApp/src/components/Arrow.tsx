import React, {ReactComponentElement, ReactSVGElement, useState} from "react";
import '../styles/Arrow.scss'

interface ArrowProps{
    svg : ReactComponentElement<any>;
    isVisibleInitial:boolean;
    onClickFunction:Function;
}

const Arrow : React.FC<ArrowProps> = ({svg, isVisibleInitial, onClickFunction})  =>{
    
    return (
        <>
            <div className={"isVisible-"+isVisibleInitial}>
                <button className="arrow-button" onClick={(e)=>{
                    e.stopPropagation();
                    if(isVisibleInitial) {
                        onClickFunction();
                    }
                }}>
                    {svg}
                </button>
            </div>
        </>
    )
}

export default Arrow;