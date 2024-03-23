using System.Collections.Generic;
using OpenTK.Audio.OpenAL;
using Yari.Audio;
using Yari.Math;

namespace Yari.Native.OpenAL
{

	public class ALAudioClip : AudioClip
	{

		static List<ALAudioClip> ClipsPlaying = new List<ALAudioClip>();
		static List<ALAudioClip> ClipsStopped = new List<ALAudioClip>();

		public static void CheckClipStates()
		{
			ClipsPlaying.ForEach((c) => 
			{
				if(!c.IsPlaying() && !c.IsPaused())
				{
					ClipsStopped.Add(c);
				}
			});

			ClipsPlaying.RemoveAll(ClipsStopped.Contains);
			ClipsStopped.Clear();
		}

		public int Id;
		public ALAudioData Data;

		public ALAudioClip(ALAudioData data)
		{
			Id = AL.GenSource();
			Data = data;

			AL.Source(Id, ALSourcei.Buffer, data.Id);
		}

		public AudioData GetData()
		{
			return Data;
		}

		public void Play()
		{
			AL.SourcePlay(Id);

			ClipsPlaying.Add(this);
		}

		public void Loop()
		{
			AL.Source(Id, ALSourceb.Looping, true);
			AL.SourcePlay(Id);

			ClipsPlaying.Add(this);
		}

		public void Pause()
		{
			AL.SourcePause(Id);
		}

		public void Resume()
		{
			AL.SourcePlay(Id);
		}

		public void Stop()
		{
			AL.SourceStop(Id);
		}

		public void Set(ClipController controller, object v)
		{
			switch(controller)
			{
				case ClipController.Gain:
					AL.Source(Id, ALSourcef.Gain, (float) v);
					break;
				case ClipController.Pitch:
					AL.Source(Id, ALSourcef.Pitch, (float) v);
					break;
				case ClipController.SourceLocation:
					vec3 vec = (vec3) v;
					AL.Source(Id, ALSource3f.Position, vec.x, vec.y, vec.z);
					break;
			}
		}

		public object Get(ClipController controller)
		{
			switch(controller)
			{
				case ClipController.Gain:
					AL.GetSource(Id, ALSourcef.Gain, out float v1);
					return v1;
				case ClipController.Pitch:
					AL.GetSource(Id, ALSourcef.Pitch, out float v2);
					return v2;
				case ClipController.SourceLocation:
					AL.GetSource(Id, ALSource3f.Position, out float v3, out float v4, out float v5);
					return new vec3(v3, v4, v5);
			}

			return null;
		}

		public bool IsPlaying()
		{
			AL.GetSource(Id, ALGetSourcei.SourceState, out int v);
			return v == (int) ALSourceState.Playing;
		}

		public bool IsPaused()
		{
			AL.GetSource(Id, ALGetSourcei.SourceState, out int v);
			return v == (int) ALSourceState.Paused;
		}

		public bool IsDestroyed()
		{
			AL.GetSource(Id, ALGetSourcei.SourceState, out int v);
			return v == (int) ALSourceState.Stopped;
		}

	}

}
