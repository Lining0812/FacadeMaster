using FacadeMaster.Core.Models;
using Grasshopper.Kernel;
using System;

namespace FacadeMaster.GH.A01Project
{
    /// <summary>
    /// 示例：在 .gha 里读取 ProjectManager 的 ActiveProject，并在项目切换时自动重算。
    /// 可以直接删掉本文件，不影响其他组件。
    /// </summary>
    public class GetActiveProject : GH_Component
    {
        public GetActiveProject()
            : base("当前项目", "ActiveProject",
                  "读取 Rhino 插件里 ProjectManager 的当前激活项目",
                  "FacadeMaster", "A01项目")
        {
            Message = "当前项目";
        }

        public override Guid ComponentGuid => new Guid("C7E86036-D5FA-47F4-A137-3E414B863082");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("项目名称", "名称", "EngProject.ProjectName", GH_ParamAccess.item);
            pManager.AddTextParameter("项目编号", "编号", "EngProject.ProjectCode", GH_ParamAccess.item);
            pManager.AddTextParameter("项目Id", "Id", "EngProject.Id", GH_ParamAccess.item);
            pManager.AddGenericParameter("项目对象", "项目", "EngProject 对象，可接到其它组件", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var project = FM_ProjectContext.ActiveProject;

            if (project == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning,
                    FM_ProjectContext.IsAvailable
                        ? "当前没有激活的项目，请在 FacadeMaster 面板里选择或新建项目。"
                        : "未取得 ProjectManager：请确认 FacadeMaster.RH 插件已在 Rhino 中加载。");
                return;
            }

            DA.SetData(0, project.ProjectName);
            DA.SetData(1, project.ProjectCode);
            DA.SetData(2, project.Id.ToString());
            DA.SetData(3, project);
        }

        // 项目切换 / 项目内容变更 -> 让本组件重新求解，否则 GH 不会自动刷新
        // 注意：GH 里这两个钩子是 public virtual，重写时不能降成 protected
        public override void AddedToDocument(GH_Document document)
        {
            base.AddedToDocument(document);
            FM_ProjectContext.ActiveProjectChanged += OnProjectChanged;
            FM_ProjectContext.ProjectContentChanged += OnProjectChanged;
        }

        public override void RemovedFromDocument(GH_Document document)
        {
            FM_ProjectContext.ActiveProjectChanged -= OnProjectChanged;
            FM_ProjectContext.ProjectContentChanged -= OnProjectChanged;
            base.RemovedFromDocument(document);
        }

        private void OnProjectChanged(object sender, EventArgs e)
        {
            // 注意：GH_Document 上只有无参 ExpireSolution()，带参的这个在组件自己身上；
            // 事件可能来自非 UI 线程（例如插件内的 HTTP 服务），统一切到主线程再重算
            Rhino.RhinoApp.InvokeOnUiThread(new Action(() => ExpireSolution(true)));
        }
    }
}
