using Godot;
using System;

public partial class CardInstance : Node2D
{

	bool selected = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

    public override void _PhysicsProcess(double delta)
    {
        if (selected)
		{
			GlobalPosition = Lerp(GlobalPosition, GetGlobalMousePosition(), 25 * (float)delta);
		}
	}

    public void OnTryPickup(Node viewport, InputEvent input, int shape_idx)
	{
		if (input.IsActionPressed("click"))
		{
			selected = true;
			GetOwner<Game>().grabbedCard = this;
		}
	}

	public void Drop()
	{
		selected = false;
	}

    private Vector2 Lerp(Vector2 beginning, Vector2 goal, float speed)
	{
        return beginning * (1 - speed) + goal * speed;
    }

}
