using System;
using Yari.Common.Toolkit;

namespace Yari.Codec
{

	public class FileSystem
	{

		static string CurPath;

		private FileSystem()
		{
		}

		public static void AsApplicationSource()
		{
			CurPath = Environment.CurrentDirectory;
			Log.Info($"FileSystem is opened at {CurPath}. Check if resources put correctly.");
		}

		public static void AsReleaseSource()
		{
			CurPath = Environment.CurrentDirectory.Replace("Debug", "Release");
			Log.Info($"FileSystem is retargeted at {CurPath}. Do not contain \"Debug\" in your game path!");
		}

		public static FileHandler GetAbsolute(string path)
		{
			return new FileHandlerImpl(path);
		}

		public static FileHandler GetResource(string path)
		{
			return new FileHandlerImpl(CurPath + "/" + path);
		}

	}

}