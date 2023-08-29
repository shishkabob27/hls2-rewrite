using Editor;
using Sandbox;

namespace HLS2;
[Library( "weapon_python" ), HammerEntity]
[Alias( "weapon_357" )]
[EditorModel( "models/hl1/weapons/world/python.vmdl" )]
[Title( "Colt Python" ), Category( "Weapons" )]
public class Python : Gun
{
	public override string ViewModelPath => "models/hl1/weapons/view/v_python.vmdl";
	public override string WorldModelPath => "models/hl1/weapons/world/python.vmdl";
	public override float PrimaryAttackDelay => 0.7f;
	public override float PrimaryReloadDelay => 2.5f;
	public override int MaxPrimaryAmmo => 6;
	public override AmmoType PrimaryAmmoType => AmmoType.Python;
	public override bool Automatic => true;
	public override int Bucket => 1;
	public override int BucketWeight => 2;
	public override string CrosshairIcon => "/ui/crosshairs/crosshair3.png";
	public override string AmmoIcon => "ui/ammo2.png";
	public override string InventoryIcon => "/ui/weapons/weapon_357.png";
	public override string InventoryIconSelected => "/ui/weapons/weapon_357_selected.png";

	public override void PrimaryAttack()
	{
		PrimaryAmmo -= 1;
		ShootBullet( 40, 0.05f );
		PlaySound( "357_shot" );
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
