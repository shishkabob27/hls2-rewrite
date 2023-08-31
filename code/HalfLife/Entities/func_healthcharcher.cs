using Sandbox;
using Editor;

namespace HLS2;

[Library( "func_healthcharger" ), HammerEntity]
[Solid]
[Category( "Gameplay" )]
partial class func_healthcharger : ModelEntity, IUse
{
	[Net] public float ChargerPower { get; set; }

	public override void Spawn()
	{
		base.Spawn();

		//50 HP on Easy, 40 HP on Medium, and 25 HP on Hard
		ChargerPower = 50;

		SetupPhysicsFromModel( PhysicsMotionType.Static );
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}

	public bool OnUse( Entity user )
	{
		if ( user is not Player player )
			return false;

		if (player.HasHEV == false)
			return false;

		if ( player.Health >= 100 || ChargerPower <= 0 )
		{
			PlaySound( "medshotno1" );
			return false;
		}

		var add = 10 * Time.Delta;

		if ( add > ChargerPower )
			add = ChargerPower;

		ChargerPower -= add;

		player.Health += add;
		player.Health = player.Health.Clamp( 0, 100 );
		return player.Health < 100;
	}
}
