using System;
using System.Buffers.Binary;
using System.IO;
using OpenTK.Audio.OpenAL;
using Yari.Audio;
using Yari.Codec;

namespace Yari.Native.OpenAL
{

	public class ALAudioData : AudioData
	{

		public static void InitALDevice()
		{
			ALDevice device = ALC.OpenDevice(null);
			ALContext context = ALC.CreateContext(device, (int[]) null);
			ALC.MakeContextCurrent(context);

			Common.Manage.Reference.FREE.OnHoldReferred(() =>
			{
				ALC.DestroyContext(context);
				ALC.CloseDevice(device);
			});
		}

		public int Id;
		public int Length;

		private ALAudioData() { }

		public static unsafe ALAudioData Read(FileHandler handler)
		{
			ReadOnlySpan<byte> file = File.ReadAllBytes(handler.Path);

			int index = 0;

			if(file[index++] != 'R'
			|| file[index++] != 'I'
			|| file[index++] != 'F'
			|| file[index++] != 'F')
			{
				throw new Exception("Only support Wave file!");
			}

			BinaryPrimitives.ReadInt32LittleEndian(file.Slice(index, 4));
			index += 4;

			if(file[index++] != 'W'
			|| file[index++] != 'A'
			|| file[index++] != 'V'
			|| file[index++] != 'E')
			{
				throw new Exception("Only support Wave file!");
			}

			short numChannels = -1;
			int sampleRate = -1;
			int byteRate = -1;
			short blockAlign = -1;
			short bitsPerSample = -1;
			ALFormat format = 0;

			int buffer = AL.GenBuffer();

			while(index + 4 < file.Length)
			{
				string identifier = "" + (char) file[index++] + (char) file[index++] + (char) file[index++] + (char) file[index++];
				int size = BinaryPrimitives.ReadInt32LittleEndian(file.Slice(index, 4));
				index += 4;
				
				switch (identifier)
				{
					case "fmt " when size != 16:
						throw new Exception($"Unknown Audio Format with subchunk1 size {size}");
					case "fmt ":
						{
							short audioFormat = BinaryPrimitives.ReadInt16LittleEndian(file.Slice(index, 2));
							index += 2;
							if(audioFormat != 1)
							{
								throw new Exception($"Unknown Audio Format with ID {audioFormat}");
							}
							else
							{
								numChannels = BinaryPrimitives.ReadInt16LittleEndian(file.Slice(index, 2));
								index += 2;
								sampleRate = BinaryPrimitives.ReadInt32LittleEndian(file.Slice(index, 4));
								index += 4;
								byteRate = BinaryPrimitives.ReadInt32LittleEndian(file.Slice(index, 4));
								index += 4;
								blockAlign = BinaryPrimitives.ReadInt16LittleEndian(file.Slice(index, 2));
								index += 2;
								bitsPerSample = BinaryPrimitives.ReadInt16LittleEndian(file.Slice(index, 2));
								index += 2;

								if(numChannels == 1)
								{
									if(bitsPerSample == 8)
									{
										format = ALFormat.Mono8;
									}
									else if(bitsPerSample == 16)
									{
										format = ALFormat.Mono16;
									}
									else
									{
										throw new Exception($"Can't Play mono {bitsPerSample} sound.");
									}
								}
								else if(numChannels == 2)
								{
									if(bitsPerSample == 8)
									{
										format = ALFormat.Stereo8;
									}
									else if(bitsPerSample == 16)
									{
										format = ALFormat.Stereo16;
									}
									else
									{
										throw new Exception($"Can't Play stereo {bitsPerSample} sound.");
									}
								}
								else
								{
									throw new Exception($"Can't play audio with {numChannels} sound");
								}
							}

							break;
						}
					case "data":
						{
							ReadOnlySpan<byte> data = file.Slice(index, size);
							index += size;

							fixed(byte* pData = data)
							{
								AL.BufferData(buffer, format, pData, size, sampleRate);
							}

							break;
						}
					case "JUNK":
					case "iXML":
						// this exists to align things
						index += size;
						break;
					default:
						index += size;
						break;
				}
			}

			Common.Manage.Reference.FREE.OnHoldReferred(() =>
			{
				AL.DeleteBuffer(buffer);
			});

			ALAudioData aad = new ALAudioData();

			aad.Id = buffer;
			
			return aad;
		}

		public AudioClip CreateClip()
		{
			return new ALAudioClip(this);
		}

		public float GetLengthInMill()
		{
			throw new NotImplementedException();
		}

	}

}
