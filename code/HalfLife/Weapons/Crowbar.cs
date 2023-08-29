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
	public override string CrosshairIcon => "/ui/crosshairs/crosshair0.png";
	public override string InventoryIcon => "/ui/weapons/weapon_crowbar.png";
	public override string InventoryIconSelected => "/ui/weapons/weapon_crowbar_selected.png";
	public override CitizenAnimationHelper.HoldTypes HoldType => CitizenAnimationHelper.HoldTypes.Swing;


	public override void PrimaryAttack()
	{
		
		Attack(out bool hit, out Surface surface, 10);
		if ( hit )
		{
			switch (surface.ResourceName)
			{
				case "flesh":
					PlaySound( "cbar_hitbod" );
					break;
				default:
					PlaySound( "cbar_hit" );
					break;

			}
		}
		else
		{
			PlaySound( "cbar_miss" );
		}
		ViewModelEntity?.SetAnimParameter( "attack_has_hit", hit );
		ViewModelEntity?.SetAnimParameter( "fire", true );
		(Owner as AnimatedEntity)?.SetAnimParameter( "b_attack", true );

	}
}
