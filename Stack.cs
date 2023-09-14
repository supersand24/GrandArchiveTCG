using Godot;
using Godot.Collections;
using System.Linq;

public partial class Stack : Node2D
{

	[Export] string zoneName = "Zone";
	[Export] bool privateZone = false;

	Array<CardInstance> cards = new Array<CardInstance>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		UpdateImage();

	}

	public CardInstance GetTopCard()
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
			GetNode<Sprite2D>("TopCardImage").Texture = GetTopCard().GetNode<Sprite2D>("Image").Texture;
		}
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
