using Godot;
using System;

public class OptionsMenu : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	
	enum selectionV {PLAYER_1, PLAYER_2, RETURN};
	enum selectionH {ON, OFF};
	selectionV currentSelectionV = selectionV.PLAYER_1;
	selectionH currentSelectionH1 = selectionH.OFF;
	selectionH currentSelectionH2 = selectionH.OFF;
	
	bool released = false;
	
	Sprite cursorV;
	Sprite cursorH1;
	Sprite cursorH2;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cursorV = GetNode<Sprite>("cursorV");
		cursorH1 = GetNode<Sprite>("cursorH1");
		cursorH2 = GetNode<Sprite>("cursorH2");
		cursorV.Position = GetNode<Position2D>("PositionP1").Position;
		cursorH1.Position = GetNode<Position2D>("PositionP1OFF").Position;
		cursorH2.Position = GetNode<Position2D>("PositionP2OFF").Position;
	}

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
		if(Input.IsActionPressed("ui_select"))
		{
			if((released == true) && (currentSelectionV == selectionV.RETURN))
			{
				GetTree().ChangeScene("res:///world.tscn");
			}
		}
		else if((Input.IsActionPressed("right")) || (Input.IsActionPressed("left")))
		{
			if(released == true)
			{
				released = false;
				if(currentSelectionV == selectionV.PLAYER_1)
				{
					world.Assist1 = world.ChangeAssist(world.Assist1);
				}
				else if(currentSelectionV == selectionV.PLAYER_2)
				{
					world.Assist2 = world.ChangeAssist(world.Assist2);
				}
			}
		}
		else if(Input.IsActionPressed("down"))
		{
			if(released == true)
			{
				released = false;
				if(currentSelectionV == selectionV.PLAYER_1)
				{
					currentSelectionV = selectionV.PLAYER_2;
				}
				else if(currentSelectionV == selectionV.PLAYER_2)
				{
					currentSelectionV = selectionV.RETURN;
				}
			}
		}
		else if(Input.IsActionPressed("up"))
		{
			if(released == true)
			{
				released = false;
				if(currentSelectionV == selectionV.PLAYER_2)
				{
					currentSelectionV = selectionV.PLAYER_1;
				}
				else if(currentSelectionV == selectionV.RETURN)
				{
					currentSelectionV = selectionV.PLAYER_2;
				}
			}
		}
		else
		{
			released = true;
		}
		
		if(currentSelectionV == selectionV.PLAYER_1)
		{
			cursorV.Position = GetNode<Position2D>("PositionP1").Position;
		}
		else if(currentSelectionV == selectionV.PLAYER_2)
		{
			cursorV.Position = GetNode<Position2D>("PositionP2").Position;
			
		}
		else if(currentSelectionV == selectionV.RETURN)
		{
			cursorV.Position = GetNode<Position2D>("PositionReturn").Position;
		}
		
		if(world.Assist1 == true)
		{
			currentSelectionH1 = selectionH.ON;
		}
		else
		{
			currentSelectionH1 = selectionH.OFF;
		}
		
		if(world.Assist2 == true)
		{
			currentSelectionH2 = selectionH.ON;
		}
		else
		{
			currentSelectionH2 = selectionH.OFF;
		}
		
		if(currentSelectionH1 == selectionH.ON)
		{
			cursorH1.Position = GetNode<Position2D>("PositionP1ON").Position;
		}
		else
		{
			cursorH1.Position = GetNode<Position2D>("PositionP1OFF").Position;
		}
		if(currentSelectionH2 == selectionH.ON)
		{
			cursorH2.Position = GetNode<Position2D>("PositionP2ON").Position;
		}
		else
		{
			cursorH2.Position = GetNode<Position2D>("PositionP2OFF").Position;
			
		}
  }
}
