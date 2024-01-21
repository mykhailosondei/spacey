import { ReactComponent as WifiIcon } from "./svgs/Amenities/Wifi.svg";
import { ReactComponent as TVIcon } from "./svgs/Amenities/TV.svg";
import { ReactComponent as KitchenIcon } from "./svgs/Amenities/Kitchen.svg";
import { ReactComponent as AirConditioningIcon } from "./svgs/Amenities/AirConditioning.svg";
import { ReactComponent as HeatingIcon } from "./svgs/Amenities/Heating.svg";
import { ReactComponent as WasherIcon } from "./svgs/Amenities/Washer.svg";
import { ReactComponent as DryerIcon } from "./svgs/Amenities/Dryer.svg";
import { ReactComponent as FreeParkingIcon } from "./svgs/Amenities/FreeParking.svg";
import { ReactComponent as PoolIcon } from "./svgs/Amenities/Pool.svg";
import { ReactComponent as GymIcon } from "./svgs/Amenities/Gym.svg";
import { ReactComponent as HotTubIcon } from "./svgs/Amenities/HotTub.svg";
import { ReactComponent as PetFriendlyIcon } from "./svgs/Amenities/PetFriendly.svg";
import { ReactComponent as SmokingAllowedIcon } from "./svgs/Amenities/SmokingAllowed.svg";
import { ReactComponent as FamilyFriendlyIcon } from "./svgs/Amenities/FamilyFriendly.svg";
import { ReactComponent as WheelchairAccessibleIcon } from "./svgs/Amenities/WheelchairAccessible.svg";
import {FunctionComponent, SVGProps} from "react";

interface AmenityIconsDictionary {
    [key: string]: FunctionComponent<SVGProps<SVGSVGElement> & {     title?: string | undefined; }>;
}
export const amenityIconsDictionary:AmenityIconsDictionary = {
  Wifi: WifiIcon,
  TV: TVIcon,
  Kitchen: KitchenIcon,
  AirConditioning: AirConditioningIcon,
  Heating: HeatingIcon,
  Washer: WasherIcon,
  Dryer: DryerIcon,
  FreeParking: FreeParkingIcon,
  Pool: PoolIcon,
  Gym: GymIcon,
  HotTub: HotTubIcon,
  PetFriendly: PetFriendlyIcon,
  SmokingAllowed: SmokingAllowedIcon,
  FamilyFriendly: FamilyFriendlyIcon,
  WheelchairAccessible: WheelchairAccessibleIcon,
};