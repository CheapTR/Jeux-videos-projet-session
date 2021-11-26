using Godot;
using System;

public class p1_hp : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	
	int hp = 3;
	
	Sprite hp1;
	Sprite hp2;
	Sprite hp3;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hp1 = GetNode<Sprite>("Sprite");
		hp2 = GetNode<Sprite>("Sprite2");
		hp3 = GetNode<Sprite>("Sprite3");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	private void _on_player_LoseHealth()
	{
		hp--;
				if(hp == 1)
				{
					hp2.Visible = false;
				}
				if(hp == 2)
				{
					hp3.Visible = false;
				}
	}
}
