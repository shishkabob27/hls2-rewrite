using Sandbox;

namespace HLS2;
public partial class Weapon : Carriable
{
	public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";
	public override string WorldModelPath => "weapons/rust_pistol/rust_pistol.vmdl";
	public virtual float PrimaryAttackDelay => 0.0f;
	public virtual float SecondaryAttackDelay => 0.0f;
	public virtual float PrimaryReloadDelay => 0.0f;
	public virtual float SecondaryReloadDelay => 0.0f;
	public virtual int MaxPrimaryAmmo => 0;
	public virtual int MaxSecondaryAmmo => 0;
	public virtual AmmoType PrimaryAmmoType => AmmoType.None;
	public virtual AmmoType SecondaryAmmoType => AmmoType.None;
	public virtual bool Automatic => false;
	[Net] public int PrimaryAmmo { get; set; } = 0;
	[Net] public int SecondaryAmmo { get; set; } = 0;
	public virtual int Bucket => 0;
	public virtual int BucketWeight => 1;
	public virtual int Order => (Bucket * 10000) + BucketWeight;
	public virtual string CrosshairIcon => "/ui/crosshairs/crosshair2.png";
	public virtual string AmmoIcon => "ui/ammo1.png";
	public virtual string AltAmmoIcon => "ui/ammo3.png";
	public virtual string InventoryIcon => "/ui/weapons/weapon_error.png";
	public virtual string InventoryIconSelected => "/ui/weapons/weapon_error_selected.png";
	bool IsPrimaryReloading => TimeSincePrimaryReload < PrimaryReloadDelay;
	bool IsSecondaryReloading => TimeSinceSecondaryReload < SecondaryReloadDelay;
	public override void Spawn()
	{
		base.Spawn();
		PrimaryAmmo = MaxPrimaryAmmo;
	}
	public override void FrameSimulate( IClient cl )
	{
		base.FrameSimulate( cl );
	}
	public override void Simulate( IClient cl )
	{
		if ( Owner is not Player ) return;
		if ( CanReloadPrimary() && Input.Pressed( "Reload" ) )
		{
			TimeSincePrimaryReload = 0;
			ReloadPrimary();
		}
		if ( CanPrimaryAttack() && !IsPrimaryReloading )
		{
			TimeSincePrimaryAttack = 0;
			if ( PrimaryAmmo > 0 || MaxPrimaryAmmo == 0 )
			{
				using ( LagCompensation() )
				{
					PrimaryAttack();
				}
			}
			else
			{
				if ( CanReloadPrimary() )
				{
					TimeSincePrimaryReload = 0;
					ReloadPrimary();
				}
			}
		}
		if ( CanSecondaryAttack() && !IsSecondaryReloading )
		{
			TimeSinceSecondaryAttack = 0;
			if ( SecondaryAmmo > 0 || MaxSecondaryAmmo == 0 )
			{
				using ( LagCompensation() )
				{
					SecondaryAttack();
				}
			}
			else
			{
				if ( CanReloadSecondary() )
				{
					TimeSinceSecondaryReload = 0;
					ReloadSecondary();
				}
			}
		}
	}
	public TimeSince TimeSincePrimaryReload;
	public virtual void ReloadPrimary()
	{
		var ammo = (Owner as Player).Ammo.AmmoCount( PrimaryAmmoType ).Clamp( 0, MaxPrimaryAmmo - PrimaryAmmo );
		(Owner as Player).Ammo.TakeAmmo( PrimaryAmmoType, ammo );
		PrimaryAmmo += ammo;
	}
	public TimeSince TimeSinceSecondaryReload;
	public virtual void ReloadSecondary()
	{
		var ammo = (Owner as Player).Ammo.AmmoCount( SecondaryAmmoType ).Clamp( 0, MaxSecondaryAmmo - SecondaryAmmo );
		(Owner as Player).Ammo.TakeAmmo( SecondaryAmmoType, ammo );
		SecondaryAmmo += ammo;
	}
	public virtual bool CanReloadPrimary()
	{
		return PrimaryAmmo != MaxPrimaryAmmo && (Owner as Player).Ammo.AmmoCount( PrimaryAmmoType ) > 0 && !IsPrimaryReloading;
	}
	public virtual bool CanReloadSecondary()
	{
		return false;
		//return Input.Pressed( InputButton.sometyhingidk ) && PrimaryAmmo != MaxPrimaryAmmo;
	}
	public TimeSince TimeSincePrimaryAttack;
	public TimeSince TimeSinceSecondaryAttack;
	public virtual void PrimaryAttack()
	{

	}
	public virtual void SecondaryAttack()
	{

	}
	public virtual bool CanPrimaryAttack()
	{
		return (Automatic ? Input.Down( "Attack1" ) : Input.Pressed( "Attack1" )) && TimeSincePrimaryAttack >= PrimaryAttackDelay;
	}
	public virtual bool CanSecondaryAttack()
	{
		return (Automatic ? Input.Down( "Attack2" ) : Input.Pressed( "Attack2" )) && TimeSinceSecondaryAttack >= SecondaryAttackDelay;
	}
	public virtual bool IsUsable()
	{
		if ( PrimaryAmmo > 0 ) return true;
		if ( PrimaryAmmoType == AmmoType.None ) return true;
		if ( Owner as Player == null ) return false;
		return (Owner as Player).Ammo.AmmoCount( PrimaryAmmoType ) > 0;
	}
}
