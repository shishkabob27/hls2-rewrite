using Editor;
using Sandbox;
using System.Collections.Generic;
using XeNPC2;

namespace HLS2; 
[Library( "monster_scientist" ), HammerEntity]
[EditorModel( "models/hl1/monster/scientist/scientist_01.vmdl" )]
[Title( "Scientist" ), Category( "Monsters" ), Icon( "person" )]
public class Scientist : NPC
{
	[Property]
	public float Body { get; set; } = -1;
	public override float MovementSpeed => 70;
	public string SetScientistModel()
	{
		if (Body == -1)
		{
			Body = Game.Random.Int( 0, 3 );
		}
		switch ( Body )
		{
			case 0: return "models/hl1/monster/scientist/scientist_01.vmdl";
			case 1: return "models/hl1/monster/scientist/scientist_02.vmdl";
			case 2: return "models/hl1/monster/scientist/scientist_03.vmdl";
			case 3: return "models/hl1/monster/scientist/scientist_04.vmdl";
			default: return "models/hl1/monster/scientist/scientist_01.vmdl";
		}
	} 

	public override void Spawn()
	{
		base.Spawn();
		SetModel( SetScientistModel() );
		SetupPhysicsFromAABB( PhysicsMotionType.Keyframed, new Vector3( -16, -16, 0 ), new Vector3( 16, 16, 72 ) );
		Health = 20; 
	}

	public override void Think()
	{
		base.Think();
		WishSpeed = 70;
		TryNavigate();
		TryMove();
	}
}
