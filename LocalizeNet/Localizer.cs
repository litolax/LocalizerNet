using System.Text.Json;

namespace LocalizeNet;

public class Localizer
{
    private Dictionary<string, JsonDocument> _locales = new();
    private string _currentLanguage;
    private readonly string _fallbackLgn;
    private readonly bool _debug;
    private readonly string _keySeparator;

    public Localizer(string lng, string fallbackLgn, bool debug, string keySeparator = ".")
    {
        this._currentLanguage = lng;
        this._fallbackLgn = fallbackLgn;
        this._debug = debug;
        this._keySeparator = keySeparator;
    }

    public string T(string key)
    {
        if (!this.TryGetToken(key, out var token)) return key;

        return token.GetString() ?? key;
    }

    public void SetCurrentLanguage(string lng) => this._currentLanguage = lng.ToLowerInvariant();

    public bool AddLocaleResource(string lng, string resource)
    {
        try
        {
            var jsonResource = JsonDocument.Parse(resource);
            this._locales.Add(lng.ToLowerInvariant(), jsonResource);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return true;
    }

    public bool RemoveLocaleResource(string lng)
    {
        var result = this._locales.Remove(lng.ToLowerInvariant());
        return result;
    }

    private bool TryGetToken(string key, out JsonElement token)
    {
        if (!this._locales.TryGetValue(this._currentLanguage, out var jsonDocument) && !this._locales.TryGetValue(this._fallbackLgn, out jsonDocument))
        {
            token = default;
            return false;
        }

        JsonElement root = jsonDocument.RootElement;
        string[] keys = key.Split(this._keySeparator);

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