using ApplicationCommon.Enums;
using ApplicationLogic.Utilities;

namespace ApplicationLogic.Tests.Static;

public class MapperUtilitiesTests
{
    [Test]
    public void ConstructAmenitiesFromStringArray_ValidAmenities_ReturnsAmenities()
    {
        // Arrange
        var amenities = new[] { "Wifi", "Kitchen", "Washer" };

        // Act
        var result = MapperUtilities.ConstructAmenitiesFromStringArray(amenities);

        // Assert
        Assert.AreEqual(Amenities.Wifi | Amenities.Kitchen | Amenities.Washer, result);
    }
    
    [Test]
    public void ConstructAmenitiesFromStringArray_InvalidAmenities_ThrowsArgumentException()
    {
        // Arrange
        var amenities = new[] { "Wifi", "Kitchen", "Washer", "InvalidAmenity" };

        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => MapperUtilities.ConstructAmenitiesFromStringArray(amenities));
    }
    
    [Test]
    public void ConstructStringArrayFromAmenities_ValidAmenities_ReturnsStringArray()
    {
        // Arrange
        var amenities = Amenities.Wifi | Amenities.Kitchen | Amenities.Washer;

        // Act
        var result = MapperUtilities.ConstructStringArrayFromAmenities(amenities);

        // Assert
        var expected = new[] { "Wifi", "Kitchen", "Washer" };

        foreach (var amenity in expected)
        {
            Assert.Contains(amenity, result);
        }
    }
    
    [Test]
    public void ConstructStringArrayFromAmenities_ZeroAmenity_ReturnsEmptyStringArray()
    {
        // Arrange
        var amenities = (Amenities)0;

        // Act
        var result = MapperUtilities.ConstructStringArrayFromAmenities(amenities);

        // Assert
        Assert.IsEmpty(result);
    }
}