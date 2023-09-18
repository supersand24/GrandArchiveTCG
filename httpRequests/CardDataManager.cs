using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Text;

public partial class CardDataManager : HttpRequest
{

    [Export] public Godot.Collections.Dictionary<string, CardData> cards = new();
    [Export] public Godot.Collections.Dictionary<string, CardEditionData> cardEditions = new();

    CardDatabaseRequests requests = new();

    public CardData GetCardData(string uuid)
    {
        CardData ret;
        if (cards.TryGetValue(uuid, out ret))
        {
            return ret;
        } 
        else
        {
            return null;
        }
    }

    public CardEditionData GetCardEdition(string uuid)
    {
        CardEditionData ret;
        if (cardEditions.TryGetValue(uuid, out ret))
        {
            return ret;
        }
        else
        {
            return null;
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

                foreach (Dictionary entry in response.data)
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
                else requests.RemoveOldestRequest();

                break;
            default:
                GD.PrintErr("Unknown response code : " + response_code);
                break;
        }

        GetParent<Game>().LoadComplete();
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