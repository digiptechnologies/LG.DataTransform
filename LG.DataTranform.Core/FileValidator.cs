using LG.DataTransform.Models;

namespace LG.DataTransform.Core;

#nullable enable

/// <summary>
/// Perform multiple validation.
/// </summary>
public class FileValidator
{
    #region FileValidationResult

    /// <summary>
    /// Represent the file validation result.
    /// </summary>
    public class FileValidationResult : BaseResult
    {
       
    }

    #endregion
    

    #region Methods

    public FileValidationResult IsFileValidCsv(string? filePath)
    {
        var fileValidationResult = new FileValidationResult();

        try
        {
            //Valid file path.
            if (!File.Exists(filePath))
            {
                fileValidationResult.AppendError($"Given file path: {filePath} , is not exist");
                return fileValidationResult;
            }

            //Valid file ext.
            var fileExt = Path.GetExtension(filePath);
            if (string.IsNullOrEmpty(fileExt) || !fileExt.Equals(".csv", StringComparison.InvariantCultureIgnoreCase))
            {
                fileValidationResult.AppendError($"Only .csv file extension is allow.");
                return fileValidationResult;
            }

            //Valid format. (for now validating number of columns).
            var firstRow = File.ReadLines(filePath).FirstOrDefault();
            if (firstRow == null || firstRow.Split(Constants.Separator).Length != 5)
            {
                fileValidationResult.AppendError($"Please select another file, as the current file does not have the intended file format.");
                return fileValidationResult;
            }

        }
        catch (Exception e)
        {
            fileValidationResult.AppendError(e.Message);
        }

        return fileValidationResult;
    }

    #endregion



}
