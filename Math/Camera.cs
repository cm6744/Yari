using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yari.Math;

namespace Yari.Math
{

	public class PerspectiveCamera
	{

		public vec2 Center = new vec2();
		public float ScaleX = 1.0f;
		public float ScaleY = 1.0f;
		public float Rotation = 0.0f;
		public Rect Viewport = new Rect();
		public affine CombinedAffine = new affine();
		public affine InvertedAffine = new affine();
		public affine ProjectionAffine = new affine();
		protected affine TranslationAffine = new affine();
		public bool ShouldSeeCenter { get; private set; } = false;
		public Rect CacheDim { get; private set; } = new Rect();

		public Rect Dim()
		{
			return CacheDim;
		}

		public bool IsInSight(float x, float y, float w, float h)
		{
			vec2 testCache = new vec2(x, y);
			Project(testCache);
			w /= ScaleX;
			h /= ScaleY;
			float vx = Center.x - Viewport.w / 2;
			float vy = Center.y - Viewport.h / 2;
			return testCache.x + w > vx && testCache.y + h > vy && testCache.x < Viewport.w && testCache.y < Viewport.h;
		}

		public void Push()
		{
			if(ShouldSeeCenter)
			{
				Center = new vec2(Viewport.w / 2.0f, Viewport.h / 2.0f);
			}

			float hw = Viewport.w / 2.0f;
			float hh = Viewport.h / 2.0f;
			ProjectionAffine.ToOrtho(ScaleX * -hw, ScaleX * hw, ScaleY * -hh, ScaleY * hh);
			TranslationAffine.Set(-Center.x, -Center.y, Rotation, 1.0f, 1.0f);
			CombinedAffine.Set(ProjectionAffine);
			CombinedAffine.Mul(TranslationAffine);
			InvertedAffine.Set(CombinedAffine);
			InvertedAffine.Invert();
			CacheDim = new Rect(w: Viewport.w, h: Viewport.h, x: Center.x - Viewport.w / 2.0f,
				y: Center.y - Viewport.h / 2.0f);
			ShouldSeeCenter = false;
		}

		public void ToCenter()
		{
			ShouldSeeCenter = true;
		}

		public void Project(vec2 v)
		{
			CombinedAffine.ApplyTo(v);
			v.x = Viewport.w * (v.x + 1.0f) / 2.0f + Viewport.x;
			v.y = Viewport.h * (v.y + 1.0f) / 2.0f + Viewport.y;
		}

		public void Unproject(vec2 v)
		{
			v.x = 2.0f * (v.x - Viewport.x) / Viewport.w - 1.0f;
			v.y = 2.0f * (v.y - Viewport.y) / Viewport.h - 1.0f;
			InvertedAffine.ApplyTo(v);
		}

		public void Project(vec2 v, float[] scrCoords)
		{
			CombinedAffine.ApplyTo(v);
			v.x = scrCoords[2] * (v.x + 1) / 2 + scrCoords[0];
			v.y = scrCoords[3] * (v.y + 1) / 2 + scrCoords[1];
		}

		public void Unproject(vec2 v, float[] scrCoords)
		{
			v.x = 2.0f * (v.x - scrCoords[0]) / scrCoords[2] - 1.0f;
			v.y = 2.0f * (v.y - scrCoords[1]) / scrCoords[3] - 1.0f;
			InvertedAffine.ApplyTo(v);
		}

		public float ToWldX(float x)
		{
			vec2 cache = new vec2(x, -1);
			Unproject(cache);
			return cache.x;
		}

		public float ToWldY(float y)
		{
			vec2 cache = new vec2(-1, y);
			Unproject(cache);
			return cache.y;
		}

		public float ToScrX(float x)
		{
			vec2 cache = new vec2(x, -1);
			Project(cache);
			return cache.x;
		}

		public float ToScrY(float y)
		{
			vec2 cache = new vec2(-1, y);
			Project(cache);
			return cache.y;
		}

		public float ToWldX(float x, float[] scrCoords)
		{
			vec2 cache = new vec2(x, -1);
			Unproject(cache, scrCoords);
			return cache.x;
		}

		public float ToWldY(float y, float[] scrCoords)
		{
			vec2 cache = new vec2(-1, y);
			Unproject(cache, scrCoords);
			return cache.y;
		}

		public float ToScrX(float x, float[] scrCoords)
		{
			vec2 cache = new vec2(x, -1);
			Project(cache, scrCoords);
			return cache.x;
		}

		public float ToScrY(float y, float[] scrCoords)
		{
			vec2 cache = new vec2(-1, y);
			Project(cache, scrCoords);
			return cache.y;
		}

	}

}