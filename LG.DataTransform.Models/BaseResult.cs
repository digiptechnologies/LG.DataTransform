namespace LG.DataTransform.Models;

public abstract class BaseResult
{
    #region Fields
    
    private readonly IList<string?> _errors = new List<string?>();

    #endregion


    #region Properties

    public IEnumerable<string?> Errors => _errors;
    public bool HasError => _errors.Any();

    #endregion

    #region Constructor

    public BaseResult(string? error = null)
    {
        AppendError(error);
    }

    #endregion

    #region Methods

    public void AppendError(string? error)
    {
        if (!string.IsNullOrEmpty(error)) _errors.Add(error);
    }

    #endregion
}