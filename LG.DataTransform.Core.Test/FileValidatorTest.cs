using Xunit;

namespace LG.DataTransform.Core.Test;

public class FileValidatorTest
{
    [Fact]
    public void IsFileExist_Success()
    {
        var expectedResult = new FileValidator.FileValidationResult();
        FileValidator fileValidator = new FileValidator();

        var result = fileValidator.IsFileValidCsv(@"SampleData\Test.csv");

        Assert.Equal(expectedResult.HasError, result.HasError);
    }

    [Fact]
    public void IsFileNotExist_Success()
    {
        var expectedResult = new FileValidator.FileValidationResult();
        expectedResult.AppendError("File Not found");

        FileValidator fileValidator = new FileValidator();

        var result = fileValidator.IsFileValidCsv(@"SampleData\dummyfile.csv");

        Assert.Equal(expectedResult.HasError, result.HasError);
    }

    [Fact]
    public void FileHasCsvExtension_Success()
    {
        var expectedResult = new FileValidator.FileValidationResult();
       
        FileValidator fileValidator = new FileValidator();

        var result = fileValidator.IsFileValidCsv(@"SampleData\Test.csv");

        Assert.Equal(expectedResult.HasError, result.HasError);
    }
     
}