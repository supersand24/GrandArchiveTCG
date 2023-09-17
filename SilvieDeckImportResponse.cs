using Godot;
using Godot.Collections;
using System.Text;

public partial class SilvieDeckImportResponse
{

    public string creator;
    public string name;
    string description;
    bool published;
    public string url;

    public Dictionary decksRaw;
    public Array<Array<string>> decks = new();

    public SilvieDeckImportResponse(Godot.Collections.Dictionary dictionary)
    {
        Variant temp;
        dictionary.TryGetValue("creator", out temp);
        creator = temp.AsString();

        dictionary.TryGetValue("name", out temp);
        name = temp.AsString();

        dictionary.TryGetValue("description", out temp);
        description = temp.AsString();

        dictionary.TryGetValue("published", out temp);
        published = temp.AsBool();

        dictionary.TryGetValue("url", out temp);
        url = temp.AsString();

        dictionary.TryGetValue("cards", out temp);
        decksRaw = temp.AsGodotDictionary();

        decksRaw.TryGetValue("main", out temp);
        decks.Add(new());
        foreach (Variant card in temp.AsGodotArray())
        {
            Variant quantity;
            Variant uuid;
            card.AsGodotDictionary().TryGetValue("quantity", out quantity);
            card.AsGodotDictionary().TryGetValue("uuid", out uuid);
            for (int i = 0; i < quantity.AsUInt16(); i++)
                decks[0].Add(uuid.AsString());
        }

        decksRaw.TryGetValue("material", out temp);
        decks.Add(new());
        foreach (Variant card in temp.AsGodotArray())
        {
            Variant quantity;
            Variant uuid;
            card.AsGodotDictionary().TryGetValue("quantity", out quantity);
            card.AsGodotDictionary().TryGetValue("uuid", out uuid);
            for (int i = 0; i < quantity.AsUInt16(); i++)
                decks[1].Add(uuid.AsString());
        }

        decksRaw.TryGetValue("sideboard", out temp);
        decks.Add(new());
        foreach (Variant card in temp.AsGodotArray())
        {
            Variant quantity;
            Variant uuid;
            card.AsGodotDictionary().TryGetValue("quantity", out quantity);
            card.AsGodotDictionary().TryGetValue("uuid", out uuid);
            for (int i = 0; i < quantity.AsUInt16(); i++)
                decks[2].Add(uuid.AsString());
        }

    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Creator : ").Append(creator).Append("\n");
        sb.Append("Name : ").Append(name).Append("\n");
        sb.Append("Description : ").Append(description).Append("\n");
        sb.Append("Published : ").Append(published).Append("\n");
        sb.Append("Url : ").Append(url).Append("\n");
        return sb.ToString();
    }
}