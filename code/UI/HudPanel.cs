using Sandbox;
namespace HLS2;
public class HUDEntity : HudEntity<HUDRootPanel>
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
