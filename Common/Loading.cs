using System.Collections.Generic;
using System.Resources;
using System;
using Yari.Codec;

namespace Yari.Common
{

	public abstract class CommonLoader
	{

		protected float Progress;
		protected float Total;
		protected float Run;
		protected bool Done;

		protected Queue<Runnable> PreLoadTasks = new Queue<Runnable>();
		protected Queue<Runnable> Tasks = new Queue<Runnable>();

		protected List<CommonLoader> Children = new List<CommonLoader>();

		protected string Namespace;
		protected YariCoreResources Resources;
		protected YariCoreI18n I18n;

		protected CommonLoader(Namespace space)
		{
			Namespace = space.Name;
			Resources = space.Resources;
			I18n = space.I18n;
		}

		public void SubLoader(CommonLoader loader)
		{
			if(loader == null)
			{
				return;
			}

			Children.Add(loader);

			Total += loader.Total;
			foreach(Runnable loaderTask in loader.Tasks)
			{
				Tasks.Enqueue(loaderTask);
			}

			Done = false;
		}

		public void Enqueue(Runnable task, bool preLoad)
		{
			Enqueue0(task, preLoad);
		}

		public void Enqueue(Runnable task)
		{
			Enqueue0(task, false);
		}

		void Enqueue0(Runnable task, bool preLoad)
		{
			if(!preLoad)
			{
				Tasks.Enqueue(task);
				++Total;
				Done = false;
			}
			else
			{
				PreLoadTasks.Enqueue(task);
			}
		}

		public void DoPreLoad()
		{
			while(PreLoadTasks.Count != 0)
			{
				PreLoadTasks.Dequeue().Invoke();
			}

			foreach(CommonLoader c in Children)
			{
				c.DoPreLoad();
			}
		}

		public void Next()
		{
			if(Tasks.Count == 0)
			{
				Done = true;
				Progress = 1;
			}
			else
			{
				Tasks.Dequeue().Invoke();
				++Run;
				Progress = Run / Total;
			}
		}

		public void FlushProgress()
		{
			Total = Tasks.Count;
			Run = 0;
			Progress = 0;
			Done = false;
		}

		public void PreLoad(FileHandler fileBase, FileHandler file)
		{
			string resName = file.Path.Replace(fileBase.Path + "/", "");
			AddLoadTask(resName, file, true);
		}

		public void Scan(FileHandler fileBase)
		{
			Scan(fileBase, fileBase);
		}

		public void Scan(FileHandler fileBase, FileHandler startPos)
		{
			string basef = startPos.Path;
			FileHandler[] files = fileBase.Folders();

			foreach(FileHandler file in files)
			{
				if(file.IsFolder())
				{
					Scan(file, startPos);
				}
				else if(file.IsDocument())
				{
					string resName = file.Path.Replace(basef + "/", "");
					AddLoadTask(resName, file, false);
				}
			}

			files = fileBase.Documents();

			foreach(FileHandler file in files)
			{
				string resName = file.Path.Replace(basef + "/", "");
				AddLoadTask(resName, file, false);
			}
		}

		private void AddLoadTask(string resource, FileHandler file, bool preLoad)
		{
			ExpandTask(new Identity(Namespace, resource), file, preLoad);
		}

		protected abstract void ExpandTask(Identity resource, FileHandler file, bool preLoad);

	}

	public class YariCoreResources
	{

		private Dictionary<string, object> ResDict = new();

		public void Load(string key, object o)
		{
			ResDict[key] = o;
		}

		public void Unload(string key)
		{
			ResDict.Remove(key);
		}

		public T Get<T>(string key)
		{
			return (T) ResDict[key];
		}

	}

	public class YariCoreI18n
	{

		public Dictionary<string, BinaryCompound> Langs = new();
		public string LangKey = "EN_US";

		public void Load(string key, BinaryCompound compound)
		{
			Langs[key] = compound;
		}

		public string Get(string key)
		{
			return Langs[LangKey].GetString(key);
		}

	}

}