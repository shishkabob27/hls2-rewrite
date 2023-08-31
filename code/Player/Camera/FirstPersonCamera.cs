using Sandbox;
using System;

namespace HLS2;

public class FirstPersonCamera : CameraComponent
{
	protected override void OnActivate()
	{
		base.OnActivate();
		// Set field of view to whatever the user chose in options
		Camera.FieldOfView = Screen.CreateVerticalFieldOfView( Game.Preferences.FieldOfView );
	}
	public override void FrameSimulate( IClient cl )
	{

		var pl = Entity as Player;
		// Update rotation every frame, to keep things smooth  

		pl.EyeRotation = pl.ViewAngles.ToRotation();

		Camera.Position = pl.EyePosition;
		Camera.Rotation = pl.ViewAngles.ToRotation();

		Camera.Main.SetViewModelCamera( Screen.CreateVerticalFieldOfView( 70 ) );

		AddViewBob();

		// Set the first person viewer to this, so it won't render our model
		Camera.FirstPersonViewer = Entity;

		Camera.ZNear = 8 * pl.Scale;
	}
	public override void BuildInput()
	{
		if ( Game.LocalClient.Components.TryGet<DevCamera>( out var _ ) )
			return;

		var pl = Entity as Player;
		var viewAngles = (pl.ViewAngles + Input.AnalogLook).Normal;
		pl.ViewAngles = viewAngles.WithPitch( viewAngles.pitch.Clamp( -89f, 89f ) );
		return;
	}

	void AddViewBob()
	{
		var bob = V_CalcBob();
		var a = Camera.Position;
		a.z += bob;
		Camera.Position = a;
	}

	double bobtime;
	float bob;
	float bobcycle;
	float lasttimebob;

	[ConVar.Client] public static float cl_bob { get; set; } = 0.01f;
	[ConVar.Client] public static float cl_bobcycle { get; set; } = 0.8f;
	[ConVar.Client] public static float cl_bobup { get; set; } = 0.5f;

	float V_CalcBob()
	{
		Vector3 vel;
		if ( Game.LocalPawn is not Player player ) return 0;

		if ( player.GroundEntity == null || Time.Now == lasttimebob )
		{
			// just use old value
			return bob;
		}

		lasttimebob = Time.Now;

		bobtime += Time.Delta;
		bobcycle = (float)(bobtime - (int)(bobtime / cl_bobcycle) * cl_bobcycle);
		bobcycle /= cl_bobcycle;

		if ( bobcycle < cl_bobup )
		{
			bobcycle = (float)Math.PI * bobcycle / cl_bobup;
		}
		else
		{
			bobcycle = (float)Math.PI + (float)Math.PI * (bobcycle - cl_bobup) / (1.0f - cl_bobup);
		}

		// bob is proportional to simulated velocity in the xy plane
		// (don't count Z, or jumping messes it up)
		//VectorCopy( pparams->simvel, vel );
		vel = player.Velocity.WithZ( 0 );
		//vel[2] = 0;

		bob = (float)Math.Sqrt( vel[0] * vel[0] + vel[1] * vel[1] ) * cl_bob;
		bob = bob * 0.3f + bob * 0.7f * (float)Math.Sin( bobcycle );
		bob = Math.Min( bob, 4 );
		bob = Math.Max( bob, -7 );
		return bob;

	}
}
