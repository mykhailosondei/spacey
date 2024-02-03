import NavBar from "../NavBar";
import {PropertyTypeSlider} from "../PropertyTypeSlider";
import GeneralListingHolder from "../GeneralListingHolder";
import {SwitchToUser} from "./SwitchToUser";

export const MainPage = () => {
    
    return <>
        <SwitchToUser>
            <NavBar/>
            <PropertyTypeSlider/>
            <GeneralListingHolder/>
        </SwitchToUser>
    </>
}