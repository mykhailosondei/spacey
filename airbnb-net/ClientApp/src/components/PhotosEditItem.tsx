import {ImageDTO} from "../DTOs/Image/ImageDTO";
import Trash from "./Icons/Trash";
import React from "react";
import Plus from "./Icons/Plus";
import {ImageUploadService} from "../services/ImageUploadService";

interface PhotosEditItemProps {
    value: ImageDTO[],
    onSubmit: (value: ImageDTO[]) => void
}

export const PhotosEditItem = (props: PhotosEditItemProps) => {
    
    const [images, setImages] = React.useState<ImageDTO[]>(props.value);
    
    const imageUploadService = React.useMemo(() => { return ImageUploadService.getInstance()}, []);
    
    function deleteImage(image : ImageDTO) {
        setImages(images.filter((value) => value.url !== image.url));
    }

    function onFileSelected(event: React.ChangeEvent<HTMLInputElement>) {
        if(event.target.files){
            imageUploadService.uploadMany(Array.from(event.target.files)).then((result) => {
                const results = [] as string[];
                for (let i = 0; i < result.length; i++) {
                    if(result[i].status === 200){
                        results.push(result[i].data);
                    }
                }
                
                const newImagesArray : ImageDTO[]  = results.map((value) => {return {url: value}}).slice(0, 10);
                
                setImages(images.concat(newImagesArray).slice(0, 10));
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
        document.body.removeChild(input);
    }
    
    const arraysEqual = (a: ImageDTO[], b: ImageDTO[]) : boolean => {
        if (a === b) return true;
        if (a == null || b == null) return false;
        if (a.length !== b.length) return false;
    
        for (let i = 0; i < a.length; ++i) {
            if (a[i].url !== b[i].url) return false;
        }
        return true;
    }

    function submitPhotos() {
        if(images.length < 5 || images.length > 10) return;
        props.onSubmit(images);
    }
    
    function updateDisabled() {
        return arraysEqual(images, props.value) || images.length < 5 || images.length > 10;
    }

    return <div>
        <div className={"photos-edit-holder"}>
            {images.map((image, index) => {
                return <div key={index} className="edit-photo-holder">
                    <div className="delete-overlay">
                        <div className="svg-round-back" onClick={() => deleteImage(image)}>
                            <Trash/>
                        </div>
                    </div>
                    <img src={image.url} alt="Listing"/>
                </div>
            })}
            <div className={"edit-photo-holder"}>
                <div className="photo-holder-content" onClick={selectImage}>
                    <Plus/>
                    Add more
                </div>
            </div>
        </div>
        <button className="white-on-black-btn margin-t10" disabled={updateDisabled()}
                onClick={submitPhotos}>Update
        </button>
    </div>
};