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
        /// 新增方法
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        void Delete(T entity);

        void Update(T entity);

        T GetById(Guid id);

        ICollection<T> GetAll();

        /// <summary>
        /// 保存修改方法
        /// </summary>
        void SaveChanges();
    }
}
