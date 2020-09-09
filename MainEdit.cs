/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2020/9/9
 * 时间: 2:42
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;
	using FileNameEditor = System.Windows.Forms.Design.FileNameEditor;
	/// <summary>
	/// Description of MainEdit.
	/// </summary>
	public class MainEdit
	{
		
		public String filename;
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		[Description("要打开的主文件名")]
		[Category("主要设置")]
		[DisplayName("主文件名")]
		public String FileName { 
			get { return filename; }
			set {
				filename = value;
				if (FolderName == null || FolderName == "") {
					FolderName = System.IO.Directory.GetParent(value).ToString();
				}
			}
		}
		
		[Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
		[Description("运行所在的文件夹")]
		[Category("主要设置")]
		[DisplayName("运行路径")]
		public String FolderName { get; set;  }
		
		
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		[Description("编译器的路径")]
		[Category("编译选项")]
		[DisplayName("编译器")]
		public String Compiler { get; set; }
		
		
		
		[Description("编译选项")]
		[Category("编译选项")]
		[ReadOnly(true)]
		[DisplayName("编译选项")]
		public String CompilerOptions { get; set; }
		
		[Description("附加选项(如-lgdi -lopengl32)")]
		[Category("编译选项")]
		[DisplayName("附加选项")]
		public String CompilerOptionsAdd { get; set; }
		
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		[Description("输出文件名")]
		[Category("编译选项")]
		[DisplayName("输出文件名")]
		public String OutputName { get; set; }
		
		[Description("命令行参数")]
		[Category("运行选项")]
		[DisplayName("命令行参数")]
		public String Arguments { get; set; }
		
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		[Description("运行器")]
		[Category("运行选项")]
		[DisplayName("运行器")]
		public String Runner { get; set; }
		
		/*
		[Description("环境变量")]
		[Category("运行选项")]
		[DisplayName("环境变量")]
		public Dictionary<String, String> RunEnvironment { get; set;  }
		*/
		
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		[Description("用这个编辑器打开文件编辑")]
		[Category("编辑器")]
		[DisplayName("编辑器")]
		public String EditorFileName { get; set;  }
		
		
		
		public MainEdit()
		{
			this.CompilerOptions = "-Wall -Werror -Wextra -pedantic -Wimplicit-fallthrough -Wsequence-point -Wswitch-default -Wswitch-unreachable -Wswitch-enum -Wstringop-truncation -Wbool-compare -Wtautological-compare -Wfloat-equal -Wshadow=global -Wpointer-arith -Wpointer-compare -Wcast-align -Wcast-qual -Wwrite-strings -Wdangling-else -Wlogical-op -Wconversion -Wno-pedantic-ms-format";
			this.Runner = execPath("ConsolePauser.exe");
			this.Compiler = "gcc.exe";
			this.EditorFileName = "write";
			/*
			this.RunEnvironment = new Dictionary<string, string>();
			this.RunEnvironment["PATH"] = System.Environment.GetEnvironmentVariable("PATH");
			*/
		}
		
		public static String execPath(String path) {
			 return System.IO.Directory.GetParent(Application.ExecutablePath).ToString() + "\\" + path;
		}
		
				
		public void saveTo(String path) {
			var writer = new System.Xml.Serialization.XmlSerializer(typeof(MainEdit));  
            var file = System.IO.File.Create(path);  
            writer.Serialize(file, this);  
            file.Close();  	
		}
		
		public static MainEdit readFrom(String path) {
			var reader = new System.Xml.Serialization.XmlSerializer(typeof(MainEdit));  
			var file = new System.IO.StreamReader(path);  
			MainEdit me = reader.Deserialize(file) as MainEdit;
    		file.Close();  
    		return me;
		}
		
		public MainEdit from(String path) {
			return MainEdit.readFrom(path);
		}
		
		public void edit() {
			var me = this;
			Process p = new Process();
			p.StartInfo.UseShellExecute = true;
			p.StartInfo.FileName  = me.EditorFileName;
			p.StartInfo.Arguments = me.FileName;
			p.Start();
		}
		
		public bool compile() {
			var me = this;
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
			var me = this;
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
		
		public void crun() {
			if (compile()) run();
		}
		
		public void openFolder() {
			var me = this;
			String s = System.IO.Directory.GetParent(me.FileName).ToString();
			Process.Start("explorer.exe", s);
		}
		
		public List<Tuple<String, String>> getCommands() {
			return new List<Tuple<String, String>> {
				new Tuple<String, String>("编辑", "edit"),
				new Tuple<String, String>("编译运行", "crun"),
				new Tuple<String, String>("打开文件夹", "openFolder")
			};
		}
		
		public void invoke(String name) {
			this.GetType().GetMethod(name).Invoke(this, new object[0]);
		}
	}
