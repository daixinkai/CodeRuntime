using CodeRuntime.Common;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CodeRuntime
{
    /// <summary>
    /// 方法构建
    /// </summary>
    public sealed class MethodConstruct
    {
        internal MethodConstruct() { }
        /// <summary>
        /// 解析字符串构造函数
        /// </summary>
        /// <param name="items">待解析字符串数组</param>
        private object ConstructEvaluator(string name, string code, Type returnType, IEnumerable<string> reference = null)
        {
            code = code.Trim();
            if (!code.EndsWith(";") && !string.IsNullOrEmpty(code))
            {
                code += ";";
            }
            //创建C#编译器实例
            ICodeCompiler comp = new CSharpCodeProvider().CreateCompiler();
            //编译器的传入参数
            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("system.dll");                //添加程序集 system.dll 的引用
            cp.ReferencedAssemblies.Add("system.data.dll");            //添加程序集 system.data.dll 的引用
            cp.ReferencedAssemblies.Add("system.xml.dll");            //添加程序集 system.xml.dll 的引用            
            //添加程序集
            foreach (var item in Tools.GetCurrentAssemblys())
            {
                cp.ReferencedAssemblies.Add(item.Location);
            }
            if (reference != null && reference.Count() > 0)
            {
                foreach (var item in reference)
                {
                    var temp = item.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ? item : item.TrimEnd('.') + ".dll";
                    if (!cp.ReferencedAssemblies.Contains(temp))
                    {
                        cp.ReferencedAssemblies.Add(temp);
                    }
                }
            }
            cp.GenerateExecutable = false;                            //不生成可执行文件
            cp.GenerateInMemory = true;                    //在内存中运行
            StringBuilder builder = new StringBuilder();                //创建代码串
            /*
             *  添加常见且必须的引用字符串
             */

            builder.Append("using System; \n");
            builder.Append("using System.Data; \n");
            builder.Append("using System.Xml; \n");
            builder.Append("namespace Dym_Method_Namespace_ \n{ \n");                    //生成代码的命名空间为EvalGuy，和本代码一样
            builder.Append("  public class Dym_Method_Class_ \n{ \n");            //产生 _Evaluator 类，所有可执行代码均在此类中运行
            builder.AppendFormat("    public static {0} {1}() ",            //添加定义公共函数代码
                returnType == null ? "void" : returnType.Name,                //函数返回值为可执行字符串项中定义的返回值类型
                              name);                        //函数名称为可执行字符串项中定义的执行字符串名称
            builder.Append("\n{ ");                                    //添加函数开始括号
            builder.AppendFormat("{0}", code);//添加函数体，返回可执行字符串项中定义的表达式的值
            builder.Append("\n}");                                    //添加函数结束括号
            builder.Append("\n}\n }");                                    //添加类结束和命名空间结束括号
            //得到编译器实例的返回结果
            CompilerResults cr = comp.CompileAssemblyFromSource(cp, builder.ToString());
            if (cr.Errors.HasErrors)                            //如果有错误
            {
                StringBuilder error = new StringBuilder();            //创建错误信息字符串
                error.Append("编译有错误的表达式: ");                //添加错误文本
                foreach (CompilerError err in cr.Errors)            //遍历每一个出现的编译错误
                {
                    error.AppendFormat("{0}\n", err.ErrorText);        //添加进错误文本，每个错误后换行
                }
                throw new Exception("编译错误: " + error.ToString());//抛出异常
            }
            Assembly a = cr.CompiledAssembly;                        //获取编译器实例的程序集            
            return a.CreateInstance("Dym_Method_Namespace_.Dym_Method_Class_");        //通过程序集查找并声明 EvalGuy._Evaluator 的实例
        }

        internal MethodInfo Generator(string code, string[] reference = null)
        {
            string name = "M_" + DateTime.Now.Ticks;
            var obj = ConstructEvaluator(name, code, null, reference);
            var method = obj.GetType().GetMethod(name);
            return method;
            //return method.CreateDelegate(typeof(Action));
        }

        internal MethodInfo Generator(string code, Type returnType, string[] reference = null)
        {
            string name = "M_" + DateTime.Now.Ticks;
            var obj = ConstructEvaluator(name, code, returnType, reference);
            var method = obj.GetType().GetMethod(name);
            return method;
            //var type = typeof(Func<>);
            //type.MakeGenericType(returnType);
            //return method.CreateDelegate(type);
        }

        public static Action CreateMethod(string code, params string[] reference)
        {
            CheckCode(code);
            MethodConstruct methodC = new MethodConstruct();
            var method = methodC.Generator(code, reference).CreateDelegate(typeof(Action)) as Action;
            return method;
        }

        public static Func<TResult> CreateMethod<TResult>(string code, params string[] reference)
        {
            CheckCode(code);
            MethodConstruct methodC = new MethodConstruct();
            var method = methodC.Generator(code, reference).CreateDelegate(typeof(Func<TResult>)) as Func<TResult>;
            return method;
        }


        static void CheckCode(string code)
        {

        }

    }
}
