using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Cell
{
    public string OEM { get; set; }
    public string Model { get; set; }
    public string LaunchAnnounced { get; set; }
    public string LaunchStatus { get; set; }
    public string BodyDimensions { get; set; }
    public string BodyWeight { get; set; }
    public string BodySim { get; set; }
    public string DisplayType { get; set; }
    public string DisplaySize { get; set; }
    public string DisplayResolution { get; set; }
    public string FeaturesSensors { get; set; }
    public string PlatformOS { get; set; }
    public int announcedYear{get; set;}
    public int releasedYear{get;set;}

    // Constructor
    public Cell(string oem, string model, string launchAnnounced, string launchStatus, string bodyDimensions, string bodyWeight, string bodySim, string displayType, string displaySize, string displayResolution, string featuresSensors, string platformOS)
    {
        OEM = oem;
        Model = model;
        LaunchAnnounced = launchAnnounced;
        LaunchStatus = launchStatus;
        BodyDimensions = bodyDimensions;
        BodyWeight = bodyWeight;
        BodySim = bodySim;
        DisplayType = displayType;
        DisplaySize = displaySize;
        DisplayResolution = displayResolution;
        FeaturesSensors = featuresSensors;
        PlatformOS = platformOS;
        announcedYear = announcedYear;
        releasedYear = releasedYear;
    }

    // ToString method to convert object details to a string
    public override string ToString()
    {
       announcedYear = GetYear(LaunchAnnounced);
       releasedYear = GetYear(LaunchStatus);
      
      
      return $"OEM: {OEM}\nModel: {Model}\nLaunch Announced: {LaunchAnnounced} ({announcedYear})\nLaunch Status: {LaunchStatus} ({releasedYear})\nBody Dimensions: {BodyDimensions}\nBody Weight: {BodyWeight}\nBody SIM: {BodySim}\nDisplay Type: {DisplayType}\nDisplay Size: {DisplaySize}\nDisplay Resolution: {DisplayResolution}\nFeatures & Sensors: {FeaturesSensors}\nPlatform OS: {PlatformOS}";

    }
  // Method to extract year from a date string
  public int GetYear(string date)
  {
      int year;
      string[] parts = date.Split(new char[] { ',', '.', ' ', '"' }, StringSplitOptions.RemoveEmptyEntries);
      foreach (string part in parts)
      {
          if (int.TryParse(part, out year))
          {
              return year;
          }
      }
      return -1; // Indicate no valid year found
  }


    // Method to calculate mean of numeric columns
    public double CalculateMean(string column)
    {
        double mean = 0;
        if (double.TryParse(column, out double value))
        {
            mean = value;
        }
        return mean;
    }

    // Method to calculate median of numeric columns
    public double CalculateMedian(string column)
    {
        double median = 0;
        if (double.TryParse(column, out double value))
        {
            median = value;
        }
        return median;
    }

    // Method to calculate standard deviation of numeric columns
    public double CalculateStandardDeviation(string column)
    {
        double standardDeviation = 0;
        if (double.TryParse(column, out double value))
        {
            standardDeviation = value;
        }
        return standardDeviation;
    }

    // Method to find mode of categorical columns
    public string FindMode(List<string> values)
    {
        var mode = values.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;
        return mode;
    }

    // Method to list unique values for each column
    public List<string> ListUniqueValues(string column)
    {
        List<string> uniqueValues = column.Split(',').Select(x => x.Trim()).Distinct().ToList();
        return uniqueValues;
    }

    // Method to add data for each object's variable
    public void AddData(string oem, string model, string launchAnnounced, string launchStatus, string bodyDimensions, string bodyWeight, string bodySim, string displayType, string displaySize, string displayResolution, string featuresSensors, string platformOS)
    {
        OEM = oem;
        Model = model;
        LaunchAnnounced = launchAnnounced;
        LaunchStatus = launchStatus;
        BodyDimensions = bodyDimensions;
        BodyWeight = bodyWeight;
        BodySim = bodySim;
        DisplayType = displayType;
        DisplaySize = displaySize;
        DisplayResolution = displayResolution;
        FeaturesSensors = featuresSensors;
        PlatformOS = platformOS;
    }

    // Method to delete an object
    public void DeleteObject()
    {
        // Reset all properties to default or null values
        OEM = "";
        Model = "";
        LaunchAnnounced = "";
        LaunchStatus = "";
        BodyDimensions = "";
        BodyWeight = "";
        BodySim = "";
        DisplayType = "";
        DisplaySize = "";
        DisplayResolution = "";
        FeaturesSensors = "";
        PlatformOS = "";
    }
  
}

class Program
{
  static int yearMix;
  int[] arra;

  static void Main()
  {
      // File path
      string filePath = "cells.csv";

      // Check if file exists
      if (File.Exists(filePath))
      {
          // Read all lines from the file
          string[] lines = File.ReadAllLines(filePath);

          // Create dictionary to store Cell objects
          Dictionary<int, Cell> cellMap = new Dictionary<int, Cell>();

          // Count for phones with multiple features_sensors
          int multipleFeaturesCount = 0;
          int announcedSwitchedCount = 0;

          // Variables to store highest average weight of phone body
          Dictionary<string, (double sum, int count)> weightSumByOEM = new Dictionary<string, (double, int)>();

          // Variables to track phones announced in one year and released in another
          List<string> announcedReleasedMismatch = new List<string>();

          // Variables to track phones with only one feature sensor
          int phonesWithOneFeatureSensor = 0;

          // Variables to track the year with the most phones launched
          Dictionary<int, int> phonesLaunchedByYear = new Dictionary<int, int>();

          // Loop counter
          int lineNumber = 0;

          // Iterate through each line
          foreach (string line in lines)
          {
              // Increment line number
              lineNumber++;

              // Split the line into columns
              string[] columns = SplitCSVLine(line);

              // Create a new Cell object
              Cell cell = new Cell(columns[0], columns[1], columns[2], columns[3], columns[4], columns[5], columns[6], columns[7], columns[8], columns[9], columns[10], columns[11]);

              // Calculate average weight of the phone body for each OEM
              if (double.TryParse(columns[5], out double weight))
              {
                  if (!weightSumByOEM.ContainsKey(columns[0]))
                  {
                      weightSumByOEM[columns[0]] = (weight, 1);
                  }
                  else
                  {
                      var (sum, count) = weightSumByOEM[columns[0]];
                      weightSumByOEM[columns[0]] = (sum + weight, count + 1);
                  }
              }

              // Check if the phone was announced in one year but made available in another
              string[] announcedParts = columns[2].Split(',', '.'); // Splitting the announced date
              string[] releasedParts = columns[3].Split(',', '.'); // Splitting the released date
            
              
              

              // Check if features_sensors contains multiple entries
              if (columns[10].Contains(","))
              {
                  multipleFeaturesCount++;
              }

              // Count phones with only one feature sensor
              if (!string.IsNullOrWhiteSpace(columns[10]) && !columns[10].Contains(","))
              {
                  phonesWithOneFeatureSensor++;
              }

              // Count phones launched by year
              if (DateTime.TryParse(columns[2], out DateTime launchDate))
              {
                  int year = launchDate.Year;
                  if (year > 1999)
                  {
                      if (phonesLaunchedByYear.ContainsKey(year))
                      {
                          phonesLaunchedByYear[year]++;
                      }
                      else
                      {
                          phonesLaunchedByYear[year] = 1;
                      }
                  }
              }

              // Add the object to the dictionary
              cellMap.Add(lineNumber, cell);
          }
        // Arrays to store announcedYear and releasedYear
        int[] announcedYears = new int[cellMap.Count];
        int[] releasedYears = new int[cellMap.Count];
        int announceReleaseDissimilarity=0;

        // Loop counter
        int i = 0;

        // Iterate through each cell in the dictionary
        foreach (KeyValuePair<int, Cell> kvp in cellMap)
        {
            // Extract announcedYear and releasedYear
            if( kvp.Value.GetYear(kvp.Value.LaunchAnnounced) >1999){
            announcedYears[i] = kvp.Value.GetYear(kvp.Value.LaunchAnnounced);
            }
            releasedYears[i] = kvp.Value.GetYear(kvp.Value.LaunchStatus);
            if(releasedYears[i] !=-1 && announcedYears[i] !=-1 && releasedYears[i] != announcedYears[i])
              {
                announceReleaseDissimilarity +=1;
                announcedReleasedMismatch.Add(kvp.Key.ToString());
              }
            Console.WriteLine($"Index: {kvp.Key}");
            Console.WriteLine(kvp.Value.ToString()); // Using ToString() method to print object details
            Console.WriteLine();

            i++; // Increment counter
        }
       
        int mostCommonValue = 0;
        int highestCount = 0;
        foreach (KeyValuePair<int, int> pair in cnt) {
           if (pair.Value > highestCount) {
              mostCommonValue = pair.Key;
              highestCount = pair.Value;
           }
        }
          
          // Print out the count of phones with multiple features_sensors
          Console.WriteLine($"Number of phones with multiple features/sensors: {multipleFeaturesCount}");

          // Print out the count of phones announced in one year but released in another
          Console.WriteLine($"Number of phones announced in one year but released in another: {announcedSwitchedCount}");
          if (announcedSwitchedCount > 0)
          {
              Console.WriteLine("Phones announced in one year but released in another:");
              foreach (var phone in announcedReleasedMismatch)
              {
                  Console.WriteLine(phone);
                  Console.WriteLine(announceReleaseDissimilarity);

              }
          }

          // Print out the average weight of the phone body for each OEM
          if (weightSumByOEM.Any())
          {
              var highestAverageWeightOEM = weightSumByOEM.OrderByDescending(kv => kv.Value.sum / kv.Value.count).First().Key;
              Console.WriteLine($"OEM with the highest average weight of the phone body: {highestAverageWeightOEM}");
          }

          // Print out the count of phones with only one feature sensor
          Console.WriteLine($"Number of phones with only one feature / sensor: {phonesWithOneFeatureSensor}");

          Console.WriteLine($"The year with the highest phone production after 1999 was {mostCommonValue} with {highestCount} number of phones");

        // Call the methods to perform the checks
        CheckHighestAverageWeight(cellMap);
      }
      else
      {
          Console.WriteLine("File does not exist.");
      }
  }


    // Function to split CSV line while handling quoted fields
    static string[] SplitCSVLine(string line)
    {
        List<string> columns = new List<string>();
        bool inQuotes = false;
        int start = 0;

        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] == '"')
            {
                inQuotes = !inQuotes;
            }

            if (line[i] == ',' && !inQuotes)
            {
                columns.Add(line.Substring(start, i - start));
                start = i + 1;
            }
        }

        columns.Add(line.Substring(start)); // Add the last column

        return columns.ToArray();
    }

    // Function to check the company (oem) with the highest average weight of the phone body
    static void CheckHighestAverageWeight(Dictionary<int, Cell> cellMap)
    {
        var oemAverageWeights = cellMap.Values.GroupBy(cell => cell.OEM)
                                              .ToDictionary(group => group.Key,
                                                            group => group.Average(cell => double.TryParse(cell.BodyWeight, out double weight) ? weight : 0));

        var highestAverageWeightOEM = oemAverageWeights.OrderByDescending(x => x.Value).FirstOrDefault();

        Console.WriteLine($"Company with the highest average weight of the phone body: {highestAverageWeightOEM.Value}");
    }

 
   
}
