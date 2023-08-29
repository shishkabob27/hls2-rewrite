using Sandbox;
using Sandbox.UI;

namespace HLS2;
public class HUDEntity : HudEntity<HUD>
{
	public static HUDEntity Current;

	public HUDEntity()
	{
		Current = this;
		
		if ( Game.IsClient )
		{

		}
	}
}
