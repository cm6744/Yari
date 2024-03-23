using System;

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
        }

        public static void AsReleaseSource()
        {
	        CurPath = Environment.CurrentDirectory.Replace("Debug", "Release");
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
