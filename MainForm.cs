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
using Microsoft.CSharp.RuntimeBinder;
namespace kousa
{
	
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		dynamic me;
		String config = System.IO.Directory.GetParent(Application.ExecutablePath).ToString() + "\\save.xml";
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			resetEditor();
			try {
				if (System.IO.File.Exists(config)) {
					me = me.from(config);
				}
			} catch (Exception ex) {
				throw ex;
			}
			
		}
		
		void at_exit() {
			me.saveTo(config);
		}
		
		
		
		void ToolStripButton1Click(object sender, EventArgs e) {
			resetEditor();
		}
		void resetEditor(){
			me = Factory.createObject("MainEdit.cs", "MainEdit");
			propertyGrid1.SelectedObject = me;
			resetCommands();
		}
		
		void resetCommands() {
			while (toolStrip1.Items.Count > 1) {
				toolStrip1.Items.RemoveAt(toolStrip1.Items.Count - 1);
			}
			var l = me.getCommands();
			foreach (Tuple<String, String> tp in l) {
				var foo = tp.Item2;
				toolStrip1.Items.Add(tp.Item1, null, (o, e) => {
				    me.invoke(foo);
				});
			}
		}
	}
}
