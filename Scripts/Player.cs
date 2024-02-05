using Godot;
using System;
using System.Linq;
using System.Threading;

public partial class Player : CharacterBody2D
{
  private Viewport viewport;
	private AnimatedSprite2D anim;
  private TextureProgressBar HPBar;

	private const float JUMP_VELOCITY = -200.0f;
	public const float SPEED = 100.0f;


	public enum Direction
	{
		Down,
		Right,
		Left,
		Up
	}
	public static readonly string[] DirectionTable = { "up_", "side_", "side_", "front_" };

	private Direction dir = Direction.Down;
	private int _hp = 100;
	private float gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
  public int HP
  { 
	get { return _hp; }
	set {
	  this._hp = value;
	  this.HPBar.Value = this._hp;
	}
  }

	public override void _Ready()
	{
	// this.global = GetNode<Global>("/root/Global");
	this.HPBar = GetNode<TextureProgressBar>("./HPBar");
	this.anim = GetNode<AnimatedSprite2D>("./Movement");
		this.anim.Play("front_idle");
	  // this.global.RenderScore();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = this.Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		//Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		//if (direction != Vector2.Zero)
		//{
		//	velocity = direction * Player.SPEED * (float)delta;
		//}
		//else
		//{
		//	velocity.X = Mathf.MoveToward(this.Velocity.X, 0, Player.SPEED * (float)delta);
		//	velocity.Y = 0;
		//}

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += (float)(gravity * delta);

		// Handle Jump.
		if (Input.IsActionJustPressed("move_jump") && IsOnFloor())
			velocity.Y = JUMP_VELOCITY;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		var direction = Input.GetAxis("move_left", "move_right");
		if (direction != 0)
			velocity.X = direction * SPEED;
		else
			velocity.X = Mathf.MoveToward(velocity.X, 0, SPEED);

		MoveAndSlide();

		this.PlayAnim(this.VecToMovement(velocity));

		this.Velocity = velocity;
		MoveAndSlide();
	}

	private void PlayAnim(string movement)
	{
		switch (this.dir)
		{
			case Direction.Left:
				this.anim.FlipH = true;
				break;

			case Direction.Right:
				this.anim.FlipH = false;
				break;
		}

	if (movement != "idle")
	  this.anim.Play(DirectionTable[(int)this.dir] + movement);
	else
	  this.anim.Play("front_idle");
	
	}

	private string VecToMovement(Vector2 vec)
	{
		if (vec.X == 0 && vec.Y == 0)
			return "idle";

		if (vec.X > 0)
		{
			this.dir = Direction.Right;
			return "walk";
		}
		else if (vec.X < 0)
		{
			this.dir = Direction.Left;
			return "walk";
		}

		if (vec.Y > 0)
		{
			this.dir = Direction.Up;
			return "walk";
		}
		else if (vec.Y < 0)
		{
			this.dir = Direction.Down;
			return "walk";
		}

	return ""; // vil aldrig ske

	}

  private void SetDirByMouse(Vector2 globalMousePos) {
	if ((globalMousePos.X - this.Position.X) > 0) {
	  this.dir = Direction.Right;
	  return;
	}

	this.dir = Direction.Left;
  }

  
}
