import {ListingCreateDTO} from "../DTOs/Listing/ListingCreateDTO";

export const StartReadonlyCreator = (props: { listing: ListingCreateDTO }) => {
    return <div className="readonly-creator">
        <div className={"rc-titles"}>
            <div className="readonly-creator-title">
                Tell us about <br/> your place
            </div>
            <div className="readonly-creator-description">
                Let's get started creating your listing by telling us a bit about your place.
            </div>
        </div>
        <div className="rc-content">
            <div className="readonly-creator-image">
                <img src={"https://mir-s3-cdn-cf.behance.net/project_modules/1400_opt_1/185a64187743273.658ef5445b463.jpg"} alt=""/>
            </div>
        </div>
    </div>;
};