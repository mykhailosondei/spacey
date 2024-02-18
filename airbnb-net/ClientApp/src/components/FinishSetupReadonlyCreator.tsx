import {ListingCreateDTO} from "../DTOs/Listing/ListingCreateDTO";

export const FinishSetupReadonlyCreator = (props: { listing: ListingCreateDTO }) => {
    return <div className="readonly-creator">
        <div className={"rc-titles"}>
            <div className="readonly-creator-title">
                Finish setting up <br/> your listing
            </div>
            <div className="readonly-creator-description">
                Finally, you'll set up pricing and publish your listing.
            </div>
        </div>
        <div className="rc-content">
            <div className="readonly-creator-image">
                <img src={"https://mir-s3-cdn-cf.behance.net/project_modules/max_1200/3d279b182156915.65284e04274b2.png"} alt=""/>
            </div>
        </div>
    </div>;
};