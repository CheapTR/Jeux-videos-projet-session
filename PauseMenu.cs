using Godot;
using System;

public class PauseMenu : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	
	bool released = false;
	bool free = false;
	bool pressedOnce = false;
	
	Sprite currentSprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		currentSprite = GetNode<Sprite>("Sprite");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
	
	if(free == true)
	{
		GetTree().Paused = false;
		QueueFree();
	}
	  if(Input.IsActionPressed("pause"))
		{
			if(released == true)
			{
				pressedOnce = true;
			}
		}
		else
		{
			if(pressedOnce == true)
			{
				free = true;
			}
			released = true;
		}
		
  }
}
