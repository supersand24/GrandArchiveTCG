using Godot;
using Godot.Collections;

public partial class Game : Node
{
	public CardDataManager cardDataManager;
    HttpRequest imageRequest;

    Dictionary imageCache = new Dictionary();

	public CardInstance grabbedCard { get; set; } = null;

	[Export] public CompressedTexture2D cardBack;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		imageRequest = GetNode<HttpRequest>("ImageRequest");
		cardDataManager = GetNode<CardDataManager>("CardDataManager");

		cardDataManager.GetCardsFromDatabase();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionReleased("click") && grabbedCard != null)
		{
			grabbedCard.Drop();
			grabbedCard = null;
		}
	}

	public void ImageRequest(string url)
	{
		Error httpError = imageRequest.Request(url);
		if (httpError != Error.Ok)
		{
			GD.PrintErr("Bad HTTP Request : " + url);
		}
	}

	public void ImageRequestCompleted(long result, long response_code, string[] headers, byte[] body)
	{
		Image image = new Image();
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
