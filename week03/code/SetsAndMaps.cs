using System.Text.Json;

public static class SetsAndMaps
{
    /// <summary>
    /// The words parameter contains a list of two character 
    /// words (lower case, no duplicates). Using sets, find an O(n) 
    /// solution for returning all symmetric pairs of words.  
    ///
    /// For example, if words was: [am, at, ma, if, fi], we would return :
    ///
    /// ["am & ma", "if & fi"]
    ///
    /// The order of the array does not matter, nor does the order of the specific words in each string in the array.
    /// at would not be returned because ta is not in the list of words.
    ///
    /// As a special case, if the letters are the same (example: 'aa') then
    /// it would not match anything else (remember the assumption above
    /// that there were no duplicates) and therefore should not be returned.
    /// </summary>
    /// <param name="words">An array of 2-character words (lowercase, no duplicates)</param>
    public static string[] FindPairs(string[] words)
    {
        // TODO Problem 1 - ADD YOUR CODE HERE
        var set = new HashSet<string>(words); // Store all words in a set for fast lookups
        var result = new List<string>(); // List to store the resulting pairs

        // Iterate over each word
        foreach (var word in words)
        {
            if (word[0] == word[1]) continue; // Skip words like "aa" (same letters)

            var reversed = new string(new[] { word[1], word[0] }); // Create the reversed version of the current word

            // Check if the reversed word exists in the set
            if (set.Contains(reversed))
            {
                // Add the symmetric pair to the result list
                result.Add($"{word} & {reversed}");

                // Remove both words from the set to avoid duplicate pairs
                set.Remove(word);
                set.Remove(reversed);
            }
        }

        return result.ToArray(); // Convert the result list to an array and return it
    }

    /// <summary>
    /// Read a census file and summarize the degrees (education)
    /// earned by those contained in the file.  The summary
    /// should be stored in a dictionary where the key is the
    /// degree earned and the value is the number of people that 
    /// have earned that degree.  The degree information is in
    /// the 4th column of the file.  There is no header row in the
    /// file.
    /// </summary>
    /// <param name="filename">The name of the file to read</param>
    /// <returns>fixed array of divisors</returns>
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>();
        foreach (var line in File.ReadLines(filename))
        {
            var fields = line.Split(",");
            // Ensure the line has at least 4 columns
            if (fields.Length > 3)
            {
                var degree = fields[3].Trim(); // Extract degree from column 4 (index 3)

                if (degrees.ContainsKey(degree))
                {
                    degrees[degree]++; // Increment the count if the degree already exists
                }
                else
                {
                    degrees[degree] = 1; // Add new degree with initial count 1
                }
            }
        }

        return degrees; // Return the dictionary with all degree counts
    }

    /// <summary>
    /// Determine if 'word1' and 'word2' are anagrams.  An anagram
    /// is when the same letters in a word are re-organized into a 
    /// new word.  A dictionary is used to solve the problem.
    /// 
    /// Examples:
    /// is_anagram("CAT","ACT") would return true
    /// is_anagram("DOG","GOOD") would return false because GOOD has 2 O's
    /// 
    /// Important Note: When determining if two words are anagrams, you
    /// should ignore any spaces.  You should also ignore cases.  For 
    /// example, 'Ab' and 'Ba' should be considered anagrams
    /// 
    /// Reminder: You can access a letter by index in a string by 
    /// using the [] notation.
    /// </summary>
    public static bool IsAnagram(string word1, string word2)
    {
        // TODO Problem 3 - ADD YOUR CODE HERE
        var cleanA = word1.Replace(" ", "").ToLower(); // Remove spaces and convert to lowercase
        var cleanB = word2.Replace(" ", "").ToLower(); // Do the same for the second word

        // Quick check: if lengths differ, they can't be anagrams
        if (cleanA.Length != cleanB.Length)
        {
            return false;
        }

        var letterCounts = new Dictionary<char, int>();

        // Count each character in the first word
        foreach (char c in cleanA)
        {
            if (letterCounts.ContainsKey(c))
                letterCounts[c]++; // Increment count if character exists
            else
                letterCounts[c] = 1; // Initialize count to 1
        }

        // Subtract character counts using the second word
        foreach (char c in cleanB)
        {
            if (!letterCounts.ContainsKey(c))
                return false;

            letterCounts[c]--; // Decrease count

            if (letterCounts[c] < 0)
                return false;
        }

        // Ensure all character counts are zero
        return letterCounts.Values.All(count => count == 0);
    }

    /// <summary>
    /// This function will read JSON (Javascript Object Notation) data from the 
    /// United States Geological Service (USGS) consisting of earthquake data.
    /// The data will include all earthquakes in the current day.
    /// 
    /// JSON data is organized into a dictionary. After reading the data using
    /// the built-in HTTP client library, this function will return a list of all
    /// earthquake locations ('place' attribute) and magnitudes ('mag' attribute).
    /// Additional information about the format of the JSON data can be found 
    /// at this website:  
    /// 
    /// https://earthquake.usgs.gov/earthquakes/feed/v1.0/geojson.php
    /// 
    /// </summary>
    public static string[] EarthquakeDailySummary()
    {
        const string uri = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";
        using var client = new HttpClient();
        using var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        using var jsonStream = client.Send(getRequestMessage).Content.ReadAsStream();
        using var reader = new StreamReader(jsonStream);
        var json = reader.ReadToEnd();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);

        // TODO Problem 5:
        // 1. Add code in FeatureCollection.cs to describe the JSON using classes and properties 
        // on those classes so that the call to Deserialize above works properly.
        // 2. Add code below to create a string out each place a earthquake has happened today and its magitude.
        // 3. Return an array of these string descriptions.
        var result = new List<string>(); // Prepare list to hold formatted earthquake strings

        if (featureCollection?.features != null) // Ensure the data was deserialized correctly
        {
            // Iterate over each earthquake feature
            foreach (var feature in featureCollection.features)
            {
                var place = feature.properties?.place; // Get location description
                var mag = feature.properties?.mag; // Get magnitude

                if (!string.IsNullOrEmpty(place) && mag.HasValue) // Only include valid data
                {
                    result.Add($"{place} - Mag {mag.Value}"); // Format and add to result
                }
            }
        }

        return result.ToArray(); // Convert list to array and return
    }
}