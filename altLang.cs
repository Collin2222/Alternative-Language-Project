using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;

class CellPhoneStatistics
{
    // Dictionary to store Cell objects with row index as the key
    private Dictionary<int, Cell> cellDictionary = new Dictionary<int, Cell>();

    // Main method
    static void Main(string[] args)
    {
        // Create an instance of CellPhoneStatistics
        CellPhoneStatistics statistics = new CellPhoneStatistics();

        // Read the CSV file and store data
        statistics.ReadCSVFile("cells.csv");

        // Perform operations such as calculating statistics
        statistics.CalculateStatistics();

        // Additional operations can be added here

        // For example, listing unique values for each column
        statistics.ListUniqueValues();
    }

    // Method to read the CSV file and store data
    public void ReadCSVFile(string filePath)
    {
        try
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Read records and iterate through each row
                int index = 0;
                while (csv.Read())
                {
                    // Read each column value
                    string oem = csv.GetField<string>("oem");
                    string model = csv.GetField<string>("model");
                    string launchAnnounced = csv.GetField<string>("launch_announced");
                    string launchStatus = csv.GetField<string>("launch_status");
                    string bodyDimensions = csv.GetField<string>("body_dimensions");
                    string bodyWeight = csv.GetField<string>("body_weight");
                    string bodySim = csv.GetField<string>("body_sim");
                    string displayType = csv.GetField<string>("display_type");
                    string displaySize = csv.GetField<string>("display_size");
                    string displayResolution = csv.GetField<string>("display_resolution");
                    string featuresSensors = csv.GetField<string>("features_sensors");
                    string platformOS = csv.GetField<string>("platform_os");

                    // Create a Cell object using the row data
                    Cell cell = new Cell(
                        oem,
                        model,
                        launchAnnounced,
                        launchStatus,
                        bodyDimensions,
                        bodyWeight,
                        bodySim,
                        displayType,
                        displaySize,
                        displayResolution,
                        featuresSensors,
                        platformOS
                    );

                    // Store the Cell object in the dictionary with the row index as the key
                    cellDictionary[index] = cell;

                    // Increment index for next row
                    index++;
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions such as file not found, CSV parsing errors, etc.
            Console.WriteLine($"An error occurred while reading the CSV file: {ex.Message}");
        }
    }

    // Method to calculate statistics on columns (e.g., mean, median, standard deviation)
    public void CalculateStatistics()
    {
        // Example: Calculate mean, median, and standard deviation for body weight
        List<float?> bodyWeights = new List<float?>();
        foreach (var cell in cellDictionary.Values)
        {
            if (cell.BodyWeight.HasValue)
            {
                bodyWeights.Add(cell.BodyWeight.Value);
            }
        }

        // Calculate statistics such as mean, median, and standard deviation
        // Code to calculate these statistics goes here
        // For example, calculate mean
        float mean = CalculateMean(bodyWeights);

        // Print the calculated statistics
        Console.WriteLine($"Mean body weight: {mean} g");

        // Similarly, calculate other statistics as needed
    }

    // Helper method to calculate mean
    private float CalculateMean(List<float?> values)
    {
        if (values == null || values.Count == 0)
        {
            return 0;
        }

        float sum = 0;
        int count = 0;
        foreach (var value in values)
        {
            if (value.HasValue)
            {
                sum += value.Value;
                count++;
            }
        }

        return sum / count;
    }

    // Method to list unique values for each column
    public void ListUniqueValues()
    {
        // Example: List unique values for platform OS
        HashSet<string> uniquePlatformOS = new HashSet<string>();
        foreach (var cell in cellDictionary.Values)
        {
            if (!string.IsNullOrEmpty(cell.PlatformOS))
            {
                uniquePlatformOS.Add(cell.PlatformOS);
            }
        }

        // Print the unique values
        Console.WriteLine("Unique platform OS:");
        foreach (var os in uniquePlatformOS)
        {
            Console.WriteLine(os);
        }

        // Similarly, list unique values for other columns as needed
    }

    // Add more methods for operations such as adding and deleting objects as needed
}
