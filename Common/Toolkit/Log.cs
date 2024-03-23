using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Yari.Common.Toolkit
{

	public class Log
	{

		public static bool UseConsoleOutput = true;
		public static bool PrintClassName = true;
		
		const string
			DEBUG = "DEBUG",
			INFO = "INFO",
			WARN = "WARN",
			FATAL = "FATAL",
			STACKTRACE = "STACKTRACE";

		public static List<string> LogStack = new List<string>();

		static void Print(string level, string info)
		{
			string className = "UNKNOWN";

			if(PrintClassName)
			{
				StackTrace stacks = new StackTrace(1);
				StackFrame frame = stacks.GetFrame(1);

				if(frame != null)
				{
					MethodBase method = frame.GetMethod();

					if(method != null && method.ReflectedType != null)
					{
						className = method.ReflectedType.Name;
					}
				}
			}
			else
			{
				className = "-";
			}

			string threadName = Thread.CurrentThread.Name;

			if(string.IsNullOrWhiteSpace(threadName))
			{
				threadName = "UNKNOWN";
			}

			string time = System.DateTime.Now.ToString("u");

			string outs = $"[{level}] [{threadName}] [{className}] [{time}] {info}";

			if(UseConsoleOutput)
			{
				Console.WriteLine(outs);
			}

			if(level != DEBUG)
			{
				LogStack.Add(outs);
			}
		}

		public static void Info(string info)
		{
			Print(INFO, info);
		}

		public static void Warn(string info)
		{
			Print(WARN, info);
		}

		public static void Fatal(string info)
		{
			Print(FATAL, info);
		}

		public static void Warn(Exception exc)
		{
			Print(WARN, exc.Message);
			Print(STACKTRACE, exc.StackTrace);
		}

		public static void Fatal(Exception exc)
		{
			Print(FATAL, exc.Message);
			Print(STACKTRACE, exc.StackTrace);
		}

	}

}