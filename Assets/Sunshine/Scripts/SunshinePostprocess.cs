using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SunshinePostprocess : MonoBehaviour
{
		SunshineCamera sunshineCamera = null;

		void OnEnable ()
		{
				sunshineCamera = GetComponent<SunshineCamera> ();
		}

		void OnRenderImage (RenderTexture source, RenderTexture destination)
		{
				if (sunshineCamera == null)
						sunshineCamera = GetComponent<SunshineCamera> ();

				if (sunshineCamera != null && sunshineCamera.enabled)
						sunshineCamera.OnPostProcess (source, destination);
				else {
						//This should never happen! But just in case...
						//Copy the buffer this once, then disable self!
						Graphics.Blit (source, destination);
						enabled = false;
				}
		}

		/// <summary>
		/// Blits a Fullscreen Triangle, which can be significantly more efficient than a Quad
		/// Needs more testing...
		/// </summary>
		public static void Blit (RenderTexture source, RenderTexture destination, Material material, int pass)
		{
				//Appears to work perfectly, but not tested thouroughly enough (DirectX with MSAA?)...
				//Use good-old Graphics.Blit() for the time being...
				Graphics.Blit (source, destination, material, pass);
				/*
		RenderTexture.active = destination;
		GL.PushMatrix();
		{
			GL.LoadOrtho();
			material.SetTexture("_MainTex", source);
			material.SetPass(pass);
			
			GL.Begin(GL.TRIANGLES);
			{
				GL.TexCoord(new Vector3(0f, 0f, 0f));
				GL.Vertex3(0f, 0f, 0f);
				
				GL.TexCoord(new Vector3(0f, 2f, 0f));
				GL.Vertex3(0f, 2f, 0f);
				
				GL.TexCoord(new Vector3(2f, 0f, 0f));
				GL.Vertex3(2f, 0f, 0f);
			}		
			GL.End();
		}
		GL.PopMatrix();
		*/
		}
}
