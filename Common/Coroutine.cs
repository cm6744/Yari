using System;
using System.Collections.Generic;
using System.Threading;

namespace Yari.Common
{

	public class Coroutine
	{

		public bool IsCompleted;
		public bool IsStarted;

		public Runnable Task;

		public Coroutine(Runnable runnable)
		{
			Task = runnable;
		}

		public void Start()
		{
			ThreadPool.QueueUserWorkItem(s =>
			{
				IsStarted = true;
				Task.Invoke();
				IsCompleted = true;
			});
		}

		public void Start(TimeSpan time)
		{
			long toMills = time.Ticks + DateTime.Now.Ticks;
			Start(toMills);
		}

		void Start(long toMills)
		{
			ThreadPool.QueueUserWorkItem(_ =>
			{
				if(DateTime.Now.Ticks < toMills)
				{
					ThreadPool.QueueUserWorkItem(_ => Start(toMills));
					return;
				}
				IsStarted = true;
				Task.Invoke();
				IsCompleted = true;
			});
		}

	}

}