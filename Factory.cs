/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2020/9/10
 * 时间: 0:50
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Windows.Forms;

namespace kousa
{
	/// <summary>
	/// Description of Factory.
	/// </summary>
	public class Factory
	{
		public Factory()
		{
			
		}
		public static dynamic createObject(String path, String classname) {
			var objCSharpCodePrivoder = new CSharpCodeProvider();
            var objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();
            var objCompilerParameters = new CompilerParameters();

            objCompilerParameters.ReferencedAssemblies.Add("System.dll");
            objCompilerParameters.ReferencedAssemblies.Add("System.Design.dll");
            objCompilerParameters.ReferencedAssemblies.Add("System.Drawing.dll");
            objCompilerParameters.ReferencedAssemblies.Add("System.Drawing.Design.dll");
            objCompilerParameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            objCompilerParameters.ReferencedAssemblies.Add("System.XML.dll");
            
            objCompilerParameters.GenerateExecutable = false;
            objCompilerParameters.GenerateInMemory = true;
            objCompilerParameters.CompilerOptions += " /nologo";
            String text = System.IO.File.ReadAllText(path);
            var cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, text);
         	if (cr.Errors.HasErrors || cr.Errors.HasWarnings) {
            	String s = "";
            	for (int i = 0; i < cr.Output.Count; ++i) {
            		s += "[" + cr.Output[i] + "]\n";
            	}
            	MessageBox.Show(s, "编译错误");
                return null;
            }
            else {
                var objAssembly = cr.CompiledAssembly;
                return objAssembly.CreateInstance(classname);
            }   
		}
	}
}
