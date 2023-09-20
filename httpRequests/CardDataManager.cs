using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Text;

public partial class CardDataManager : HttpRequest
{

    public System.Collections.Generic.Dictionary<string, CardData> cards = new();
    public System.Collections.Generic.Dictionary<string, CardEditionData> cardEditions = new();

    CardDatabaseRequests requests = new();

    public CardData GetCardData(string uuid)
    {
        CardData ret;
        cards.TryGetValue(uuid, out ret);
        return ret;
    }

    public CardEditionData GetCardEdition(string uuid)
    {
        CardEditionData ret;
        if (cardEditions.TryGetValue(uuid, out ret)) return ret;
        else
        {
            CardData data = GetCardData(uuid);
            GD.PrintErr("Could not find " + uuid + ", defaulted to " + data.editions[0].uuidBase);
            return data.editions[0];
        }
    }

    public void CardRequest(Dictionary filters = null)
    {
        if (requests.Request(filters)) HTTPRequest();
        else
        {
            GD.Print("Request Pending");
        }
    }

    private void HTTPRequest(int page = 1)
    {
        Error httpError = Request(requests.GetOldestRequest() + "page=" + page);
        if (httpError != Error.Ok) GD.PrintErr("Bad HTTP Request : " + requests.GetOldestRequest());
    }

    public void CardRequestCompleted(long result, long response_code, string[] headers, byte[] body)
    {
        switch (response_code)
        {
            case 200:
                Variant data = Json.ParseString(body.GetStringFromUtf8());
                CardDatabaseResponse response = new CardDatabaseResponse(data.AsGodotDictionary());

                foreach (Godot.Collections.Dictionary entry in response.data)
                {
                    Variant uuid;
                    if (entry.TryGetValue("uuid", out uuid))
                    {
                        CardData card = new CardData(entry);
                        cards.Add(uuid.ToString(), card);
                        foreach (CardEditionData cardEdition in card.editions)
                        {
                            cardEditions.Add(cardEdition.uuidEdition, cardEdition);
                            cardEdition.baseData = card;
                        }
                    }
                }

                GD.Print("Request Complete Page " + response.page + "/" + response.totalPages);

                if (response.hasMore) HTTPRequest(response.page + 1);
                else
                {
                    requests.RemoveOldestRequest();
                    GetParent<Game>().LoadComplete();
                }

                break;
            default:
                GD.PrintErr("Unknown response code : " + response_code);
                break;
        }
    }

}

internal class CardDatabaseRequests
{
    List<Dictionary> requests = new();

    public bool Request(Dictionary filters)
    {
        requests.Add(filters);
        return requests.Count == 1;
    }

    public string GetOldestRequest()
    {
        if (requests.Count == 0) return null;
        return FormatURL();
    }

    public void RemoveOldestRequest()
    {
        if (requests.Count == 0) return;
        requests.RemoveAt(0);
    }

    public string FormatURL()
    {
        if (requests[0] == null) return "https://api.gatcg.com/cards/search?";

        StringBuilder sb = new();
        sb.Append("https://api.gatcg.com/cards/search?");
        Dictionary filters = requests[0];

        if (filters.ContainsKey("types"))
        {
            if (filters["types"].VariantType == Variant.Type.Array)
                foreach (string type in filters["types"].AsGodotArray())
                    sb.Append("type=").Append(type).Append("&");
            else sb.Append("type=").Append(filters["types"].AsString()).Append("&");
        }

        GD.Print("Formatted URL " + sb.ToString());
        return sb.ToString();
    }

}
