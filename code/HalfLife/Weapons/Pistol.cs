using Editor;
using Sandbox;

namespace HLS2;
[Library( "weapon_9mmhandgun" ), HammerEntity]
[Alias( "weapon_glock" )]
[EditorModel( "models/hl1/weapons/world/glock.vmdl" )]
[Title( "Pistol" ), Category( "Weapons" )]
public class Pistol : Gun
{
	public override string ViewModelPath => "models/hl1/weapons/view/v_glock.vmdl";
	public override string WorldModelPath => "models/hl1/weapons/world/glock.vmdl";
	public override float PrimaryAttackDelay => 0.31f;
	public override float PrimaryReloadDelay => 1.4f;
	public override int MaxPrimaryAmmo => 17;
	public override AmmoType PrimaryAmmoType => AmmoType.Pistol;
	public override bool Automatic => true;
	public override int Bucket => 1;
	public override int BucketWeight => 1;
	public override string CrosshairIcon => "/ui/crosshairs/crosshair2.png";
	public override string InventoryIcon => "/ui/weapons/weapon_pistol.png";
	public override string InventoryIconSelected => "/ui/weapons/weapon_pistol_selected.png";

	public override void PrimaryAttack()
	{
		PrimaryAmmo -= 1;
		ShootBullet( 8, 0.05f );
		PlaySound( "pistol_shot" );
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
}
