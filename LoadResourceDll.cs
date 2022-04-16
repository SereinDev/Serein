
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;

namespace LegendTool
{
    public static class LoadResoureDll
    {
        /// <summary> 已加载DLL
        /// </summary>
        private static Dictionary<string, Assembly> LoadedDlls = new Dictionary<string, Assembly>();
        /// <summary> 已处理程序集
        /// </summary>
        private static Dictionary<string, object> Assemblies = new Dictionary<string, object>();
        /// <summary> 在对程序集解释失败时触发
        /// </summary>
        /// <param name="sender">AppDomain</param>
        /// <param name="args">事件参数</param>
        private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                //程序集
                Assembly ass;
                //获取加载失败的程序集的全名
                var assName = new AssemblyName(args.Name).FullName;
                //判断Dlls集合中是否有已加载的同名程序集
                if (LoadedDlls.TryGetValue(assName, out ass) && ass != null)
                {
                    LoadedDlls[assName] = null;//如果有则置空并返回
                    return ass;
                }
                else
                {
                    throw new DllNotFoundException(assName);//否则抛出加载失败的异常
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("error:\n位置：AssemblyResolve()！\n描述：" + ex.Message);
                return null;
            }
        }

        /// <summary> 注册资源中的dll
        /// </summary>
        /// <param name="pattern">*表示连续的未知字符,_表示单个未知字符,如*.dll</param>
        public static void RegistDLL(string pattern = "*.dll")
        {
            System.IO.Directory.GetFiles("", "");
            //获取调用者的程序集
            var ass = new StackTrace(0).GetFrame(1).GetMethod().Module.Assembly;
            //判断程序集是否已经处理
            if (Assemblies.ContainsKey(ass.FullName))
            {
                return;
            }
            //程序集加入已处理集合
            Assemblies.Add(ass.FullName, null);
            //绑定程序集加载失败事件(这里我测试了,就算重复绑也是没关系的)
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
            //获取所有资源文件文件名
            var res = ass.GetManifestResourceNames();
            var regex = new Regex("^" + pattern.Replace(".", "\\.").Replace("*", ".*").Replace("_", ".") + "$", RegexOptions.IgnoreCase);
            foreach (var r in res)
            {
                //如果是dll,则加载
                if (regex.IsMatch(r))
                {
                    try
                    {
                        var s = ass.GetManifestResourceStream(r);
                        var bts = new byte[s.Length];
                        s.Read(bts, 0, (int)s.Length);
                        var da = Assembly.Load(bts);
                        //判断是否已经加载
                        if (LoadedDlls.ContainsKey(da.FullName))
                        {
                            continue;
                        }
                        LoadedDlls[da.FullName] = da;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("error:加载dll失败\n位置：RegistDLL()！\n描述：" + ex.Message);
                    }
                }
            }
        }
    }
}
