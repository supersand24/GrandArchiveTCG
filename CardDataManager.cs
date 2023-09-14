using Godot;
using Godot.Collections;

public partial class CardDataManager : HttpRequest
{

    //[Export] public Array<CardData> cards = new Array<CardData>();
    [Export] public Dictionary<string, CardData> cards = new Dictionary<string, CardData>();

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
                        cards.Add(uuid.ToString(), new CardData(entry));
                }

                break;
            default:
                GD.PrintErr("Unknown response code : " + response_code);
                break;
        }
    }

}