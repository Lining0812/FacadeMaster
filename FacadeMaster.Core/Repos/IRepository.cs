using FacadeMaster.Core.Models;
using System;
using System.Collections.Generic;

namespace FacadeMaster.Core.Repos
{
    /// <summary>
    /// 仓储接口 - 提供通用的数据访问操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        /// Id获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(Guid id);

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns></returns>
        ICollection<T> GetAll();

        /// <summary>
        /// 从文件读取数据
        /// </summary>
        void LoadFromFile();

        /// <summary>
        /// 保存修改方法
        /// </summary>
        void SaveChanges();
    }
}
