namespace LocalizeNet;

public class Localizer
{
    private readonly string _localization;

    public Localizer(string localization)
    {
        this._localization = localization;
    }

    public string GetLocalization()
    {
        return this._localization;
    }
}