using Godot;

public partial class CardInstance : Node2D
{

	public Vector2 posGoal = Vector2.Zero;

	[Export] bool faceUp = true;

	public double handRatio = 0.5;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		if (!faceUp)
		{
			GetNode<Sprite2D>("Image").Texture = GetOwner<Game>().cardBack;
		}
    }

	public void OnTryPickup(Node viewport, InputEvent input, int shape_idx)
	{
		if (input.IsActionPressed("click"))
		{
			Hand hand = GetParent<Hand>();
			Game game = hand.GetOwner<Game>();

			game.grabbedCard = this;
			hand.UpdateHandSpacing();
		}
	}

	public void Drop()
	{
        Hand hand = GetParent<Hand>();
        Game game = hand.GetOwner<Game>();

		Vector2 oldPos = GlobalPosition;
        hand.RemoveChild(this);
        game.AddChild(this);
		GlobalPosition = oldPos;

        hand.UpdateHandSpacing();
    }

	public void MoveToGoal(float speed)
	{
        GlobalPosition = Lerp(GlobalPosition, posGoal, speed);
    }

    private Vector2 Lerp(Vector2 beginning, Vector2 goal, float speed)
    {
        return beginning * (1 - speed) + goal * speed;
    }

}
