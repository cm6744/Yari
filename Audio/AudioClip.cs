namespace Yari.Audio
{

	public interface AudioClip
	{

		AudioData GetData();

		void Play();

		void Loop();

		void Pause();

		void Resume();

		void Stop();

		void Set(ClipController controller, object v);

		object Get(ClipController controller);

		bool IsPlaying();

		bool IsPaused();

		bool IsDestroyed();

	}

	public enum ClipController
	{

		Gain,
		Pitch,
		FramePosition,
		SourceLocation

	}

}
