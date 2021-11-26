using Godot;
using System;

public class world : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	
	static public String winner;
	static public bool Assist1 = false;
	static public bool Assist2 = false;
	bool released = false;
	
	
	enum selection {FINAL_DESTINATION, VALLEY_OF_THE_END, OPTIONS, EXIT};
	selection currentSelection = selection.FINAL_DESTINATION;
	
	Sprite cursor;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cursor = GetNode<Sprite>("cursor");
		cursor.GlobalPosition = (GetNode<Position2D>("PositionFD").GlobalPosition);
	}

  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if(Input.IsActionPressed("ui_select"))
		{
			if(released == true)
			Select_Scene();
		}
		else
		{
			if((Input.IsActionPressed("right")) || (Input.IsActionPressed("left")))
			{
				if(released == true)
				{
					released = false;
					if(currentSelection == selection.FINAL_DESTINATION)
					{
						currentSelection = selection.VALLEY_OF_THE_END;
					}
					else if(currentSelection == selection.VALLEY_OF_THE_END)
					{
						currentSelection = selection.FINAL_DESTINATION;
					}
				}
			}
			else if(Input.IsActionPressed("down"))
			{
				if(released == true)
				{
					released = false;
					if((currentSelection == selection.FINAL_DESTINATION) || (currentSelection == selection.VALLEY_OF_THE_END))
					{
						currentSelection = selection.OPTIONS;
					}
					else if (currentSelection == selection.OPTIONS)
					{
						currentSelection = selection.EXIT;
					}
				}
			}
			else if(Input.IsActionPressed("up"))
			{
				if(released == true)
				{
					released = false;
					if(currentSelection == selection.OPTIONS)
					{
						currentSelection = selection.FINAL_DESTINATION;
					}
					else if (currentSelection == selection.EXIT)
					{
						currentSelection = selection.OPTIONS;
					}
				}
			}
			else
			{
				released = true;
			}
			
			if(currentSelection == selection.FINAL_DESTINATION)
			{
				cursor.GlobalPosition = (GetNode<Position2D>("PositionFD").GlobalPosition);
			}
			if(currentSelection == selection.VALLEY_OF_THE_END)
			{
				cursor.GlobalPosition = (GetNode<Position2D>("PositionVotE").GlobalPosition);
			}
			if(currentSelection == selection.OPTIONS)
			{
				cursor.GlobalPosition = (GetNode<Position2D>("PositionOptions").GlobalPosition);
			}
			if(currentSelection == selection.EXIT)
			{
				cursor.GlobalPosition = (GetNode<Position2D>("PositionExit").GlobalPosition);
			}
		}
	}

	private void Select_Scene()
	{
		if(currentSelection == selection.FINAL_DESTINATION)
		{
			GetTree().ChangeScene("res:///FinalDestination.tscn");
		}
		if(currentSelection == selection.VALLEY_OF_THE_END)
		{
			GetTree().ChangeScene("res:///Valley_of_the_End.tscn");
		}
		if(currentSelection == selection.OPTIONS)
		{
			GetTree().ChangeScene("res:///OptionsMenu.tscn");
		}
		if(currentSelection == selection.EXIT)
		{
			GetTree().Quit();
		}
	}
	public static bool ChangeAssist(bool player)
	{
		GD.Print(!player);
		return !player;
	}
}


