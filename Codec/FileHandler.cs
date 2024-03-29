using System.Collections.Generic;
using System.IO;

namespace Yari.Codec
{

	//In control of files and folders without "/" contained.
	//You should consider the file type when using because it has no handler for error.
	public interface FileHandler
	{

		public string Path { get; }

		bool IsFolder();

		bool IsDocument();

		string Format();

		string Name();

		FileHandler Enter(string name);

		FileHandler Exit();

		FileHandler[] Folders();

		FileHandler[] Documents();

		bool Exists();

		void Mkdirs();

		void MkDocs();

		void Delete();

	}

	public class FileHandlerImpl : FileHandler
	{

		readonly string File;
		readonly bool Doc, Fol;

		public FileHandlerImpl(string file)
		{
			File = file;

			Doc = System.IO.File.Exists(file);
			Fol = Directory.Exists(file);
		}

		public void Delete()
		{
			if(Doc)
			{
				System.IO.File.Delete(File);
			}
			else if(Fol)
			{
				Directory.Delete(File, true);
			}
		}

		public FileHandler[] Documents()
		{
			List<FileHandler> files = new List<FileHandler>();

			foreach(FileInfo info in new DirectoryInfo(File).GetFiles())
			{
				files.Add(new FileHandlerImpl(info.FullName));
			}

			return files.ToArray();
		}

		public FileHandler Enter(string name)
		{
			string call;

			if(File.EndsWith("/"))
			{
				call = File + name;
			}
			else
			{
				call = File + "/" + name;
			}

			return new FileHandlerImpl(call);
		}

		public bool Exists()
		{
			return Doc || Fol;
		}

		public FileHandler Exit()
		{
			int idx = File.LastIndexOf('/');
			string call = File.Substring(0, idx);
			return new FileHandlerImpl(call);
		}

		public FileHandler[] Folders()
		{
			List<FileHandler> files = new List<FileHandler>();

			foreach(DirectoryInfo info in new DirectoryInfo(File).GetDirectories())
			{
				files.Add(new FileHandlerImpl(info.FullName));
			}

			return files.ToArray();
		}

		public string Format()
		{
			int idx = File.LastIndexOf('.');
			return File.Substring(idx + 1);
		}

		public bool IsDocument()
		{
			return Doc;
		}

		public string Path => File;

		public bool IsFolder()
		{
			return Fol;
		}

		public void Mkdirs()
		{
			new DirectoryInfo(File).Create();
		}

		public void MkDocs()
		{
			Exit().Mkdirs();
			new FileInfo(File).Create().Close();
		}

		public string Name()
		{
			int idx = File.LastIndexOf('.');
			int idx1 = File.LastIndexOf('/') + 1;
			return File.Substring(idx1, idx - idx1);
		}

	}

}