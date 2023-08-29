using Editor;
using Sandbox;

namespace HLS2;
[Library( "weapon_9mmAR" ), HammerEntity]
[Alias( "weapon_mp5" )]
[EditorModel( "models/hl1/weapons/world/mp5.vmdl" )]
[Title( "MP5" ), Category( "Weapons" )]
public class MP5 : Gun
{
	public override string ViewModelPath => "models/hl1/weapons/view/v_mp5.vmdl";
	public override string WorldModelPath => "models/hl1/weapons/world/mp5.vmdl";
	public override float PrimaryAttackDelay => 0.1f;
	public override float PrimaryReloadDelay => 1.4f;
	public override float SecondaryAttackDelay => 0.7f;
	public override int MaxPrimaryAmmo => 50;
	public override int MaxSecondaryAmmo => 10;
	public override AmmoType PrimaryAmmoType => AmmoType.Pistol;
	public override AmmoType SecondaryAmmoType => AmmoType.SMGGrenade;
	public override bool Automatic => true;
	public override int Bucket => 2;
	public override int BucketWeight => 1;
	public override string CrosshairIcon => "/ui/crosshairs/crosshair8.png";
	public override string InventoryIcon => "/ui/weapons/weapon_smg.png";
	public override string InventoryIconSelected => "/ui/weapons/weapon_smg_selected.png";
	public override CitizenAnimationHelper.HoldTypes HoldType => CitizenAnimationHelper.HoldTypes.Rifle;

	public override void PrimaryAttack()
	{
		PrimaryAmmo -= 1;
		ShootBullet( 5, 0.05f );
		PlaySound( "sounds/hl1/weapons/hks.sound" );
		(Owner as AnimatedEntity)?.SetAnimParameter( "b_attack", true );
		if ( Game.IsClient )
		{
			ShootEffects();
			DoViewPunch( 1f );
		}
	}
	public override void SecondaryAttack()
	{
		SecondaryAmmo -= 1;
		PlaySound( "sounds/hl1/weapons/glauncher.sound" );
		(Owner as AnimatedEntity)?.SetAnimParameter( "b_attack", true );
		if ( Game.IsClient )
		{
			DoViewPunch( 1.5f );
		}

		ViewModelEntity?.SetAnimParameter( "fire_grenade", true );

		if ( Game.IsServer )
		{
			var grenade = new MP5Grenade()
			{
				Owner = Owner,
				Rotation = Rotation.LookAt( Owner.AimRay.Forward ),
				Position = Owner.AimRay.Position + Owner.AimRay.Forward * 40
			};

			grenade.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
			grenade.PhysicsGroup.Velocity = Owner.AimRay.Forward * 1000;

			grenade.ApplyLocalAngularImpulse( new Vector3( 0, 300, 0 ) );
		}
	}
	public override void ReloadPrimary()
	{
		base.ReloadPrimary();
		ViewModelEntity?.SetAnimParameter( "reload", true );
	}
}
