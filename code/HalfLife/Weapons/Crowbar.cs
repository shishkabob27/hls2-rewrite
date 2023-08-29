using Editor;
using Sandbox;

namespace HLS2;
[Library( "weapon_crowbar" ), HammerEntity]
[EditorModel( "models/hl1/weapons/world/crowbar.vmdl" )]
[Title( "Pistol" ), Category( "Weapons" )]
public class Crowbar : Melee
{
	public override string ViewModelPath => "models/hl1/weapons/view/v_crowbar.vmdl";
	public override string WorldModelPath => "models/hl1/weapons/world/crowbar.vmdl";
	public override float PrimaryAttackDelay => 0.22f;
	public override AmmoType PrimaryAmmoType => AmmoType.None;
	public override bool Automatic => true;

	public override void PrimaryAttack()
	{
		PlaySound( "cbar_miss" );
		Attack(10);
		PlaySound( "cbar_hit" );
		ViewModelEntity?.SetAnimParameter( "fire", true );
		(Owner as AnimatedEntity)?.SetAnimParameter( "b_attack", true );

	}
}
