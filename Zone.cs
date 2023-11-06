using Godot;
using Godot.Collections;

public partial class Zone : Node2D
{

    [Export] public int width = 10;
    [Export] public int height = 10;

    [Export(PropertyHint.Range, "1,3,")] public int rows = 1;
    int row1Pos = 20;
    int row2Pos = 40;
    float[] rowPosistions;

    public Array<CardInstance> cards = new();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        CalcuateRowPosistions();
	}

    private void CalcuateRowPosistions()
    {
        rowPosistions = new float[rows];
        rowPosistions[0] = height * 2 / (rows + 1);
        GD.Print(height * 2 / (rows + 1));
        for (int i = 1; i < rows; i++)
        {
            GD.Print("Calculating Row " + i);
            rowPosistions[i] = rowPosistions[0] * (i + 1);
        }
    }

    public override void _Draw()
    {
        DrawRect(new Rect2(-width, -height, width*2, height*2), Colors.Green, false);
        DrawCircle(Vector2.Zero, 3, Colors.White);
        foreach(float row in rowPosistions)
            DrawLine(new Vector2(-width, -height + row), new Vector2(width, -height + row), Colors.Red);
    }

    public void UpdateCardSpacing()
    {
        switch (cards.Count)
        {
            case 0:
                break;
            case 1:
                cards[0].posGoal = GlobalPosition;
                break;
            default:
                float step = width*2 / (cards.Count - 1);
                Vector2 newPos = GlobalPosition;
                newPos.X -= width;
                foreach (CardInstance card in cards)
                {
                    card.posGoal = newPos;
                    newPos.X += step;
                }
                break;
        }
    }
}
