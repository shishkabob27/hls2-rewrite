using Sandbox;
using Editor;

namespace HLS2;

[Library( "func_recharge" ), HammerEntity]
[Solid]
[Category( "Gameplay" )]
partial class func_recharge : ModelEntity, IUse
{
	[Net] public float ChargerPower { get; set; }

	public override void Spawn()
	{
		base.Spawn();

		//50 AP on Easy, 40 AP on Medium, and 25 AP on Hard, and 30 AP in multiplayer
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

		if ( player.Suit >= 100 || ChargerPower <= 0 )
		{
			PlaySound( "medshotno1" );
			return false;
		}

		var add = 10 * Time.Delta;

		if ( add > ChargerPower )
			add = ChargerPower;

		ChargerPower -= add;

		player.Suit += add;
		player.Suit = player.Suit.Clamp( 0, 100 );
		return player.Suit < 100;
	}
}
