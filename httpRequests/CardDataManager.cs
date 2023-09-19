using Godot;
using System.Collections.Generic;
using System.Text;

public partial class CardDataManager : HttpRequest
{

    public Dictionary<string, CardData> cards = new();
    public Dictionary<string, CardEditionData> cardEditions = new();

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
            GD.PrintErr("Could not find " + uuid + ", defaulted to " + data.editions[0].uuid);
            return data.editions[0];
        }
    }

    public void CardRequest(string cardName = null, string effect = null)
    {
        if (requests.Request(cardName, effect)) HTTPRequest();
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
                            cardEditions.Add(cardEdition.uuid, cardEdition);
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
    List<CardDatabaseAwait> requests = new();

    public bool Request(string cardName, string effect)
    {
        requests.Add(new CardDatabaseAwait(cardName, effect));
        return requests.Count == 1;
    }

    public string GetOldestRequest()
    {
        if (requests.Count == 0) return null;
        return requests[0].GetFormattedURL();
    }

    public void RemoveOldestRequest()
    {
        if (requests.Count == 0) return;
        requests.RemoveAt(0);
    }

}

internal class CardDatabaseAwait
{
    string cardName;
    string effect;

    public CardDatabaseAwait(string cardName, string effect)
    {
        this.cardName = cardName;
        this.effect = effect;
    }

    public string GetFormattedURL()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("https://api.gatcg.com/cards/search?");
        if (cardName != null) sb.Append("name=").Append(cardName).Append("&");
        if (effect != null) sb.Append("effect=").Append(effect).Append("&");
        return sb.ToString();
    }

}