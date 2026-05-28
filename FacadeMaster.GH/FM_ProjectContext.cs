using FacadeMaster.Core;
using FacadeMaster.Core.Models;
using Rhino.PlugIns;
using System;

namespace FacadeMaster.GH
{
    /// <summary>
    /// .gha 侧访问 Rhino 插件（.rhp）里那个唯一 ProjectManager 的入口。
    /// <para>
    /// ProjectManager 由 <c>RhinoPlugin.OnLoad</c> 通过 DI 创建，是进程级共享状态，
    /// GH 组件不要自己 new，也不要自己建 DI 容器，一律从这里取。
    /// </para>
    /// <para>
    /// 取值顺序：先用 <see cref="PlugIn.Find(Guid)"/> 从 Rhino 的插件表拿实例
    /// （不依赖静态字段，插件被卸载重载后依然有效），失败再退回插件类的静态 Instance。
    /// </para>
    /// <para>
    /// 用法（在 SolveInstance 里读，不要缓存到字段长期持有）：
    /// <code>
    /// var project = FM_ProjectContext.ActiveProject;      // EngProject，可能是 null
    /// if (project == null) { AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "未找到当前项目"); return; }
    /// DA.SetData(0, project.ProjectName);
    /// </code>
    /// </para>
    /// </summary>
    public static class FM_ProjectContext
    {
        /// <summary>
        /// 插件 GUID，必须与 FacadeMaster.RH\Properties\AssemblyInfo.cs 里的
        /// [assembly: Guid("385a325d-a747-472a-b267-c4e42ac78061")] 保持一致。
        /// </summary>
        public static readonly Guid PluginGuid = new Guid("385a325d-a747-472a-b267-c4e42ac78061");

        /// <summary>
        /// 共享的 ProjectManager。插件未加载（例如 GH 独立运行）或初始化失败时为 null。
        /// </summary>
        public static ProjectManager Manager
        {
            get
            {
                try
                {
                    var plugin = PlugIn.Find(PluginGuid) as FacadeMaster.RH.RhinoPlugin;
                    if (plugin?.ProjectManager != null) return plugin.ProjectManager;

                    return FacadeMaster.RH.RhinoPlugin.Instance?.ProjectManager;
                }
                catch (Exception)
                {
                    // 插件尚未加载 / 类型解析失败都不该把 GH 求解带崩，按“取不到”处理
                    return null;
                }
            }
        }

        /// <summary>Rhino 插件是否已加载，可用于给用户更准确的提示信息。</summary>
        public static bool IsAvailable => Manager != null;

        /// <summary>当前激活项目（ProjectManager.ActiveProject），无项目时为 null。</summary>
        public static EngProject ActiveProject => Manager?.ActiveProject;

        /// <summary>
        /// 当前项目切换事件。注意：静态事件只是转发到 ProjectManager 的事件，
        /// 必须在 <c>AddedToDocument / RemovedFromDocument</c> 里成对订阅/退订，避免组件被删后仍被回调。
        /// </summary>
        public static event EventHandler ActiveProjectChanged
        {
            add { var m = Manager; if (m != null) m.ActiveProjectChanged += value; }
            remove { var m = Manager; if (m != null) m.ActiveProjectChanged -= value; }
        }

        /// <summary>当前项目内部数据（批次/物料）变更事件，用法同 <see cref="ActiveProjectChanged"/>。</summary>
        public static event EventHandler ProjectContentChanged
        {
            add { var m = Manager; if (m != null) m.ProjectContentChanged += value; }
            remove { var m = Manager; if (m != null) m.ProjectContentChanged -= value; }
        }

        /// <summary>读取当前项目，取不到返回 false（不抛异常）。</summary>
        public static bool TryGetActiveProject(out EngProject project)
        {
            project = ActiveProject;
            return project != null;
        }
    }
}
