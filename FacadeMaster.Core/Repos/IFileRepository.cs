namespace FacadeMaster.Core.Repos
{
    /// <summary>
    /// 以本地 JSON 文件持久化的仓储。实现该接口的仓储支持在运行期切换文件位置
    /// （用户在设置里改了项目库 / 物料库路径后调用）。
    /// </summary>
    public interface IFileRepository
    {
        /// <summary>当前持久化文件全路径</summary>
        string FilePath { get; }

        /// <summary>
        /// 切换持久化文件。
        /// <para>
        /// 目标文件不存在且 <paramref name="keepDataIfTargetMissing"/> 为 true 时，
        /// 会把当前内存数据写到新文件（相当于“另存为”），避免用户选了个新路径就发现数据空了；
        /// 目标文件已存在时则清空内存并从新文件重新加载。
        /// </para>
        /// </summary>
        void ChangeFilePath(string filePath, bool keepDataIfTargetMissing = true);
    }
}
