﻿using Editor;
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
	public override int MaxPrimaryAmmo => 50;
	public override AmmoType PrimaryAmmoType => AmmoType.Pistol;
	public override bool Automatic => true;
	public override void PrimaryAttack()
	{
		PrimaryAmmo -= 1;
		ShootBullet( 8, 0.05f );
		PlaySound( "hks" );
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