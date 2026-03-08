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
        public RhinoPlugin()
        {
            Instance = this;
        }

        ///<summary>Gets the only instance of the RhinoPlugin plug-in.</summary>
        public static RhinoPlugin Instance { get; private set; }

        public IServiceProvider ServiceProvider { get; private set; }

        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and maintain plug-in wide options in a document.
        protected override LoadReturnCode OnLoad(ref string errorMessage)
        {
            // 初始化依赖注入容器
            ServiceProvider = new ServiceCollection().AddFMServices().BuildServiceProvider();

            // 注册插件面板
            string pluginDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string iconPath = Path.Combine(pluginDir, "EmbeddedResources", "plugin-utility.ico");
            Icon icon = new Icon(iconPath);
            Panels.RegisterPanel(this,typeof(FM_Panel), "FacadeMaster面板", icon);

            return base.OnLoad(ref errorMessage);
        }

        public T GetService<T>()
        {
            return ServiceProvider.GetService<T>();
        }
    }
}