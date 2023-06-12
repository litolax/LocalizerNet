using System.Text.Json;

namespace LocalizeNet;

public class Localizer
{
    private readonly JsonDocument _localization;

    public Localizer(string localization)
    {
        this._localization = JsonDocument.Parse(localization);
    }

    public string GetString(string key)
    {
        JsonElement token;

        if (TryGetToken(key, out token))
        {
            return token.GetString() ?? key;
        }

        return key;
    }

    private bool TryGetToken(string key, out JsonElement token)
    {
        string[] keys = key.Split(':');
        JsonElement root = this._localization.RootElement;

        foreach (string k in keys)
        {
            if (root.TryGetProperty(k, out JsonElement child))
            {
                root = child;
            }
            else
            {
                token = default;
                return false;
            }
        }

        token = root;
        return true;
    }
}