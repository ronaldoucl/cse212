// Represents the root of the JSON data returned by the USGS API
public class FeatureCollection
{
    public List<Feature> features { get; set; } // List of earthquake records
}

// Represents a single earthquake feature in the JSON data
public class Feature
{
    public Properties properties { get; set; } // Contains magnitude and location info
}

// Represents the properties of an earthquake (magnitude and place)
public class Properties
{
    public double? mag { get; set; } // Magnitude of the earthquake (this can be null)
    public string place { get; set; } // Location description of the earthquake
}