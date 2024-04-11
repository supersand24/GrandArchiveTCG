using Godot;
using System.Collections.Generic;

public partial class CardStack : CardInstance
{

	public List<CardEditionData> stack = new();

	public override void _Ready()
	{
		Hide();
		//animPlayer.Play("lower");
	}

    public void AddCardToTop(CardEditionData card)
	{
		Show();
		stack.Insert(0, card);
	}

	public CardStack PullTopCard()
	{
		if (stack.Count > 0)
		{
			CardStack newCard = GD.Load<PackedScene>("res://cardInstance/CardStack.tscn").Instantiate<CardStack>();
            GetParent().AddChild(newCard);
            newCard.AddCardToTop(RemoveCardFromTop());
            newCard.Position = Position;
            newCard.posGoal = Position;
			newCard.currentZone = currentZone;
			return newCard;
        }
		else
		{
            GD.PrintErr("No more cards in " + Name);
            return null;
        }
	}

	public CardEditionData RemoveCardFromTop()
	{
		CardEditionData card = stack[0];
		stack.RemoveAt(0);
		return card;
	}

	public void AddCardToBottom(CardEditionData card)
	{
        Show();
        stack.Add(card);
	}

    #region Card Data

    #endregion

}
