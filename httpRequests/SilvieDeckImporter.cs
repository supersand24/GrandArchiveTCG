using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using static Godot.OpenXRInterface;

public partial class SilvieDeckImporter : HttpRequest
{

    SilvieDeckImportOpenRequests requestManager = new();

    public void ImportDeck(string username, string id, Hand player)
    {
        requestManager.OpenRequest(username, id, "https://build-v2.silvie.org/@" + username + "/" + id, player);
        if (Request("https://api.silvie.org/api/build/v2/export/json?user=%40" + username + "&id=" + id + "&format=json") != Error.Ok)
            GD.PrintErr("Bad HTTP Request : https://api.silvie.org/api/build/v2/export/json?user=%40" + username + "&id=" + id + "&format=json");
    }

    public void DeckImportCompleted(long result, long response_code, string[] headers, byte[] body)
    {
        switch (response_code)
        {
            case 200:
                Variant data = Json.ParseString(body.GetStringFromUtf8());
                SilvieDeckImportResponse response = new SilvieDeckImportResponse(data.AsGodotDictionary());
                Hand playerHand = requestManager.IsRequestOpen(response.creator, response.url);
                if (playerHand != null)
                {
                    GD.Print("Imported " + response.name + " by " + response.creator);
                    GD.Print(response.url);
                    foreach (string cardUUID in response.decks[0])
                    {
                        playerHand.mainDeck.AddCardToBottom(GetParent<Game>().cardDataManager.GetCardEdition(cardUUID));
                    }

                    foreach (string cardUUID in response.decks[1])
                    {
                        playerHand.materialDeck.AddCardToBottom(GetParent<Game>().cardDataManager.GetCardEdition(cardUUID));
                    }

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

    public void OpenRequest(string username, string id, string deck_id, Hand playerHand)
    {
        requests.Add(new SilvieDeckImportAwait(username, id, deck_id, playerHand));
    }

    public Hand IsRequestOpen(string username, string url)
    {
        foreach (SilvieDeckImportAwait request in requests)
        {
            //if (request.username.Equals(username) && request.url.Equals(url))
                //return request.playerHand;
            if (request.username.Equals(username) && request.url.StartsWith("https://build-v2.silvie.org/@" + username + "/" + request.id))
                return request.playerHand;
        }
        GD.PrintErr("Request not found. " + url);
        return null;
    }
}

internal class SilvieDeckImportAwait
{
    public string username;
    public string url;
    public string id;
    public Hand playerHand;

    public SilvieDeckImportAwait(string username, string id, string url, Hand playerHand)
    {
        this.username = username;
        this.url = url;
        this.id = id;
        this.playerHand = playerHand;
    }

}
