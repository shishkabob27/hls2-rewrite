﻿using Sandbox;
using System;
using System.Collections.Generic;

namespace HLS2;
public partial class AmmoStorageComponent : EntityComponent<Player>, ISingletonComponent
{
	[Net, Local]
	public IList<int> Ammo { get; set; }
	public void ClearAmmo()
	{
		Ammo.Clear();
	}

	public int AmmoCount( AmmoType type )
	{
		var iType = (int)type;
		if ( Ammo == null ) return 0;
		if ( Ammo.Count <= iType ) return 0;

		return Ammo[(int)type];
	}

	public bool SetAmmo( AmmoType type, int amount )
	{
		var iType = (int)type;
		if ( !Game.IsServer ) return false;
		if ( Ammo == null ) return false;

		while ( Ammo.Count <= iType )
		{
			Ammo.Add( 0 );
		}

		Ammo[(int)type] = amount;
		return true;
	}

	public int GiveAmmo( AmmoType type, int amount )
	{
		if ( !Game.IsServer ) return 0;
		if ( Ammo == null ) return 0;
		if ( type == AmmoType.None ) return 0;

		var total = AmmoCount( type ) + amount;
		var max = MaxAmmo( type );

		if ( total > max ) total = max;
		var taken = total - AmmoCount( type );

		SetAmmo( type, total );
		return taken;
	}

	public int TakeAmmo( AmmoType type, int amount )
	{
		if ( Ammo == null ) return 0;

		var available = AmmoCount( type );
		amount = Math.Min( available, amount );

		SetAmmo( type, available - amount );
		return amount;
	}

	public int MaxAmmo( AmmoType ammo )
	{
		switch ( ammo )
		{
			case AmmoType.Pistol: return 250;
			case AmmoType.Python: return 36;
			case AmmoType.Buckshot: return 100;
			case AmmoType.Crossbow: return 40;
			case AmmoType.RPG: return 5;
			case AmmoType.Uranium: return 100;
			case AmmoType.Grenade: return 10;
			case AmmoType.SMGGrenade: return 10;
			case AmmoType.Satchel: return 5;
			case AmmoType.Tripmine: return 5;
			case AmmoType.Snark: return 15;
			case AmmoType.Hornet: return 8;
		}

		return 0;
	}
}
public enum AmmoType
{
	None,
	Pistol,
	Buckshot,
	Crossbow,
	Python,
	Grenade,
	SMGGrenade,
	Satchel,
	Tripmine,
	Snark,
	RPG,
	Uranium,
	Hornet
}
