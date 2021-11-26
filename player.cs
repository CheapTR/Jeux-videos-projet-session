using Godot;
using System;

public class player : KinematicBody2D
{
	Vector2 UP = new Vector2(0, -1);
	const int GRAVITY = 15;
	const int MAXFALLSPEED = 200;
	const int MAXSPEED = 100;
	const int JUMPFORCE = 300;
	bool[] timer = new bool[8];

	const int ACCEL = 10;
	Vector2 vZero = new Vector2();

	bool facing_right = true;

	AnimationTree animTree;
	
	enum states {NOT_ATTACKING, ATTACKING, AIRBORNE};
	enum inputs {ONE, TWO, THREE, FOUR, FIVE, SIX, ATTACK}
	
	inputs currentInput = inputs.FIVE;
	
	inputs[] Command = new inputs[4];
	inputs[] Fireball = new inputs[4];
	inputs[] FireballLeft = new inputs[4];
	
	states currentState = states.NOT_ATTACKING;

	Vector2 motion = new Vector2();

	Sprite currentSprite;
	
	AnimationPlayer animPlayer;
	AnimationNodeStateMachinePlayback animState;
	
	AudioStreamPlayer audio;
	
	
	private bool enabled = false;
	private bool released = false;
	
	bool pauseReleased = true;
	
	[Signal]
	public delegate void LoseHealth();
	
		// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		currentSprite = GetNode<Sprite>("Sprite");
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		
		animTree = GetNode<AnimationTree>("AnimationTree");
		animState = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
		
		
		audio = new AudioStreamPlayer();
		
		Fireball[0] = inputs.TWO;
		Fireball[1] = inputs.THREE;
		Fireball[2] = inputs.SIX;
		Fireball[3] = inputs.ATTACK;
		FireballLeft[0] = inputs.TWO;
		FireballLeft[1] = inputs.ONE;
		FireballLeft[2] = inputs.FOUR;
		FireballLeft[3] = inputs.ATTACK;
		emptyTimer();
		GetNode<Label>("../../Label").Visible = false;
		GetNode<Label>("../../P1 State").Visible = false;
	}

	public override void _PhysicsProcess(float delta)
	{
		
		GetNode<Label>("../../Label").Text = "FPS : " + Engine.GetFramesPerSecond();
		GetNode<Label>("../../P1 State").Text = "State : " + currentState;
	
		if(Input.IsActionPressed("debug"))
		{
			if((released == true))
			{
				released = false;
				GetNode<Label>("../../Label").Visible = !enabled;
				GetNode<Label>("../../P1 State").Visible = !enabled;
				enabled = !enabled;
			}
		}
		else
		{
			released = true;
		}
		
		
		checkInput();
		motion.y += GRAVITY;

		if(motion.y > MAXFALLSPEED) {
			motion.y = MAXFALLSPEED;
		}

		if (facing_right) {
			currentSprite.FlipH = true;
		} else {
			currentSprite.FlipH = false;
		}
		
		if(Input.IsActionPressed("pause"))
		{
			if(pauseReleased == true)
			{
				var PauseMenu = GD.Load<PackedScene>("res://PauseMenu.tscn");
				var pausemenu = (PauseMenu) PauseMenu.Instance();
				GetParent().AddChild(pausemenu);
				
				GetTree().Paused = true;
				
				pauseReleased = false;
			}
		}
		else
		{
			pauseReleased = true;
			GetParent().GetParent().GetNode<Camera2D>("Camera2D").Current = true;
		}
		
		switch (currentState)
		{
			case states.NOT_ATTACKING:
				var input_vector = Vector2.Zero;
				if (Input.IsActionPressed("ui_right")) {
					motion.x = MAXSPEED;
					facing_right = true;
					animState.Travel("Run");
				} else if (Input.IsActionPressed("ui_left")) {
					motion.x = -MAXSPEED;
					facing_right = false;
					animState.Travel("Run");
				} else {
					motion.x = 0;
					
					
					animState.Travel("Idle");
				}
				if (IsOnFloor())
					// On ne regarde qu'un seul fois et non le maintient de la touche
					if (Input.IsActionJustPressed("ui_jump")) {
						motion.y = -JUMPFORCE;
						GD.Print($"motion.y = {motion.y}");
						Console.WriteLine($"motion.y = {motion.y}");
						currentState = states.AIRBORNE;
						
						audio = GetNode<AudioStreamPlayer>("Jump");;
						audio.Play();
					}
					if (Input.IsActionPressed("ui_right")) {
						motion.x += ACCEL;
						facing_right = true;
					} else if (Input.IsActionPressed("ui_left")) {
						motion.x -= ACCEL;
						facing_right = false;
					}
					else
					{
						motion.x = 0;
					}
				break;
				
			case states.ATTACKING:
					animState.Travel("Attack");
					motion.x = 0;
				break;
			
			case states.AIRBORNE:
				if (!IsOnFloor()) {
					animState.Travel("Jump");
					if (Input.IsActionPressed("ui_right")) {
						motion.x = MAXSPEED;
						facing_right = true;
					} else if (Input.IsActionPressed("ui_left")) {
						motion.x = -MAXSPEED;
						facing_right = false;
					}
					else
					{
						motion.x = 0;
					}
				}
				else
				{ 
					currentState = states.NOT_ATTACKING;
					audio = GetNode<AudioStreamPlayer>("Land");
					audio.Play();
				}
			break;
		}

		motion = MoveAndSlide(motion, UP);
	}
	
	public void attack_animation_finished()
	{
		currentState = states.NOT_ATTACKING;
	}
	
	private void emptyTimer()
	{
		for(int i = 0; i < 8; i++)
		{
				timer[i] = true;
		}
	}

	private bool tick()
	{
		for(int i = 0; i < 8; i++)
		{
			if(timer[i] == true)
			{
				timer[i] = false;
				return false;
			}
		}
		return true;
	}
	
	private void checkInput()
	{
		if(Input.IsActionPressed("attack"))
		{
			currentInput = inputs.ATTACK;
		}
		else if((Input.IsActionPressed("ui_left")) && (Input.IsActionPressed("ui_down")))
		{
			currentInput = inputs.ONE;
		}
		else if((Input.IsActionPressed("ui_right")) && (Input.IsActionPressed("ui_down")))
		{
			currentInput = inputs.THREE;
		}
		else if(Input.IsActionPressed("ui_right"))
		{
			currentInput = inputs.SIX;
		}
		else if(Input.IsActionPressed("ui_left"))
		{
			currentInput = inputs.FOUR;
		}
		else if(Input.IsActionPressed("ui_down"))
		{
			currentInput = inputs.TWO;
		}
		else
		{
			currentInput = inputs.FIVE;
		}
		if(currentInput != Command[3])
		{
			for(int i = 0; i < 3; i++)
			{
				Command[i] = Command[i+1];
			}
			Command[3] = currentInput;
			if(Command[3] == inputs.ATTACK)
			{
				select_move(Command);
			}
			emptyTimer();
		}
		if(tick() == true)
		{
			emptyTimer();
			Command[0] = inputs.FIVE;
			Command[1] = inputs.FIVE;
			Command[2] = inputs.FIVE;
			Command[3] = inputs.FIVE;
		}
	}
	
	private void select_move(inputs[] Command)
	{
		if(world.Assist1 == false)
		{
			if(((Fireball[0] == Command[0]) && (Fireball[1] == Command[1]) && (Fireball[2] == Command[2]) && (Fireball[3] == Command[3])) || 
			((FireballLeft[0] == Command[0]) && (FireballLeft[1] == Command[1]) && (FireballLeft[2] == Command[2]) && (FireballLeft[3] == Command[3])))
			{
				currentState = states.ATTACKING;
				emptyTimer();
				audio = GetNode<AudioStreamPlayer>("Shoot");
				audio.Play();
				
				shoot();
			}
			GD.Print(Command[0], " ", Command[1], " ", Command[2], " ", Command[3], " ");
			}
		else
		{
			currentState = states.ATTACKING;
			audio = GetNode<AudioStreamPlayer>("Shoot");
				audio.Play();
			shoot();
		}
	}
	
	
	private void shoot()
	{
		var Projectile = GD.Load<PackedScene>("res://Projectile.tscn");
		var projectile = (Projectile) Projectile.Instance();
		GetParent().AddChild(projectile);

		if(!facing_right)
		  {
			projectile.GlobalPosition = (GetNode<Position2D>("Position2DLeft").GlobalPosition);
			projectile.facing_right = facing_right;
		  }
		else
		  {
			projectile.GlobalPosition = (GetNode<Position2D>("Position2DRight").GlobalPosition);
			projectile.facing_right = facing_right;
		  }
	}
	private void _on_Area2D_area_entered(object area)
	{
		EmitSignal(nameof(LoseHealth));
	}
}



