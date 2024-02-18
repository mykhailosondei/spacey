import {ListingCreateDTO} from "../DTOs/Listing/ListingCreateDTO";

export function StandOutReadonlyCreator(props: { listing: ListingCreateDTO }) {
    return <div className="readonly-creator">
        <div className={"rc-titles"}>
            <div className="readonly-creator-title">
                Make your place <br/> stand out
            </div>
            <div className="readonly-creator-description">
                Add a standout feature to your listing
            </div>
        </div>
        <div className="rc-content">
            <div className="readonly-creator-image">
                <img src={"https://mir-s3-cdn-cf.behance.net/project_modules/max_1200/d72ef7120661385.60b63d7266b26.png"} alt=""/>
            </div>
        </div>
    </div>;
}