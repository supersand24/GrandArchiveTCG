using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Linq;
using static Godot.OpenXRHand;

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
		Hide();
		animPlayer.Play("lower");
	}

	public void AddCardToTop(CardEditionData card)
	{
		Show();
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
