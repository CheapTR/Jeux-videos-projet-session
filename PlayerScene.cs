using Godot;
using System;

public class PlayerScene : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Signal]
	public delegate void LoseHealth();
	
	[Signal]
	public delegate void Lost();
	
	int hp = 3;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	private void _on_player_LoseHealth()
	{
		EmitSignal(nameof(LoseHealth));
		hp--;
		if(hp == 0)
		{
			world.winner = "CutMan";
			EmitSignal(nameof(Lost));
		}
	}
}



