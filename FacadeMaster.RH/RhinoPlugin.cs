using FacadeMaster.Core;
using FacadeMaster.RH.FM_Panels;
using Microsoft.Extensions.DependencyInjection;
using Rhino.PlugIns;
using Rhino.UI;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace FacadeMaster.RH
{
    ///<summary>
    /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.</para>
    /// <para>To complete plug-in information, please also see all PlugInDescription
    /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
    /// "Show All Files" to see it in the "Solution Explorer" window).</para>
    ///</summary>
    public class RhinoPlugin : Rhino.PlugIns.PlugIn
    {
        private RhinoHttpServer _server;
        public RhinoPlugin()
        {
            Instance = this;
        }

        ///<summary>Gets the only instance of the RhinoPlugin plug-in.</summary>
        public static RhinoPlugin Instance { get; private set; }

        public IServiceProvider ServiceProvider { get; private set; }
        public ProjectManager ProjectManager { get; private set; }

        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and maintain plug-in wide options in a document.
        protected override LoadReturnCode OnLoad(ref string errorMessage)
        {
            try
            {
                // 初始化依赖注入容器
                ServiceProvider = new ServiceCollection().AddFMServices().BuildServiceProvider();
                ProjectManager = ServiceProvider.GetRequiredService<ProjectManager>();

                // 注册插件面板
                string pluginDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string iconPath = Path.Combine(pluginDir, "EmbeddedResources", "plugin-utility.ico");
                Icon icon = new Icon(iconPath);
                Panels.RegisterPanel(this, typeof(FM_Panel), "FacadeMaster面板", icon);

                // 初始化物料管理器（挂接文档打开/保存事件，同步物料库备份）
                FM_MaterialManager.Initialize();

                // 启动HTTP服务器
                _server = new RhinoHttpServer();
                _server.Start();

                return base.OnLoad(ref errorMessage);
            }
            catch (Exception ex)
            {
                // Rhino 默认只提示“插件初始化失败”，把真实异常落到临时目录并弹窗，便于定位
                try
                {
                    string log = Path.Combine(Path.GetTempPath(), "FM_PluginLoad.log");
                    File.WriteAllText(log, DateTime.Now.ToString("O") + Environment.NewLine + ex);
                }
                catch { /* 写日志失败时忽略，不影响错误上报 */ }

                errorMessage = ex.ToString();
                return LoadReturnCode.ErrorShowDialog;
            }
        }

        override protected void OnShutdown()
        {
            _server?.Dispose();
            base.OnShutdown();
        }

        public override PlugInLoadTime LoadTime => PlugInLoadTime.AtStartup;

        public T GetService<T>()
        {
            return ServiceProvider.GetService<T>();
        }

    }
}