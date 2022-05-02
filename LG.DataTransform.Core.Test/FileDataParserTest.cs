using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LG.DataTransform.Core.Test
{
    public class FileDataParserTest
    {
        [Fact]
        public void ParseCsvContentWithHeaders_Success()
        {
           IList<string> sampleData = new List<string>()
            {
                "ObservationDate,Shorthand,From,To,Price",
                "30/12/2009,Q1_10,01/01/2010,31/03/2010,0.3445",
                "30/12/2009,Q2_10,01/04/2010,30/06/2010,0.3302"
            };

            var fileDataParser = new FileDataParser();

            var result = fileDataParser.Parse(sampleData as IReadOnlyList<string>);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void ParseCsvContentWithoutHeaders_Success()
        {
            IList<string> sampleData = new List<string>()
            {
                "30/12/2009,Q1_10,01/01/2010,31/03/2010,0.3445",
                "30/12/2009,Q2_10,01/04/2010,30/06/2010,0.3302"
            };

            var fileDataParser = new FileDataParser();

            var result = fileDataParser.Parse(sampleData as IReadOnlyList<string>);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void ParseWrongCsvContent_Success()
        {
            IList<string> sampleData = new List<string>()
            {
                "30/12/2009,01/01/2010,31/03/2010,0.3445",
                "30/12/2009,Q2_10,01/04/2010,30/06/2010"
            };

            var fileDataParser = new FileDataParser();

            var result = fileDataParser.Parse(sampleData as IReadOnlyList<string>);
            Assert.Equal(2, result.Errors.Count());
        }

        [Fact]
        public void HandleWrongObservationDate_Success()
        {
            IList<string> sampleData = new List<string>()
            {
                "ObservationDate,Shorthand,From,To,Price",
                "Sample,Q1_10,01/01/2010,31/03/2010,0.3445",
                "30/12/2009,Q2_10,01/04/2010,30/06/2010,0.3302"
            };

            var fileDataParser = new FileDataParser();

            var result = fileDataParser.Parse(sampleData as IReadOnlyList<string>);

            Assert.Single( result.Quotes);
            Assert.Single(result.Errors);
        }

        [Fact]
        public void HandleWrongPrice_Success()
        {
            IList<string> sampleData = new List<string>()
            {
                "ObservationDate,Shorthand,From,To,Price",
                "30/12/2009,Q1_10,01/01/2010,31/03/2010,NULL",
                "30/12/2009,Q2_10,01/04/2010,30/06/2010,0.3302"
            };

            var fileDataParser = new FileDataParser();

            var result = fileDataParser.Parse(sampleData as IReadOnlyList<string>);

            Assert.Equal(0, result.Quotes.First().Price);
            Assert.Equal((decimal?) 0.3302, result.Quotes.Last().Price);
        }
    }
}