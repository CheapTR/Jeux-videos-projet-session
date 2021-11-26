using Godot;
using System;

public class WinScreen : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	Vector2 SpriteScale = new Vector2((float)3, (float)3);
	Vector2 SpritePosition = new Vector2((float)0, (float)-26.426);
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Sprite>(world.winner).Position = SpritePosition;
		GetNode<Sprite>(world.winner).Scale = SpriteScale;
	}

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
	  if(Input.IsActionPressed("ui_select"))
		GetTree().ChangeScene("res:///world.tscn");
  }
}
