using Sandbox;
using System;

namespace HLS2;

public partial class Viewmodel : BaseViewModel
{

	[ConVar.Client] public static bool hl2_viewmodel { get; set; } = false;

	/// <summary>
	/// Position your view model here.
	/// </summary>
	[GameEvent.Client.PostCamera]
	public override void PlaceViewmodel()
	{
		if ( Game.IsRunningInVR )
			return;

		Position = Camera.Position;
		Rotation = Camera.Rotation;

		if (hl2_viewmodel)
		{
			HL2CalcViewModelLag( this );
			HL2AddViewmodelBob( this );
		}
		else
		{
			AddViewmodelBob( this );
			AddViewBob();
		}
	}


	
}
