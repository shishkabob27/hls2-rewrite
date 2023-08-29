using Sandbox;
using Sandbox.UI;

namespace HLS2;

class InventoryIcon : Panel
{
	public Weapon Weapon;
	public Panel Icon;
	public Panel AmmoCountFull;
	public Panel AmmoCountEmpty;
	public Panel AltAmmoCountFull;
	public Panel AltAmmoCountEmpty;

	public InventoryIcon( Weapon weapon )
	{
		Weapon = weapon;
		Icon = Add.Panel( "icon" );
		AmmoCountFull = Add.Panel( "ammocountf" );
		AmmoCountEmpty = Add.Panel( "ammocounte" );
		if ( Weapon.SecondaryAmmoType != AmmoType.None )
		{

			AltAmmoCountFull = Add.Panel( "ammocountf" );
			AltAmmoCountEmpty = Add.Panel( "ammocounte" );
			AltAmmoCountFull.SetClass( "alt", true );
			AltAmmoCountEmpty.SetClass( "alt", true );
		}

		Icon.Style.SetBackgroundImage( weapon.InventoryIcon );
	}

	internal void TickSelection( Weapon selectedWeapon )
	{
		SetClass( "active", selectedWeapon == Weapon );
		if ( selectedWeapon == Weapon )
		{
			Icon.Style.SetBackgroundImage( Weapon.InventoryIconSelected );
		}
		else
		{
			Icon.Style.SetBackgroundImage( Weapon.InventoryIcon );
		}
		SetClass( "empty", !Weapon?.IsUsable() ?? true );
	}

	public override void Tick()
	{
		base.Tick();

		if ( !Weapon.IsValid() || Weapon.Owner != Game.LocalPawn )
			Delete( true );

		if ( Game.LocalPawn is Player ply && ply.Ammo.MaxAmmo( Weapon.PrimaryAmmoType ) != 0 )
		{
			float a = ((float)ply.Ammo.AmmoCount( Weapon.PrimaryAmmoType ) / (float)ply.Ammo.MaxAmmo( Weapon.PrimaryAmmoType ));
			float b = 1 - ((float)ply.Ammo.AmmoCount( Weapon.PrimaryAmmoType ) / (float)ply.Ammo.MaxAmmo( Weapon.PrimaryAmmoType ));
			AmmoCountFull.Style.Width = a * 20;
			AmmoCountEmpty.Style.Width = b * 20 - 1;
			if ( ply.Ammo.MaxAmmo(Weapon.SecondaryAmmoType) != 0 && Weapon.SecondaryAmmoType != AmmoType.None )
			{
				float c = ((float)ply.Ammo.AmmoCount( Weapon.SecondaryAmmoType ) / (float)ply.Ammo.MaxAmmo( Weapon.SecondaryAmmoType ));
				float d = 1 - ((float)ply.Ammo.AmmoCount( Weapon.SecondaryAmmoType ) / (float)ply.Ammo.MaxAmmo( Weapon.SecondaryAmmoType ));
				AltAmmoCountFull.Style.Width = c * 20;
				AltAmmoCountEmpty.Style.Width = d * 20 - 1;
			}
		}
	}
}
