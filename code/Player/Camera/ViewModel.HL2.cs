using Sandbox;
using System;

namespace HLS2;

public partial class Viewmodel
{
	Vector3 VectorMA( Vector3 va, float scale, Vector3 vb )
	{
		Vector3 vc = Vector3.Zero;
		vc[0] = va[0] + scale * vb[0];
		vc[1] = va[1] + scale * vb[1];
		vc[2] = va[2] + scale * vb[2];
		return vc;
	}

	float g_fMaxViewModelLag = .1f;

	Vector3 m_vecLastFacing;
	void HL2CalcViewModelLag( Viewmodel viewmodel )
	{
		if ( Game.LocalPawn is not Player player ) return;
		Rotation original_angles = viewmodel.Rotation;

		Vector3 vOriginalOrigin = viewmodel.Position;
		Rotation vOriginalAngles = viewmodel.Rotation;
		// Calculate our drift	
		Vector3 forward = viewmodel.Rotation.Forward;
		//m_vecLastPos = Position - lastPos;
		if ( Time.Delta != 0.0f )
		{
			Vector3 vDifference = forward - m_vecLastFacing;

			float flSpeed = 5.0f;
			// If we start to lag too far behind, we'll increase the "catch up" speed.  Solves the problem with fast cl_yawspeed, m_yaw or joysticks
			//  rotating quickly.  The old code would slam lastfacing with origin causing the viewmodel to pop to a new position

			float flDiff = vDifference.Length;
			if ( (flDiff > g_fMaxViewModelLag) && (g_fMaxViewModelLag > 0.0f) )
			{
				float flScale = flDiff / g_fMaxViewModelLag;
				flSpeed *= flScale;
			}

			// FIXME:  Needs to be predictable?
			m_vecLastFacing = VectorMA( m_vecLastFacing, flSpeed * Time.Delta, vDifference );
			// Make sure it doesn't grow out of control!!!
			m_vecLastFacing = m_vecLastFacing.Normal;

			viewmodel.Position = VectorMA( viewmodel.Position, 5.0f, vDifference * -1.0f );
			//Assert( m_vecLastFacing.IsValid() );
		}
		forward = original_angles.Forward;
		Vector3 right = original_angles.Right;
		Vector3 up = original_angles.Up;

		float pitch = original_angles.Pitch();
		if ( pitch > 180.0f )
			pitch -= 360.0f;
		else if ( pitch < -180.0f )
			pitch += 360.0f;

		if ( g_fMaxViewModelLag == 0.0f )
		{
			viewmodel.Position = vOriginalOrigin;
			viewmodel.Rotation = vOriginalAngles;
		}
		Vector3 vel = new Vector3( player.Velocity.x, player.Velocity.y );
		float speed = vel.Length;
		//FIXME: These are the old settings that caused too many exposed polys on some models
		viewmodel.Position = VectorMA( viewmodel.Position, -pitch * 0.01f, forward );
		viewmodel.Position = VectorMA( viewmodel.Position, -pitch * 0.01f, right );
		viewmodel.Position = VectorMA( viewmodel.Position, -pitch * 0.01f, up );
	}

	//float bob;
	float bobv;
	void HL2AddViewmodelBob( BaseViewModel viewmodel )
	{

		Vector3 forward = viewmodel.Rotation.Forward;
		Vector3 right = viewmodel.Rotation.Right;

		HL2CalcViewmodelBob();

		float lateralBob = bob;
		float verticalBob = bobv;

		// Apply bob, but scaled down to 40%
		viewmodel.Position = VectorMA( viewmodel.Position, verticalBob * 0.1f, forward );

		// Z bob a bit more
		var a = viewmodel.Position;
		a[2] += verticalBob * 0.1f;
		viewmodel.Position = a;

		// bob the angles

		viewmodel.Rotation = viewmodel.Rotation.Angles().WithRoll( viewmodel.Rotation.Angles().roll + verticalBob * 0.5f ).ToRotation();
		viewmodel.Rotation = viewmodel.Rotation.Angles().WithPitch( viewmodel.Rotation.Angles().pitch - verticalBob * 0.4f ).ToRotation();
		viewmodel.Rotation = viewmodel.Rotation.Angles().WithYaw( viewmodel.Rotation.Angles().yaw - lateralBob * 0.3f ).ToRotation();


		viewmodel.Position = VectorMA( viewmodel.Position, lateralBob * 0.8f, right );
	}


	float bobtimevm;
	float lastbobtimevm;

	float RemapVal( float val, float A, float B, float C, float D )
	{
		if ( A == B )
			return val >= B ? D : C;
		return C + (D - C) * (val - A) / (B - A);
	}

	float HL2_BOB_CYCLE_MAX = 0.45f;
	float HL2_BOB_UP = 0.5f;

	void HL2CalcViewmodelBob()
	{
		float cycle;

		if ( Game.LocalPawn is not Player player ) return;
		//Assert( player );

		//NOTENOTE: For now, let this cycle continue when in the air, because it snaps badly without it

		//if ( (!gpGlobals->frametime) || (player == NULL) )
		//{
		//NOTENOTE: We don't use this return value in our case (need to restructure the calculation function setup!)
		//return 0.0f;// just use old value
		//}

		//Find the speed of the player
		Vector3 vel = new Vector3( player.Velocity.x, player.Velocity.y );
		float speed = vel.Length;

		//FIXME: This maximum speed value must come from the server.
		//		 MaxSpeed() is not sufficient for dealing with sprinting - jdw

		speed = Math.Clamp( speed, -320, 320 );

		float bob_offset = RemapVal( speed, 0, 320, 0.0f, 1.0f );

		bobtimevm += (Time.Now - lastbobtimevm) * bob_offset;
		lastbobtimevm = Time.Now;

		//Calculate the vertical bob
		cycle = bobtimevm - (int)(bobtimevm / HL2_BOB_CYCLE_MAX) * HL2_BOB_CYCLE_MAX;
		cycle /= HL2_BOB_CYCLE_MAX;

		if ( cycle < HL2_BOB_UP )
		{
			cycle = MathF.PI * cycle / HL2_BOB_UP;
		}
		else
		{
			cycle = MathF.PI + MathF.PI * (cycle - HL2_BOB_UP) / (1.0f - HL2_BOB_UP);
		}

		bobv = speed * 0.005f;
		bobv = bobv * 0.3f + bobv * 0.7f * MathF.Sin( cycle );

		bobv = Math.Clamp( bobv, -7.0f, 4.0f );

		//Calculate the lateral bob
		cycle = bobtimevm - (int)(bobtimevm / HL2_BOB_CYCLE_MAX * 2) * HL2_BOB_CYCLE_MAX * 2;
		cycle /= HL2_BOB_CYCLE_MAX * 2;

		if ( cycle < HL2_BOB_UP )
		{
			cycle = MathF.PI * cycle / HL2_BOB_UP;
		}
		else
		{
			cycle = MathF.PI + MathF.PI * (cycle - HL2_BOB_UP) / (1.0f - HL2_BOB_UP);
		}

		bob = speed * 0.005f;
		bob = bob * 0.3f + bob * 0.7f * MathF.Sin( cycle );
		bob = Math.Clamp( bob, -7.0f, 4.0f );

		//NOTENOTE: We don't use this return value in our case (need to restructure the calculation function setup!)
		//return 0.0f;
	}
}
