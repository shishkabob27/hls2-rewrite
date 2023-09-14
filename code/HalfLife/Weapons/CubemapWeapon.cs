using Editor;
using Sandbox;

namespace HLS2;
[Library( "weapon_cubemap" )]
public class CubemapWeapon : Weapon
{
	public override string ViewModelPath => "models/shadertest/envballs.vmdl";
	bool Mode = false;
	public override void PrimaryAttack()
	{
		Log.Error( "lol" );
		var a = new ModelEntity();
		a.SetModel( "models/shadertest/envballs.vmdl" );
		if (Mode)
		{
			a.SetupPhysicsFromSphere( PhysicsMotionType.Dynamic, a.Position, 10f );
		}
		
		a.Position = Owner.AimRay.Position + Owner.AimRay.Forward * 100;
		
	}
	public override void SecondaryAttack()
	{
		base.SecondaryAttack();
		if (Mode)
		{
			Mode = false;
		}else { Mode = true; }
	}
}
