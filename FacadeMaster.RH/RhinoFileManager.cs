using System;
using System.IO;
using Rhino;
using Rhino.Commands;

namespace FacadeMaster.RH
{
    /// <summary>
    /// Rhino文件管理器 - 用于打开和关闭Rhino文件
    /// </summary>
    public static class RhinoFileManager
    {
        /// <summary>
        /// 打开Rhino文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>打开的Rhino文档，失败返回null</returns>
        public static RhinoDoc OpenRhinoFile(string filePath)
        {
            try
            {
                // 检查文件是否存在
                if (!File.Exists(filePath))
                {
                    RhinoApp.WriteLine("文件不存在: {0}", filePath);
                    return null;
                }

                // 验证文件扩展名
                if (!filePath.EndsWith(".3dm", StringComparison.OrdinalIgnoreCase))
                {
                    RhinoApp.WriteLine("无效的文件格式。需要 .3dm 文件。");
                    return null;
                }

                // 打开Rhino文件
                RhinoDoc doc = RhinoDoc.Open(filePath, out bool wasAlreadyOpen);

                if (doc != null)
                {
                    if (wasAlreadyOpen)
                    {
                        RhinoApp.WriteLine("文件已在另一个窗口中打开: {0}", Path.GetFileName(filePath));
                    }
                    else
                    {
                        RhinoApp.WriteLine("成功打开文件: {0}", Path.GetFileName(filePath));
                    }
                }

                return doc;
            }
            catch (Exception ex)
            {
                RhinoApp.WriteLine("打开文件时出错: {0}", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 打开桌面的未命名.3dm文件
        /// </summary>
        /// <returns>打开的Rhino文档，失败返回null</returns>
        public static RhinoDoc OpenDesktopUnnamedFile()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "未命名.3dm");
            return OpenRhinoFile(filePath);
        }

        /// <summary>
        /// 关闭当前Rhino文件
        /// </summary>
        /// <param name="saveChanges">是否保存更改</param>
        /// <returns>是否成功关闭</returns>
        public static bool CloseCurrentRhinoFile(bool saveChanges = false)
        {
            RhinoDoc doc = RhinoDoc.ActiveDoc;

            if (doc == null)
            {
                RhinoApp.WriteLine("没有活动的Rhino文档。");
                return false;
            }

            // 检查文档是否被修改 - 使用RhinoApp.RunScript调用Close命令
            if (!saveChanges)
            {
                RhinoApp.WriteLine("正在关闭文档（不保存更改）...");
            }
            else
            {
                RhinoApp.WriteLine("正在关闭文档（保存更改）...");
            }

            try
            {
                // 使用Rhino命令来关闭文档
                string closeCommand = saveChanges ? "_Save _Close" : "_Close _No";
                Result result = RhinoApp.RunScript(closeCommand, false) ? Result.Success : Result.Failure;
                
                if (result == Result.Success)
                {
                    RhinoApp.WriteLine("文档已关闭。");
                    return true;
                }
                else
                {
                    RhinoApp.WriteLine("关闭文档失败。");
                    return false;
                }
            }
            catch (Exception ex)
            {
                RhinoApp.WriteLine("关闭文件时出错: {0}", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取当前活动的Rhino文档
        /// </summary>
        /// <returns>当前活动的Rhino文档，如果没有则返回null</returns>
        public static RhinoDoc GetActiveDocument()
        {
            return RhinoDoc.ActiveDoc;
        }

        /// <summary>
        /// 检查当前是否有活动的Rhino文档
        /// </summary>
        /// <returns>是否有活动文档</returns>
        public static bool HasActiveDocument()
        {
            return RhinoDoc.ActiveDoc != null;
        }
    }
}
