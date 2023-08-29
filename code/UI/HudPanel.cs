using Sandbox;
using Sandbox.UI;

namespace HLS2;
public class HUDEntity : HudEntity<HUDRootPanel>
{
	public static HUDEntity Current;

	public HUDEntity()
	{
		Current = this;
		
		if ( Game.IsClient )
		{
			StyleSheet.FromFile( "/resource/hud.scss" );
			Log.Info( StyleSheet.Loaded );
		}
	}
}
