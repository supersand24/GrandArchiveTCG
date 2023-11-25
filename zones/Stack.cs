using Godot;
using Godot.Collections;
using System.Linq;

public partial class Stack : Zone
{

	Node cardInstancesNode;
	[Export] PackedScene cardInstance;

	[Export] Sprite2D topCardSprite;
    [Export] Sprite2D highlightSprite;

	[Export] public Array<string> cards = new();

	public CardInstance MoveTopCardToZone(ExtendedZone zone)
	{
		string cardUUID = GetTopCardUUID(true);
		if (cardUUID == null) GD.PrintErr(Name + " is out of cards!"); else
		{
			//Spawn Card
			CardInstance card = cardInstance.Instantiate<CardInstance>();
			cardInstancesNode.AddChild(card);
			card.GlobalPosition = GlobalPosition;
            card.SetCard(cardUUID);

            if (zone.layer == layer)
			{
				//Same Layer
			}
			else
            {
				//Different Layer
				if (layer == 0)
                    card.DrawAnim();
				else
					card.DropAnim();
			}

            card.layer = zone.layer;
            zone.AddCard(card);

            //If no more cards in stack, make invisible.
            topCardSprite.Visible = cards.Count > 0;

			return card;

        }
		return null;
    }

	public void MoveCardToZone(Stack zone)
	{

	}

	public CardInstance DrawCard()
	{
		string cardUUID = GetTopCardUUID(true);
		if (cardUUID == null)
		{
			GD.PrintErr(Name + " is out of cards!");
		}
		else
		{
			CardInstance card = cardInstance.Instantiate<CardInstance>();
			cardInstancesNode.AddChild(card);
			card.GlobalPosition = GlobalPosition;

			card.DrawAnim();
			card.SetCard(cardUUID);
			card.layer = 1;
			GetNode<Sprite2D>("TopCardImage").Visible = cards.Count > 0;

			return card;
		}

		return null;

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

	public string GetTopCardUUID(bool remove = false)
	{
		if (cards.Count == 0) return null;
		if (remove) RemoveCard(0);
		return cards.First();
	}

	public string GetBottomCardUUID(bool remove = true)
	{
		if (cards.Count == 0) return null;
		if (remove) RemoveCard(cards.Count - 1);
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
		if (input.IsActionPressed("left_click"))
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
