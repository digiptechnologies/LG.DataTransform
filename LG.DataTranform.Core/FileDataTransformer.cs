using System.Data;
using System.Diagnostics;
using LG.DataTransform.Models;

namespace LG.DataTransform.Core;

/// <summary>
/// Transform the Quotes data.
/// </summary>
public class FileDataTransformer
{
    #region FileDataTransformerResult

    public class FileDataTransformerResult :BaseResult
    {
        public string TransformedText { get; internal set; }
    }

    #endregion

    /// <summary>
    /// Generate transformed CSV text content based on the provided <see cref="quotes"/>
    /// </summary>
    /// <param name="quotes"></param>
    /// <returns></returns>
    public FileDataTransformerResult Transform(IReadOnlyList<Quote>? quotes)
    {
        FileDataTransformerResult result = new FileDataTransformerResult();

        if (quotes == null)
        {
            result.AppendError($"No quotes available to process.");
            return result;
        }
        try
        {
            //sort the quotes.
            var sortedQuotes = quotes.OrderBy(q => q.ObservationDate).ThenBy(f => f.From).ToList();

            Debug.Assert(sortedQuotes != null, nameof(sortedQuotes) + " != null");
            
            //generating list of column which has key-pair value having From date as DateTime and Shorthand as string.
            IEnumerable<KeyValuePair<DateTime, string>> columnDictionary =
                sortedQuotes.GroupBy(f => f.From).OrderBy(s => s.Key).Select(s => new KeyValuePair<DateTime, string>((DateTime) s.Key, s.First().Shorthand));
            
            DataTable dataTable = new DataTable("TransformDataTable");

            var columns = columnDictionary as KeyValuePair<DateTime, string>[] ?? columnDictionary.ToArray();
            AddColumns(dataTable, columns);

            foreach (var quoteGroup in sortedQuotes.GroupBy(c => c.ObservationDate))
            {
                DataRow newRow = dataTable.NewRow();
                newRow[0] = quoteGroup.Key.ToString("dd-MM-yyy");
                foreach (var quote in quoteGroup)
                {
                    if (!string.IsNullOrEmpty(quote?.Shorthand))
                    {
                        if (dataTable.Columns.Contains(quote.Shorthand))
                            newRow[quote.Shorthand] = quote.Price;
                        else
                        {
                            //In case shorthand not found, try another way to find out the correct column.
                            var keyValuePair = columns.FirstOrDefault(c => c.Key == quote.From);
                            if (keyValuePair.Equals(null))
                                newRow[keyValuePair.Value] = quote.Price;
                        }
                    }
                }
                dataTable.Rows.Add(newRow);
            }

            IEnumerable<string> getColumnNames = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            result.TransformedText += string.Join(",", getColumnNames) + Environment.NewLine;

            foreach (DataRow row in dataTable.Rows)
            {
                IEnumerable<string?> fields = row.ItemArray.Select(field => field?.ToString());
                result.TransformedText += string.Join(",", fields) + Environment.NewLine;
            }

        }
        catch (Exception e)
        {
            result.AppendError(e.Message);
        }

        return result;
    }

    /// <summary>
    /// Generate columns for data table from the given list <see cref="columns"/>.
    /// </summary>
    /// <param name="dataTable"></param>
    /// <param name="columns"></param>
    private void AddColumns(DataTable dataTable, IEnumerable<KeyValuePair<DateTime, string>> columns)
    {
        // Column for ObservationDate
        var dataColumn = new DataColumn();
        dataColumn.DataType = typeof(string);
        dataColumn.ColumnName = Constants.Space;
        dataColumn.Caption = Constants.Space;
        dataTable.Columns.Add(dataColumn);

        //Add all other columns
        foreach (var column in columns)
        {
            if (string.IsNullOrEmpty(column.Value)) continue;
            var dtColumn = new DataColumn();
            dtColumn.DataType = typeof(decimal);
            dtColumn.Caption = column.Value;
            dtColumn.ColumnName = column.Value;
            dataTable.Columns.Add(dtColumn);
        }
    }

}