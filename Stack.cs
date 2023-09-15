using Godot;
using Godot.Collections;
using System.Linq;

public partial class Stack : Node2D
{

	Node cardInstancesNode;

	[Export] string zoneName = "Zone";
	[Export] bool privateZone = false;

	[Export] Array<string> cards = new();

	public CardInstance DrawCard()
	{
		CardInstance card = GD.Load<PackedScene>("res://CardInstance.tscn").Instantiate<CardInstance>();
		cardInstancesNode.AddChild(card);
		card.GlobalPosition = GlobalPosition;
		card.FlipUp();
		return card;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		cardInstancesNode = GetOwner<Game>().GetNode("CardInstances");

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

}
