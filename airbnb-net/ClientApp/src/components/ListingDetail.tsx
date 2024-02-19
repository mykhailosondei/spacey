import {ReactNode} from "react";

interface ListingDetailProps {
    children?: ReactNode;
    name: string;
    className?: string;
}

export const ListingDetail = (props: ListingDetailProps) => {
    return <div className={`listing-detail ${props}`}>
        <div className="listing-detail-header">
            <div className="listing-detail-title">{props.name}</div>
        </div>
        <div className="listing-detail-content">
            {props.children}
        </div>
    </div>;
};