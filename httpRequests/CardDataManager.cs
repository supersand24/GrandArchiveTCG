using Godot;
using Godot.Collections;

public partial class CardDataManager : HttpRequest
{

    [Export] public Dictionary<string, CardData> cards = new();
    [Export] public Dictionary<string, CardEditionData> cardEditions = new();

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

    public void GetCardsFromDatabase()
    {
        CardRequest("type=ALLY&element=LUXEM");
    }
    public void CardRequest(string search = "")
    {
        Error httpError = Request("https://api.gatcg.com/cards/search?" + search);
        if (httpError != Error.Ok)
        {
            GD.PrintErr("Bad HTTP Request : https://api.gatcg.com/cards/search?" + search);
        }
    }
    public void CardRequestCompleted(long result, long response_code, string[] headers, byte[] body)
    {
        switch (response_code)
        {
            case 200:
                Variant data = Json.ParseString(body.GetStringFromUtf8());
                CardDatabaseResponse CardDatabaseResponse = new CardDatabaseResponse(data.AsGodotDictionary());

                foreach (Dictionary entry in CardDatabaseResponse.data)
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

                break;
            default:
                GD.PrintErr("Unknown response code : " + response_code);
                break;
        }

        GetParent<Game>().LoadComplete();
    }

}