using Editor;
using Sandbox;
using System.Collections.Generic;
using XeNPC2;

namespace HLS2; 
[Library( "monster_barney" ), HammerEntity]
[EditorModel( "models/hl1/monster/barney.vmdl" )]
[Title( "Barney" ), Category( "Monsters" ), Icon( "person" )]
public class Barney : NPC
{  
	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/hl1/monster/barney.vmdl" );
		SetupPhysicsFromAABB( PhysicsMotionType.Keyframed, new Vector3( -16, -16, 0 ), new Vector3( 16, 16, 72 ) );
		Health = 20;
	}

	public override void Think()
	{
		base.Think();
		TryMove();
	}
}
