using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Diagnostics;
using System.Management;

namespace FotoSite
{
	public static class Util
	{
		public static string AddBackSlash(string foldername)
		{
			return foldername.EndsWith(Path.DirectorySeparatorChar.ToString()) || foldername.EndsWith(Path.AltDirectorySeparatorChar.ToString()) ?
					foldername : foldername + Path.DirectorySeparatorChar;
		}

		/// <summary>
		/// Kill a process, and all of its children.
		/// </summary>
		/// <param name="pid">Process ID.</param>
		/// <remarks>http://stackoverflow.com/questions/5901679/kill-process-tree-programatically-in-c-sharp</remarks>
		public static void KillProcessAndChildren(int pid)
		{
			ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
			ManagementObjectCollection moc = searcher.Get();
			foreach (ManagementObject mo in moc)
			{
				KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
			}
			try
			{
				Process proc = Process.GetProcessById(pid);
				proc.Kill();
			}
			catch (ArgumentException)
			{
				// Process already exited.
			}
		}
	}

	public static class RepeaterExtensionMethods
	{
		public static Control FindControlInHeader(this Repeater repeater, string controlName)
		{
			return repeater.Controls[0].Controls[0].FindControl(controlName);
		}

		public static Control FindControlInFooter(this Repeater repeater, string controlName)
		{
			return repeater.Controls[repeater.Controls.Count - 1].Controls[0].FindControl(controlName);
		}
	}
}