using Godot;
using Godot.Collections;

public partial class Game : Node2D
{
	public CardDataManager cardDataManager;
	public HttpRequest cardImageManager;

	Dictionary imageCache = new();

	public CardInstance grabbedCard { get; set; } = null;

	public Array<Hand> players = new();

	[Export] public CompressedTexture2D cardBack;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cardImageManager = GetNode<HttpRequest>("CardImageManager");
		cardDataManager = GetNode<CardDataManager>("CardDataManager");

		cardDataManager.GetCardsFromDatabase();

		players.Add(GetNode<Hand>("Hand1"));

	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel")) GetTree().Quit();

		if (@event.IsActionReleased("click") && grabbedCard != null)
		{
			grabbedCard.Drop();
			GD.Print("Card Dropped at " + grabbedCard.Position);
			grabbedCard = null;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (grabbedCard != null) grabbedCard.posGoal = GetGlobalMousePosition();

		foreach (CardInstance card in players[0].cards)
		{
			if (card == grabbedCard)
				card.MoveToGoal(25 * (float)delta);
			else
				card.MoveToGoal(10 * (float)delta);
		}
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
