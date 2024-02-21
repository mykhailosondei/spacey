import {ListingCreateDTO} from "../../DTOs/Listing/ListingCreateDTO";
import React, {useMemo} from "react";
import Photos from "../Icons/Photos";
import {ImageUploadService} from "../../services/ImageUploadService";
import {ImageDTO} from "../../DTOs/Image/ImageDTO";
import Photo from "../Icons/Photo";
import Plus from "../Icons/Plus";
import Trash from "../Icons/Trash";
import {generateUniqueID} from "web-vitals/dist/modules/lib/generateUniqueID";
import {randomUUID} from "node:crypto";

export const PhotosCreator = (props: {
    setListing: (value: (((prevState: ListingCreateDTO) => ListingCreateDTO) | ListingCreateDTO)) => void,
    listing: ListingCreateDTO
}) => {
    
    const imageUploadService = useMemo(() => { return ImageUploadService.getInstance()}, []);
    
    
    const imagesEmptyOrUndefined = () =>
    {
        console.log(props.listing.imagesUrls);
        return props.listing.imagesUrls === undefined || props.listing.imagesUrls.length === 0;
    }
    
    const onFileSelected = (event: React.ChangeEvent<HTMLInputElement>) => {
        if(event.target.files){
            console.log(event.target.files);
            imageUploadService.uploadMany(Array.from(event.target.files)).then((result) => {
                const results = [] as string[];
                for (let i = 0; i < result.length; i++) {
                    if(result[i].status === 200){
                        results.push(result[i].data);
                    }
                }
                
                const newImagesArray : ImageDTO[]  = results.map((value) => {return {url: value}}).slice(0, 10);
                
                props.setListing((prevState) => {
                    return {...prevState, imagesUrls: 
                            prevState.imagesUrls ? prevState.imagesUrls.concat(newImagesArray).slice(0, 10) : newImagesArray
                    };
                });
            });
        }
    }

    function selectImage() {
        const input = document.createElement('input');
        input.type = 'file';
        input.accept = 'image/*';
        input.style.display = 'none';
        input.multiple = true;
        
        input.addEventListener('change', (event) => {
            onFileSelected(event as unknown as React.ChangeEvent<HTMLInputElement>);
        });
        
        document.body.appendChild(input);
        
        input.click();
    }
    
    const emptySlotNumber = () => {
        return (4 - props.listing.imagesUrls.length) > 0 ? (4 - props.listing.imagesUrls.length) : 0;
    }
    
    const maxImagesAchieved = () => {
        return props.listing.imagesUrls.length >= 10;
    }

    function deleteImage(image: ImageDTO) {
        props.setListing((prevState) => {
            return {...prevState, imagesUrls: prevState.imagesUrls.filter((value) => value.url !== image.url)};
        });
    }

    return <div className="photos-creator">
        {imagesEmptyOrUndefined() ? <div className="photos-creator-empty">
            <div className="photos-creator-content">
                <Photos/>
                <div className="listing-detail-title">Drag your photos here</div>
                <div className="creator-wrapper-subtitle">Choose at least 5 photos</div>
                <div className="upload-photos-button" onClick={selectImage}>
                    Upload from your device</div>
            </div>
        </div> : <div className="photos-holders">
            {props.listing.imagesUrls.map((image, index) => {
                return <div key={index} className="photo-holder">
                    <div className="delete-overlay">
                        <div className="svg-round-back" onClick={() => deleteImage(image)}>
                            <Trash/>
                        </div>
                    </div>
                    <img src={image.url} alt="Listing"/>
                </div>
            })}
            {new Array(emptySlotNumber()).fill(0).map((value, index) => {
                return <div key={index} className="photo-holder">
                    <div className="photo-holder-content" onClick={selectImage}>
                        <Photos/>
                    </div>
                </div>
            })}
            {!maxImagesAchieved() ? <div className={"photo-holder"}>
                <div className="photo-holder-content" onClick={selectImage}>
                    <Plus/>
                    Add more
                </div>
            </div> : <></>}
        </div>
        }
    </div>;
};