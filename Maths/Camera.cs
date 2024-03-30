namespace Yari.Maths
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
		public bool ShouldSeeCenter = false;

		public Rect CacheDim { get; private set; } = new Rect();

		public bool IsInSight(float x, float y, float w, float h)
		{
			vec2 testCache = new vec2(x, y);
			Project(ref testCache);
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
			CacheDim = new Rect(w: Viewport.w, h: Viewport.h, x: Center.x - Viewport.w / 2.0f, y: Center.y - Viewport.h / 2.0f);
			ShouldSeeCenter = false;
		}

		public void ToCenter()
		{
			ShouldSeeCenter = true;
		}

		public void Project(ref vec2 v)
		{
			CombinedAffine.ApplyTo(ref v);
			v.x = Viewport.w * (v.x + 1.0f) / 2.0f + Viewport.x;
			v.y = Viewport.h * (v.y + 1.0f) / 2.0f + Viewport.y;
		}

		public void Unproject(ref vec2 v)
		{
			v.x = 2.0f * (v.x - Viewport.x) / Viewport.w - 1.0f;
			v.y = 2.0f * (v.y - Viewport.y) / Viewport.h - 1.0f;
			InvertedAffine.ApplyTo(ref v);
		}

		public void Project(ref vec2 v, vec4 scrCoords)
		{
			CombinedAffine.ApplyTo(ref v);
			v.x = scrCoords.z * (v.x + 1) / 2 + scrCoords.x;
			v.y = scrCoords.w * (v.y + 1) / 2 + scrCoords.y;
		}

		public void Unproject(ref vec2 v, vec4 scrCoords)
		{
			v.x = 2.0f * (v.x - scrCoords.x) / scrCoords.z - 1.0f;
			v.y = 2.0f * (v.y - scrCoords.y) / scrCoords.w - 1.0f;
			InvertedAffine.ApplyTo(ref v);
		}

		public float ToWldX(float x)
		{
			vec2 cache = new vec2(x, -1);
			Unproject(ref cache);
			return cache.x;
		}

		public float ToWldY(float y)
		{
			vec2 cache = new vec2(-1, y);
			Unproject(ref cache);
			return cache.y;
		}

		public float ToScrX(float x)
		{
			vec2 cache = new vec2(x, -1);
			Project(ref cache);
			return cache.x;
		}

		public float ToScrY(float y)
		{
			vec2 cache = new vec2(-1, y);
			Project(ref cache);
			return cache.y;
		}

		public float ToWldX(float x, vec4 scrCoords)
		{
			vec2 cache = new vec2(x, -1);
			Unproject(ref cache, scrCoords);
			return cache.x;
		}

		public float ToWldY(float y, vec4 scrCoords)
		{
			vec2 cache = new vec2(-1, y);
			Unproject(ref cache, scrCoords);
			return cache.y;
		}

		public float ToScrX(float x, vec4 scrCoords)
		{
			vec2 cache = new vec2(x, -1);
			Project(ref cache, scrCoords);
			return cache.x;
		}

		public float ToScrY(float y, vec4 scrCoords)
		{
			vec2 cache = new vec2(-1, y);
			Project(ref cache, scrCoords);
			return cache.y;
		}

	}

}