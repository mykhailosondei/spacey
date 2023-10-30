namespace ApplicationCommon.Enums;

[Flags]
public enum Amenities
{
    None = 0,
    Wifi = 1 << 0,
    TV = 1 << 1,
    Kitchen = 1 << 2,
    AirConditioning = 1 << 3,
    Heating = 1 << 4,
    Washer = 1 << 5,
    Dryer = 1 << 6,
    FreeParking = 1 << 7,
    Pool = 1 << 8,
    Gym = 1 << 9,
    HotTub = 1 << 10,
    PetFriendly = 1 << 11,
    SmokingAllowed = 1 << 12,
    FamilyFriendly = 1 << 13,
    WheelchairAccessible = 1 << 14
}