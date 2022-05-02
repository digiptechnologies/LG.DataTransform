namespace LG.DataTransform.Models;

public class Quote
{
    public Quote(DateTime observationDate, string? shorthand, decimal? price, DateTime? @from, DateTime? to)
    {
        ObservationDate = observationDate;
        Shorthand = shorthand;
        Price = price;
        From = @from;
        To = to;
    }

    /// <summary>
    /// The date on which <see cref="Price"/> was observed
    /// </summary>
    public DateTime ObservationDate { get;  }
    
    /// <summary>
    /// Identification string for that Quote.
    /// </summary>
    public string? Shorthand { get;  }

    /// <summary>
    /// Price for the <see cref="Quote"/>
    /// </summary>
    public decimal? Price { get;  }

    /// <summary>
    /// Quote's Start Date
    /// </summary>
    public DateTime? From { get;  }

    /// <summary>
    /// Quote's End Date
    /// </summary>
    public DateTime? To { get;  }
}