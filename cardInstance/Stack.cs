using Godot;
using Godot.Collections;
using System.Linq;

public partial class Stack : Node2D
{

	Node cardInstancesNode;
	[Export] PackedScene cardInstance;

	[Export] Sprite2D topCardSprite;
    [Export] Sprite2D highlightSprite;

    [Export] string zoneName = "Zone";
	[Export] public bool privateZone = true;
	[Export] int ownerNumber = 0;

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
			CardInstance card = cardInstance.Instantiate<CardInstance>();
			cardInstancesNode.AddChild(card);
			card.GlobalPosition = GlobalPosition;

			card.DrawAnim();
			card.SetCard(cardUUID);
            RemoveCard(cards.Count - 1);
			GetNode<Sprite2D>("TopCardImage").Visible = cards.Count > 0; 

			return card;
		}

		return null;

	}

	public CardInstance SpawnCard(int index)
	{
		CardInstance card = cardInstance.Instantiate<CardInstance>();
		cardInstancesNode.AddChild(card);
		card.GlobalPosition = GlobalPosition;

        card.SetCard(cards[index]);

        RemoveCard(index);
        //card.PlayFromDeck();

        return card;
	}

	public void AddCardToTop(CardInstance card)
	{
		AddCardToTop(card.uuid);
    }

	public void AddCardToTop(string card)
	{
        cards.Add(card);
        UpdateImage();
    }

	public void AddCardToBottom(CardInstance card)
	{
        AddCardToBottom(card.uuid);
	}

	public void AddCardToBottom(string card)
	{
        Array<string> newStack = new() { card };
        foreach (string uuid in cards) newStack.Add(uuid);
        cards = newStack;
    }

	private void RemoveCard(int index)
	{
        cards.RemoveAt(index);
		UpdateImage();
    }

	public void ActivateCard(int index)
	{
		Hand hand = GetParent<Hand>();
		CardEditionData card = hand.GetParent<Game>().cardDataManager.GetCardEdition(cards[index]);

        foreach (string type in card.baseData.types)
			switch (type)
			{
				case "CHAMPION": hand.champion.AddCardToTop(card.uuidEdition); break;
			}
		RemoveCard(index);
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
		if (cards.Count == 0)
		{
			topCardSprite.Visible = false;
        }
		else
		{
            topCardSprite.Visible = true;
            if (privateZone) topCardSprite.Texture = GetParent().GetParent<Game>().cardBack;
            else
            {
                CardEditionData card = GetParent().GetParent<Game>().cardDataManager.GetCardEdition(GetTopCardUUID());
                topCardSprite.Texture = GD.Load<Texture2D>("res://images/" + card.GetEditionSlug() + ".png");
            }
        }
	}

	public void Highlight()
	{
        highlightSprite.Visible = true;
    }

	public void Unhighlight()
	{
		highlightSprite.Visible = false;
	}

	public void InputEvent(Node viewport, InputEvent input, int shape_idx)
	{
		if (input.IsActionPressed("click"))
		{
			Dictionary limits = new()
			{
				{ "types", new Godot.Collections.Array() { "CHAMPION" } },
				{ "level", "==0" }
			};
			GetParent().GetParent<Game>().cardPicker.Open(cards, this, limits);
		}
		else if (input.IsActionPressed("right_click"))
		{
			Hand owner = GetParent<Hand>();
			if (highlightSprite.Visible)
			{
				owner.UnhighlightStack();
			}
			else
			{
				Highlight();
				owner.UnhighlightStack();
				owner.highlightedStack = this;
			}
		}
	}

}
