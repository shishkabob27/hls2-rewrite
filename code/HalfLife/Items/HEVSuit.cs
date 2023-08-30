using Editor;
using Sandbox;

namespace HLS2;

[Library( "hev_suit" )]
[HammerEntity]
[Model( Model = "models/hl1/items/suit.vmdl" )]
[Title( "Hev Suit" ), Category( "items" ), Icon( "theater_comedy" )]
public partial class HevSuit : ModelEntity
{
	[Property]
	public float StartChargeAmount { get; set; }
	public Output OnPlayerTouch { get; set; }

	[Property, ResourceType( "sound" )]
	[Net]
	public string PickupSound { get; set; } = "sounds/hl1/fvox/bell.sound";
	public override void StartTouch( Entity other )
	{
		Log.Error( "touch" );
		base.Touch( other );
		if (other is Player player)
		{
			player.HasHEV = true;
			player.Suit = StartChargeAmount;
			OnPlayerTouch.Fire( other );
			Sound.FromScreen( To.Single(player),PickupSound );
		}
	}

	public override void Spawn()
	{
		base.Spawn();
		Tags.Add( "weapon" );
		SetupPhysicsFromModel( PhysicsMotionType.Static );
		
	}

}
