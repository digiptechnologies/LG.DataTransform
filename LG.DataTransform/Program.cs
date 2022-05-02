using System.Collections.ObjectModel;
using System.Diagnostics;
using LG.DataTransform.Core;
using LG.DataTransform.Models;

namespace LG.DataTransform;

public class Program
{
    public static void Main(string[] args)
    {

        while (true)
        {
            Console.WriteLine(string.Empty);
            Console.WriteLine(string.Empty);
            Console.WriteLine("Please enter the CSV file path which you want to Transform -");
            var filePath = Console.ReadLine();
            
            ProcessInput(filePath);
        }
        
    }

    private static void ProcessInput(string? filePath)
    {
        //File Validation.
        FileValidator fileValidator = new FileValidator();
        var fileValidationResult = fileValidator.IsFileValidCsv(filePath);

        if (fileValidationResult.HasError)
        {
            PrintErrorsOnConsole(fileValidationResult);
            return;
        }

        // File data parsing
        Debug.Assert(filePath != null, nameof(filePath) + " != null");
        var fileLines = File.ReadLines(filePath).ToArray();

        FileDataParser fileDataParser = new();
        var fileDataParserResult = fileDataParser.Parse(new ReadOnlyCollection<string>(fileLines));

        if (fileDataParserResult.HasError && fileDataParserResult.Quotes.Any())
        {
            PrintWarningOnConsole(fileDataParserResult);
        }

        //File Data transform
        FileDataTransformer fileDataTransformer = new();
        var fileDataTransformerResult = fileDataTransformer.Transform(fileDataParserResult.Quotes.ToList());
        if (fileDataTransformerResult.HasError)
        {
            PrintErrorsOnConsole(fileDataTransformerResult);
            return;
        }

        //Write the data to CSV file.
        var destinationFilePath = Path.Combine(Environment.CurrentDirectory, "Results",
            $"{Path.GetFileNameWithoutExtension(filePath)}_{DateTime.Now:yyyyMMddhhmmss}.csv");

        if (!Directory.Exists(Path.GetDirectoryName(destinationFilePath)))
            Directory.CreateDirectory(Path.GetDirectoryName(destinationFilePath) ?? string.Empty);

        File.WriteAllText(destinationFilePath, fileDataTransformerResult.TransformedText);

        Console.WriteLine($"Process completed Successfully. New csv file has been generated at :{destinationFilePath}");

        Process.Start(new ProcessStartInfo(destinationFilePath){ UseShellExecute = true});
    }

    private static void PrintErrorsOnConsole(BaseResult result)
    {
        if (result.HasError)
        {
            Console.WriteLine("ERROR: File data transformation failed. Please note below error(s):");
            int errorIndex = 1;
            foreach (var resultError in result.Errors)
            {
                Console.WriteLine($"   {errorIndex}: {resultError}");
                errorIndex++;
            }
        }
    }
    private static void PrintWarningOnConsole(BaseResult result)
    {
        if (result.HasError)
        {
            Console.WriteLine("WARNING : File data parsing completed with the below warning(s):");
            int errorIndex = 1;
            foreach (var resultError in result.Errors)
            {
                Console.WriteLine($"   {errorIndex}: {resultError}");
                errorIndex++;
            }
        }
    }
}