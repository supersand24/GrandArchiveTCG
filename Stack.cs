using Godot;
using Godot.Collections;
using System.Linq;

public partial class Stack : Node2D
{

	[Export] string zoneName = "Zone";
	[Export] bool privateZone = false;

	[Export] Array<string> cards = new Array<string>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		UpdateImage();

	}

	public string GetTopCardUUID()
	{
		if (cards.Count == 0) return null;
		return cards.Last();
	}

	public void UpdateImage()
	{
		if (privateZone)
		{
			GetNode<Sprite2D>("TopCardImage").Texture = GetOwner<Game>().cardBack;
		}
		else
		{
			//GetNode<Sprite2D>("TopCardImage").Texture = GetTopCard().GetNode<Sprite2D>("Image").Texture;
		}
    }

	public void OnInputEvent(Node viewport, InputEvent input, int shape_idx)
	{
		if (input.IsActionPressed("click"))
		{
			CardData card = GetOwner<Game>().cardDataManager.GetCard(GetTopCardUUID());
			GD.Print(card.name + " was drawn!");
		}
	}

}
