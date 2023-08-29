using Sandbox;

namespace HLS2;

public class DeadCamera : CameraComponent
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

		Camera.Position = pl.Position + new Vector3( 0f, 0f, 24f );
		Camera.Rotation = (pl.ViewAngles + new Angles( 0, 0, 80 )).ToRotation();

		Camera.Main.SetViewModelCamera( Screen.CreateVerticalFieldOfView( 70 ) );

		// Set the first person viewer to this, so it won't render our model
		Camera.FirstPersonViewer = pl.Corpse;

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
}
