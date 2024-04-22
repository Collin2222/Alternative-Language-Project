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
    }

    // ToString method to convert object details to a string
    public override string ToString()
    {
        return $"OEM: {OEM}\nModel: {Model}\nLaunch Announced: {LaunchAnnounced}\nLaunch Status: {LaunchStatus}\nBody Dimensions: {BodyDimensions}\nBody Weight: {BodyWeight}\nBody SIM: {BodySim}\nDisplay Type: {DisplayType}\nDisplay Size: {DisplaySize}\nDisplay Resolution: {DisplayResolution}\nFeatures & Sensors: {FeaturesSensors}\nPlatform OS: {PlatformOS}";
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

                // Check if features_sensors contains multiple entries
                if (columns[10].Contains(","))
                {
                    multipleFeaturesCount++;
                }

                // Add the object to the dictionary
                cellMap.Add(lineNumber, cell);
            }

            // Print out the contents of the dictionary
            foreach (KeyValuePair<int, Cell> kvp in cellMap)
            {
                Console.WriteLine($"Index: {kvp.Key}");
                Console.WriteLine(kvp.Value.ToString()); // Using ToString() method to print object details
                Console.WriteLine();
            }

            // Print out the count of phones with multiple features_sensors
            Console.WriteLine($"Phones with multiple features_sensors: {multipleFeaturesCount}");
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
}
