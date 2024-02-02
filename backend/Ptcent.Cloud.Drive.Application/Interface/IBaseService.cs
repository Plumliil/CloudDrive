using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ptcent.Cloud.Drive.WebApi.Application.Interface
{
    public interface IBaseService<T>
    {
        ///// <summary>
        ///// 根据配置名称获取配置内容
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="platformFlag"></param>
        ///// <returns></returns>
        //string GetAppSetting(string key, PlatformFlagEnum platformFlag = PlatformFlagEnum.KK);
        /// <summary>
        /// 单个新增
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        Task<bool> Add(T model);

        /// <summary>
        /// 多个新增
        /// </summary>
        /// <param name="list">实体集合</param>
        /// <returns></returns>
        Task<int> AddBatch(List<T> list);
        /// <summary>
        /// 单个更新
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        Task<bool> Update(T model);
        /// <summary>
        /// 多个更新
        /// 根据条件更新指定字段
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<int> UpdateBatch(Expression<Func<T, bool>> whereLambda, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> expression);
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="isSaveChange"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(List<T> entitys, bool isSaveChange);
        /// <summary>
        /// 单个删除
        /// 物理删除慎用
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> Delete(T model);
        /// <summary>
        /// 多个删除
        /// 物理删除慎用
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        Task<int> DeleteBatch(Expression<Func<T, bool>> whereLambda);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="isSaveChange"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(List<T> entitys, bool isSaveChange);
        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        Task<T> GetById(params object[] keyValues);
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        Task<IQueryable<T>> GetList();
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        Task<IQueryable<T>> GetList(Expression<Func<T, bool>> whereLambda);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="total">总条数</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderBys">排序字段，支持多个（格式：字段名字（区分大小写）, 是否降序）</param>
        /// <returns></returns>
        IQueryable<T> GetPaging(int pageIndex, int pageSize, out int total, Expression<Func<T, bool>> whereLambda, Dictionary<string, bool> orderBys);

        Task<K> HttpClientPost<K>(IHttpClientFactory clientFactory, string url, object objPara);
        Task<Stream> HttpClienGet<K>(IHttpClientFactory clientFactory, string url);
        Task<T> HttpClientGetAsync<T>(IHttpClientFactory clientFactory, string url);
        /// <summary>
        /// 返回第一条记录
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> @where);
        /// <summary>
        /// 去重查询
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        Task<List<T>> Distinct(Expression<Func<T, bool>> @where);
        /// <summary>
        /// 是否满足条件
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        Task<bool> Any(Expression<Func<T, bool>> @where);
        /// <summary>
        /// 返回总条数 - 通过条件过滤
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<T, bool>> @where);
        /// <summary>
        /// 条件查询 (异步)
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        Task<IQueryable<T>> WhereAsync(Expression<Func<T, bool>> @where);
    }
}
