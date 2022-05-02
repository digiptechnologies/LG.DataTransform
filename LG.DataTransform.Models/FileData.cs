namespace LG.DataTransform.Models;

public class FileData
{
    #region Properties

    public FileData(string filePath, IEnumerable<Quote> quote)
    {
        FilePath = filePath;
        Quote = quote;
    }

    public string FilePath { get;  }
   
    public IEnumerable<Quote> Quote { get; }

    #endregion
}