using Godot;
using System;

public class Projectile : Node2D
{
	Vector2 UP = new Vector2(0, -1);
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	public bool facing_right = true;
	public Vector2 velocity;
	const int speed = 500;
	Sprite currentSprite;
	
	bool[] timer = new bool[240];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		currentSprite = GetNode<Sprite>("Area2D/Sprite");
		for(int i = 0; i < 240; i++)
		{
			timer[i] = false;
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
		if(facing_right)
		{
			velocity.x = speed * delta;
			currentSprite.FlipH = false;
		}
		else
		{
			velocity.x = -speed * delta;
			currentSprite.FlipH = true;
		}
		Translate(velocity);
		if(tick())
		QueueFree();
	}
	
	private bool tick()
	{
		for(int i = 0; i < 240; i++)
		{
			if(timer[i] == false)
			{
				timer[i] = true;
				return false;
			}
		}
		return true;
	}
}
