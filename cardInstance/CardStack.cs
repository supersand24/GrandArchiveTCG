using Godot;
using System.Collections.Generic;

public partial class CardStack : Node2D
{

	public List<CardEditionData> stack = new();
	[Export] public bool canPickup = true;
	public int owner = 0;
	public int layer = 0;
	[Export] public GameZone zone;

	[Export] public Vector2 posGoal = Vector2.Zero; //Used for moving cards from one spot to another smoothly.

	bool faceUp = true; //Used to check if top card is visible.
	[Export] Sprite2D stackSprite;
	[Export] Sprite2D highlightSprite;

	[Export] AnimationPlayer animPlayer;

	public override void _Ready()
	{
		Hide();
		animPlayer.Play("lower");
	}

    public override void _Process(double delta)
    {
        Position = Lerp(Position, posGoal, 5f * (float)delta);
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
			newCard.zone = zone;
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

    //Show Card on Info Panel
    public void MouseHovered()
	{
		//If no cards in stack, ignore object.
		if (stack.Count == 0) { return; }

        GetTree().Root.GetChild<Game>(0).infoPanel.SetStack(this);
	}

	public void Highlight()
	{
		highlightSprite.Show();
	}

	public void Unhighlight()
	{
		highlightSprite.Hide();
	}

    public void InputEvent(Node viewport, InputEvent input, int shape_idx)
    {
        if (input.IsActionPressed("left_click"))
        {
            Game game = GetTree().Root.GetChild<Game>(0);
            if (game.highlighted == null)
            {
                Highlight();
                game.UnhighlightStack();
                game.highlighted = this;
            }
			else
			{
                game.UnhighlightStack();
            }
        }
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
