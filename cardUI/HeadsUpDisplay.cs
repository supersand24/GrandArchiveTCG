using Godot;

public partial class HeadsUpDisplay : Control
{

    [Export] Label actionHint;

    public void SetActionHint(string hint)
    {
        actionHint.Text = hint;
        actionHint.Visible = true;
    }

    public void HideActionHint()
    {
        actionHint.Visible = false;
    }

}
