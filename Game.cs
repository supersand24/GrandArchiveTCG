using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class Game : Node2D
{
	public CardDataManager cardDataManager;
	public HttpRequest cardImageManager;
	public SilvieDeckImporter silvieDeckImporter;

	[Export] public InfoPanel infoPanel;
	[Export] public HeadsUpDisplay hud;
	[Export] public CardPicker cardPicker;

	Dictionary imageCache = new();

	public CardInstance grabbedCard { get; set; } = null;
	CardInstance activatingCard = null;
	int costPaid = 0;

	public Array<Hand> players = new();

	[Export] public CompressedTexture2D cardBack;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cardImageManager = GetNode<HttpRequest>("CardImageManager");
		cardDataManager = GetNode<CardDataManager>("CardDataManager");
		silvieDeckImporter = GetNode<SilvieDeckImporter>("SilvieDeckImporter");

		//cardDataManager.CardRequest();
		cardDataManager.CardRequest( new Godot.Collections.Dictionary {
			{ "types", new Array { "CHAMPION", "ALLY" } }
		});

		players.Add(GetNode<Hand>("Hand1"));

    }

	public override void _Input(InputEvent @event)
	{
		//DEBUG Exit Game
		if (@event.IsActionPressed("ui_cancel")) GetTree().Quit();

		//Release Left Click, and currently holding a card.
		if (@event.IsActionReleased("left_click") && grabbedCard != null)
		{
			if (activatingCard == null)
			{
                ActivateCard(grabbedCard);
            }
			else
			{
				grabbedCard.canPickup = false;
				grabbedCard.MoveToZone(players[grabbedCard.ownerNumber].memory);
				costPaid++;
				if (costPaid >= activatingCard.GetCardCost())
				{
					activatingCard = null;
					hud.HideActionHint();
				}
            }
			grabbedCard = null;
		}

		//DEBUG Resolve the top card.
		if (@event.IsActionPressed("resolve"))
		{
            ExtendedZone effectStack = GetNode<ExtendedZone>("Effect Stack");
			CardInstance card = effectStack.GetLastCard(true);

			string cardType = card.GetCardTypes()[0];
			List<string> validTypes = new(){ "ALLY", "ACTION", "ATTACK", "ITEM", "DOMAIN", "PHANTASIA" };
			if ( validTypes.Contains(cardType) )
			{
				switch (cardType)
				{
					case "ALLY":
					case "DOMAIN":
					case "PHANTASIA":
					case "ITEM":
						card.MoveToZone(players[card.ownerNumber].field);
						break;
				}
			}
			else
				GD.PrintErr("Unexpected Card Type: " + cardType);

		}
	}

	public void ActivateCard(CardInstance card)
	{
        ExtendedZone effectStack = GetNode<ExtendedZone>("Effect Stack");
		activatingCard = card;
		activatingCard.canPickup = false;
		hud.SetActionHint("Choose Cards to Pay for " + card.GetCardName());
		card.MoveToZone(effectStack);
        players[0].RemoveCard(card);
		GD.Print("Activating " + card.GetDebugName());
    }

	public override void _PhysicsProcess(double delta)
	{
		if (grabbedCard != null) grabbedCard.SetGoals(GetGlobalMousePosition(), 300);

		foreach (CardInstance card in GetNode("CardInstances").GetChildren())
		{
			if (card == grabbedCard)
				card.MoveToGoal(25 * (float)delta);
			else
				card.MoveToGoal(10 * (float)delta);
		}
	}

	public override void _Draw()
    {
		DrawLine(new Vector2(0, 540), new Vector2(1920, 540), new Color(0,0,0));
    }

	private Vector2 Lerp(Vector2 beginning, Vector2 goal, float speed)
    {
        return beginning * (1 - speed) + goal * speed;
    }

    public void LoadComplete()
	{
		players[0].SpawnZones();
    }

	public void ImageRequest(string url)
	{
		Error httpError = cardImageManager.Request(url);
		if (httpError != Error.Ok)
		{
			GD.PrintErr("Bad HTTP Request : " + url);
		}
	}

	public void ImageRequestCompleted(long result, long response_code, string[] headers, byte[] body)
	{
		Image image = new();
		Error imageError = image.LoadJpgFromBuffer(body);
		switch (imageError)
		{
			case Error.Ok:
				ImageTexture imageTexture = ImageTexture.CreateFromImage(image);
				//GetNode("Card").GetNode<Sprite2D>("Image").Texture = imageTexture;
				imageCache.Add("triskit-guidance-angel-doa-alter", imageTexture);
			break;
			default:
				GD.PrintErr("An error occurred while trying to display the image.");
			break;
		}
	}
}
