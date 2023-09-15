using Godot;

public partial class CardInstance : Node2D
{

	public Vector2 posGoal = Vector2.Zero;

	[Export] bool faceUp = true;

	public double handRatio = 0.5;

	AnimationPlayer animPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		//GetNode<Sprite2D>("CardFront").Visible = faceUp;
		animPlayer = GetNode<AnimationPlayer>("Animations");
    }

	public void FlipUp()
	{
		faceUp = true;
		animPlayer.Play("flipUp");
	}

	public void InputEvent(Node viewport, InputEvent input, int shape_idx)
	{
		if (input.IsActionPressed("click"))
		{
			Game game = GetParent().GetOwner<Game>();
			game.grabbedCard = this;
		}
	}

	public void Drop()
	{
		Game game = GetParent().GetOwner<Game>();
		Vector2 oldPos = GlobalPosition;
		GlobalPosition = oldPos;
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
