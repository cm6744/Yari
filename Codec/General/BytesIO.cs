using System;
using System.IO;
using System.IO.Compression;

namespace Yari.Codec.General
{

	public class BytesIO
	{

		public static void Write(FileHandler handler, byte[] data)
		{
			using FileStream stream = new FileStream(handler.Path, FileMode.OpenOrCreate);
			using BinaryWriter bw = new BinaryWriter(stream);

			bw.Write(data, 0, data.Length);
		}

		public static void Write(FileHandler handler, ByteBufferOutChunk data)
		{
			using FileStream stream = new FileStream(handler.Path, FileMode.OpenOrCreate);
			using BinaryWriter bw = new BinaryWriter(stream);

			bw.Write(data.Bytes, data.Offset, data.Len);
		}

		public static byte[] Read(FileHandler handler)
		{
			using FileStream stream = new FileStream(handler.Path, FileMode.Open);
			using BinaryReader br = new BinaryReader(stream);

			byte[] bytes = new byte[stream.Length];
			br.Read(bytes, 0, bytes.Length);
			return bytes;
		}

		public static byte[] ReadReversed(FileHandler handler)
		{
			byte[] bytes = Read(handler);
			Array.Reverse(bytes);
			return bytes;
		}

	}

}