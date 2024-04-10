using Godot;
using System.Collections.Generic;

public partial class CardStack : Node2D
{

	public List<CardEditionData> stack = new();
	[Export] public bool canPickup = true;
	public int owner = 0;
	public int layer = 0;
	[Export] public GameZone zone;

	Vector2 posGoal = Vector2.Zero; //Used for moving cards from one spot to another smoothly.

	bool faceUp = true; //Used to check if top card is visible.
	[Export] Sprite2D stackSprite;
	[Export] Sprite2D highlightSprite;

	[Export] AnimationPlayer animPlayer;

	[Export] PackedScene cardStackScene;

	public override void _Ready()
	{
	}

	public void AddCardToTop(CardEditionData card)
	{
		stack.Insert(0, card);
	}

	public CardStack MoveTopCardToZone(CardStack stack)
	{
		CardStack topCard = cardStackScene.Instantiate<CardStack>();
		GetParent().AddChild(topCard);
		topCard.AddCardToTop(RemoveCardFromTop());
		return topCard;
	}

	public CardEditionData RemoveCardFromTop()
	{
		CardEditionData card = stack[0];
		stack.RemoveAt(0);
		return card;
	}

	public void AddCardToBottom(CardEditionData card)
	{
		stack.Add(card);
	}

    //Show Card on Info Panel
    public void MouseHovered()
	{
		//If no cards in stack, ignore object.
		if (stack.Count == 0) { return; }

        GetTree().Root.GetChild<Game>(0).infoPanel.SetStack(this);

	}

	public void MoveToGoal(float speed)
	{
		GlobalPosition = Lerp(GlobalPosition, posGoal, speed);
	}

    private Vector2 Lerp(Vector2 beginning, Vector2 goal, float speed)
    {
        return beginning * (1 - speed) + goal * speed;
    }

    #region Card Data

    #endregion

}
