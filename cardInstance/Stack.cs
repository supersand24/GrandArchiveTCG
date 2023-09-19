using Godot;
using Godot.Collections;
using System.Linq;

public partial class Stack : Node2D
{

	Node cardInstancesNode;

	[Export] string zoneName = "Zone";
	[Export] bool privateZone = false;

	[Export] public Array<string> cards = new();

	public CardInstance DrawCard()
	{
		string cardUUID = GetTopCardUUID();
		if (cardUUID == null)
		{
			GD.PrintErr(zoneName + " is out of cards!");
		}
		else
		{
			CardInstance card = GD.Load<PackedScene>("res://CardInstance.tscn").Instantiate<CardInstance>();
			cardInstancesNode.AddChild(card);
			card.GlobalPosition = GlobalPosition;

			card.DrawAnim();
			card.SetCard(cardUUID);
			cards.RemoveAt(cards.Count - 1);
			GetNode<Sprite2D>("TopCardImage").Visible = cards.Count > 0; 

			PrintDeck();
			return card;
		}

		return null;

	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		cardInstancesNode = GetParent().GetOwner<Game>().GetNode("CardInstances");
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

	public void InputEvent(Node viewport, InputEvent input, int shape_idx)
	{
		if (input.IsActionPressed("click"))
		{
            GetParent().GetParent<Game>().OpenCardPicker(cards);
		}
	}

	public void PrintDeck()
	{
		var i = 0;
        foreach (string uuid in cards)
        {
			GD.Print(i + ": " + uuid);
			i++;
        }
    }

}
