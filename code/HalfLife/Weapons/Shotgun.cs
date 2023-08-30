using Editor;
using Sandbox;

namespace HLS2;
[Library( "weapon_shotgun" ), HammerEntity]
[EditorModel( "models/hl1/weapons/world/shotgun.vmdl" )]
[Title( "Shotgun" ), Category( "Weapons" )]
public class Shotgun : Gun
{
	public override string ViewModelPath => "models/hl1/weapons/view/v_shotgun.vmdl";
	public override string WorldModelPath => "models/hl1/weapons/world/shotgun.vmdl";
	public override string WorldPlayerModelPath => "models/hl1/weapons/player/p_shotgun.vmdl";
	public override float PrimaryAttackDelay => 0.6f;
	public override float PrimaryReloadDelay => 1f;
	public override float SecondaryAttackDelay => 0.6f;
	public override float SecondaryReloadDelay => 1f;

	public override int MaxPrimaryAmmo => 8;
	public override AmmoType PrimaryAmmoType => AmmoType.Buckshot;
	public override bool Automatic => true;
	public override int Bucket => 2;
	public override int BucketWeight => 2;
	public override string CrosshairIcon => "/ui/crosshairs/crosshair6.png";
	public override string AmmoIcon => "ui/ammo4.png";
	public override string InventoryIcon => "/ui/weapons/weapon_shotgun.png";
	public override string InventoryIconSelected => "/ui/weapons/weapon_shotgun_selected.png";
	public override CitizenAnimationHelper.HoldTypes HoldType => CitizenAnimationHelper.HoldTypes.Shotgun;

	public override void PrimaryAttack()
	{
		PrimaryAmmo -= 1;
		ShootBullet( 5, 0.1f, 0, 6 );
		PlaySound( "shotgun_shot" );
		(Owner as AnimatedEntity)?.SetAnimParameter( "b_attack", true );
		if ( Game.IsClient )
		{
			ShootEffects();
			DoViewPunch( 1f );
		}
	}
	public override void ReloadPrimary()
	{
		base.ReloadPrimary();
		ViewModelEntity?.SetAnimParameter( "reload", true );
	}

	public override void SecondaryAttack()
	{
		if ( PrimaryAmmo > 0 || MaxPrimaryAmmo == 0 )
		{
			if ( PrimaryAmmo > 1 )
			{
				PrimaryAmmo -= 2;
				ShootBullet( 10, 0.1f, 0, 6 );
				PlaySound( "shotgun_shot" );
				(Owner as AnimatedEntity)?.SetAnimParameter( "b_attack", true );
				if ( Game.IsClient )
				{
					ShootEffects();
					DoViewPunch( 10f );
				}
			} 
			else
			{
				PrimaryAttack();
			}
		}
	}
}
