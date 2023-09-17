using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class SilvieDeckImporter : HttpRequest
{

    SilvieDeckImportOpenRequests requestManager = new();

    public void ImportDeck(string username, string id, Hand player)
    {
        requestManager.OpenRequest(username, "https://build.silvie.org/@" + username + "/" + id, player);
        Error httpError = Request("https://api.silvie.org/api/build/tts?user=%40" + username + "&id=" + id + "&format=json");
        if (httpError != Error.Ok)
        {
            GD.PrintErr("Bad HTTP Request : https://api.silvie.org/api/build/tts?user=%40" + username + "&id=" + id + "&format=json");
        }
    }

    public void DeckImportCompleted(long result, long response_code, string[] headers, byte[] body)
    {
        switch (response_code)
        {
            case 200:
                Variant data = Json.ParseString(body.GetStringFromUtf8());
                SilvieDeckImportResponse response = new SilvieDeckImportResponse(data.AsGodotDictionary());
                Hand playerHand = requestManager.IsRequestOpen(response.creator, response.url);
                if (playerHand == null)
                {
                    GD.PrintErr("Request was lost.");
                }
                else
                {
                    GD.Print("Imported " + response.name + " by " + response.creator);
                    GD.Print(response.url);
                    playerHand.mainDeck.cards = response.decks[0];
                    playerHand.materialDeck.cards = response.decks[1];
                }
                break;
            default:
                GD.PrintErr("Unknown response code : " + response_code);
                break;
        }
    }

}

internal class SilvieDeckImportOpenRequests
{
    List<SilvieDeckImportAwait> requests = new();

    public void OpenRequest(string username, string deck_id, Hand playerHand)
    {
        requests.Add(new SilvieDeckImportAwait(username, deck_id, playerHand));
    }

    public Hand IsRequestOpen(string username, string url)
    {
        foreach (SilvieDeckImportAwait request in requests)
        {
            if (request.username.Equals(username) && request.url.Equals(url))
                return request.playerHand;
        }
        return null;
    }
}

internal class SilvieDeckImportAwait
{
    public string username;
    public string url;
    public Hand playerHand;

    public SilvieDeckImportAwait(string username, string url, Hand playerHand)
    {
        this.username = username;
        this.url = url;
        this.playerHand = playerHand;
    }

}
