using LG.DataTransform.Models;
using static System.Decimal;

namespace LG.DataTransform.Core;

/// <summary>
/// Parse the CSV file data.
/// </summary>
public class FileDataParser
{
    #region FileDataParserResult

    public class FileDataParserResult : BaseResult
    {

        private readonly IList<Quote> _quotes = new List<Quote>();

        public IEnumerable<Quote> Quotes => _quotes;

        public void AppendQuote(Quote quote)
        {
            if (quote == null) throw new ArgumentNullException(nameof(quote));
            _quotes.Add(quote);
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Parse the provided CSV file string data.
    /// </summary>
    /// <param name="fileData"></param>
    /// <returns></returns>
    public FileDataParserResult Parse(IReadOnlyList<string>? fileData)
    {
        FileDataParserResult fileDataParserResult = new FileDataParserResult();
        try
        {
            if (fileData == null || !fileData.Any())
            {
                fileDataParserResult.AppendError($"File data content is not provided.");
                return fileDataParserResult;
            }

            //Loop through the each row.
            for (int lineIndex = 0; lineIndex < fileData.Count(); lineIndex++)
            {
            
                var lineData = fileData[lineIndex].Split(Constants.Separator);

                //skip if the first row is header
                if (lineIndex == 0 && lineData[0].Equals(nameof(Quote.ObservationDate), StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }


                //Validate each line of data. Log an error if not valid.
                if (lineData.Length != 5)
                {
                    fileDataParserResult.AppendError($"Skip the Line:{lineIndex}, from the given file as the data is not in correct format.");
                    continue;
                }

                if (!DateTime.TryParse(lineData[0], out var observationDate))
                {
                    fileDataParserResult.AppendError($"Skip the Line:{lineIndex}, from the given file as the ObservationDate is not parsed.");
                    continue;
                }

                if (!DateTime.TryParse(lineData[2], out var fromDate))
                {
                    fileDataParserResult.AppendError($"Skip the Line:{lineIndex}, from the given file as the FromDate is not parsed.");
                    continue;
                }

                if (!DateTime.TryParse(lineData[3], out var toDate))
                {
                    fileDataParserResult.AppendError($"Skip the Line:{lineIndex}, from the given file as the ToDate is not parsed.");
                    continue;
                }

                TryParse(lineData[4], out var price);

                Quote quote = new(observationDate, lineData[1], price , fromDate, toDate);
                fileDataParserResult.AppendQuote(quote);

            }

        }
        catch (Exception e)
        {
            fileDataParserResult.AppendError(e.Message);
        }

        return fileDataParserResult;
    }

    #endregion
}