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
	public override float PrimaryAttackDelay => 0.6f;
	public override float PrimaryReloadDelay => 1f;
	public override int MaxPrimaryAmmo => 8;
	public override AmmoType PrimaryAmmoType => AmmoType.Buckshot;
	public override bool Automatic => true;
	public override void PrimaryAttack()
	{
		PrimaryAmmo -= 1;
		ShootBullet( 8, 0.05f );
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
}
