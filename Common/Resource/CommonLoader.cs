using System.Collections.Generic;
using Yari.Codec;

namespace Yari.Common.Resource
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

		public string Namespace;
		public ResMapper ResMapper;
		public Localer Localer;

		protected CommonLoader(Namespace space)
		{
			Namespace = space.Name;
			ResMapper = space.ResMapper;
			Localer = space.Localer;
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

}