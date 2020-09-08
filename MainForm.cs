/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2020/9/9
 * 时间: 2:38
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
namespace kousa
{
	
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		MainEdit me = new MainEdit();
		String config = System.IO.Directory.GetParent(Application.ExecutablePath).ToString() + "\\save.xml";
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			try {
				me = MainEdit.readFrom(config);
			} catch (Exception) {
				me = new MainEdit();
			}
			propertyGrid1.SelectedObject = me;
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void at_exit() {
			me.saveTo(config);
		}
		
		void BtnEditClick(object sender, EventArgs e)
		{
			edit();
		}
		
		void edit() {
			Process p = new Process();
			p.StartInfo.UseShellExecute = true;
			p.StartInfo.FileName  = me.EditorFileName;
			p.StartInfo.Arguments = me.FileName;
			p.Start();
		}
		
		bool compile() {
			Process p = new Process();
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.FileName  = me.Compiler;
			p.StartInfo.Arguments = me.FileName + " " + me.CompilerOptions + " " + me.CompilerOptionsAdd;
			if (me.OutputName != "" && me.OutputName != null) {
				p.StartInfo.Arguments += "-o " + me.OutputName;
			}
			p.StartInfo.WorkingDirectory = me.FolderName;
			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.CreateNoWindow = true;
			p.Start();
			p.WaitForExit();
			
			if (p.ExitCode != 0) {
				MessageBox.Show(p.StandardError.ReadToEnd());
				return false;
			}
			return true;
		}
		
		void run() {
			Process p = new Process();
			p.StartInfo.UseShellExecute = true;
			if (me.Runner != "") {
				p.StartInfo.FileName  = me.Runner + " ";
			}
			if (me.OutputName != "" && me.OutputName != null) {
				p.StartInfo.Arguments = me.OutputName;
			} else {
				p.StartInfo.Arguments = "a.exe";
			}
			p.StartInfo.Arguments += " " + me.Arguments;
			p.StartInfo.WorkingDirectory = System.IO.Directory.GetParent(me.FileName).ToString();
			p.Start();
			p.WaitForExit();
		}
		
		void openFolder() {
			String s = System.IO.Directory.GetParent(me.FileName).ToString();
			Process.Start("explorer.exe", s);
		}
		
		void BtnOpenClick(object sender, EventArgs e)
		{
			openFolder();
		}
	}
}
