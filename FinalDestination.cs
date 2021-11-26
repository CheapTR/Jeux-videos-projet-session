using Godot;
using System;

public class FinalDestination : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	private bool enabled = false;
	private bool released = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Label>("Label").Visible = false;
	}

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
	GetNode<Label>("Label").Text = "FPS : " + Engine.GetFramesPerSecond();
	
		if(Input.IsActionPressed("debug"))
		{
			if((released == true))
			{
				released = false;
				GetNode<Label>("Label").Visible = !enabled;
				enabled = !enabled;
			}
		}
		else
		{
			released = true;
		}
  }

	private void _on_CutManScene_Lost()
	{
		GetTree().ChangeScene("res:///WinScreen.tscn");
	}
	private void _on_player_Lost()
	{
		GetTree().ChangeScene("res:///WinScreen.tscn");
	}
}


