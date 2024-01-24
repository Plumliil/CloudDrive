using Microsoft.EntityFrameworkCore.Query;
using Ptcent.Cloud.Drive.WebApi.Application.Interface;
using Ptcent.Cloud.Drive.WebApi.Infrastructure;
using Ptcent.Cloud.Drive.WebApi.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Ptcent.Cloud.Drive.WebApi.Application.Implement
{
    public class BaseServiceImpl<T> : IBaseService<T> where T : class, new()
    {

        private IBaseRepository<T> baseRepository { get; set; }
        public BaseServiceImpl(IBaseRepository<T> baseRepository)
        {
            //baseRepository = new BaseRepository<T>();
            this.baseRepository = baseRepository;
        }

        public async Task<bool> Add(T model)
        {
            return await baseRepository.Add(model);
        }

        public async Task<int> AddBatch(List<T> list)
        {
            return await baseRepository.AddBatch(list);
        }

        public async Task<bool> Update(T model)
        {

            return await baseRepository.Update(model);
        }

        //public async Task<int> UpdateBatch(Expression<Func<T, bool>> whereLambda, Expression<Func<T, T>> expression)

        public async Task<int> UpdateBatch(Expression<Func<T, bool>> whereLambda, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> expression)
        {
            return await baseRepository.UpdateBatch(whereLambda, expression);
        }
        public async Task<bool> UpdateAsync(List<T> entitys, bool isSaveChange)
        {
            return await baseRepository.UpdateAsync(entitys, isSaveChange);
        }
        public async Task<bool> Delete(T model)
        {
            return await baseRepository.Delete(model);
        }

        public async Task<int> DeleteBatch(Expression<Func<T, bool>> whereLambda)
        {
            return await baseRepository.DeleteBatch(whereLambda);
        }

        public async Task<bool> DeleteAsync(List<T> entitys, bool isSaveChange)
        {
            return await baseRepository.DeleteAsync(entitys, isSaveChange);
        }

        public async Task<T> GetById(params object[] keyValues)
        {
            return await baseRepository.GetById(keyValues);
        }

        public async Task<IQueryable<T>> GetList()
        {
            return await baseRepository.GetList();
        }

        public async Task<IQueryable<T>> GetList(Expression<Func<T, bool>> whereLambda)
        {
            return await baseRepository.GetList(whereLambda);
        }

        public IQueryable<T> GetPaging(int pageIndex, int pageSize, out int total, Expression<Func<T, bool>> whereLambda, Dictionary<string, bool> orderBys)
        {
            return baseRepository.GetPaging(pageIndex, pageSize, out total, whereLambda, orderBys);
        }
        public async Task<K> HttpClientPost<K>(IHttpClientFactory clientFactory, string url, object objPara)
        {
            string msgBody = string.Empty;
            try
            {
                var client = clientFactory.CreateClient();
                StringContent content = new StringContent(objPara.ToJson());
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                var source = HttpContextUtil.Current.Request.Headers["Source"].ToString();
                var authCode = HttpContextUtil.Current.Request.Headers["AuthToken"].ToString();
                content.Headers.Add("Source", source);
                content.Headers.Add("AuthToken", authCode);
                var respMsg = await client.PostAsync(url, content);//url中不能有//
                msgBody = await respMsg.Content.ReadAsStringAsync();
                return msgBody.ToObject<K>();
            }
            catch (Exception ex)
            {
                LogUtil.Error($"HttpClientPost错误{url} {ex} {objPara.ToJson()}返回结果{msgBody}");
                throw new Exception($"HttpClientPost错误{url} {objPara.ToJson()}");
            }
        }
        public async Task<K> HttpClientPostForTw<K>(IHttpClientFactory clientFactory, string url, object objPara, string token)
        {
            try
            {
                var client = clientFactory.CreateClient();
                StringContent content = new StringContent(objPara.ToJson());
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                //var source = HttpContextUtil.Current.Request.Headers["Source"].ToString();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                //token = "eyJ4NXQjUzI1NiI6ImlrWVlWczkzZmVrV3kxblI5SmpkT0xOWkg5VUgyYUZCRFkxbndHN0tjOXciLCJraWQiOiJ5c3QtY29uZmlnIiwiYWxnIjoiUlMyNTYifQ.eyJzdWIiOiJhZG1pbl96aCIsImF1ZCI6ImFkbWluX3poIiwiaXNzIjoiY2xvdWR0IiwieXN0X3VuIjoiYWRtaW5femgiLCJ5c3RfdWkiOjI4MTA2MzMwNDEwNTgyMjQwLCJ5c3RfcHQiOiJ1cyIsImlhdCI6MTY4OTAzODE1OCwieXN0X3RpIjoxfQ.CKbkno8VjEWwjE1DEwZRF9Ih8ohxTrvLicy3Av5xk6iXb5mjUxoS5MCqiZk985L2n9_YCJzepe2hW83xsOlzDGFAplQUSCI45mSgjSZpsyBQMGSnje33qgo2rFTstKlq7--gbctTu7ELPMtv7gXIOS0zVC1WXJNAoZCsxKGr9UjQH_hltv0ejKHQnGldbjeoE_NQWpX2QDw4rDA9BtoPQdlSJQNFggNpKorj6jiJ5fRn3nbtoJSJ1wZJzhxe7sRj8_Er6Ky8DOxyBMw9fv-" +
                //    "6YoDQ6antsxtuzOJdbCtFTiIgu0C-iWPv5e3vAbMU3FLwSQ21pUgGIQAuId-vyCWW6A";
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Content = content;
                var respMsg = await client.SendAsync(request);//url中不能有//
                string msgBody = await respMsg.Content.ReadAsStringAsync();
                return msgBody.ToObject<K>();
            }
            catch (Exception ex)
            {
                LogUtil.Error($"HttpClientPost错误{url} {objPara.ToJson()}");
                throw new Exception($"HttpClientPost错误{url} {objPara.ToJson()}");
            }
        }
        public async Task<K> HttpClientGetForTw<K>(IHttpClientFactory clientFactory, string url, object objPara, string token)
        {
            try
            {
                var client = clientFactory.CreateClient();

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var respMsg = await client.SendAsync(request);//url中不能有//
                string msgBody = await respMsg.Content.ReadAsStringAsync();
                return msgBody.ToObject<K>();
            }
            catch (Exception ex)
            {
                LogUtil.Error($"HttpClientGetForTw{url} {objPara.ToJson()}");
                throw new Exception($"HttpClientGetForTw{url} {objPara.ToJson()}");
            }
        }


        public async Task<K> HttpClientDeleteForTw<K>(IHttpClientFactory clientFactory, string url, object objPara, string token)
        {
            try
            {
                var client = clientFactory.CreateClient();

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var respMsg = await client.SendAsync(request);//url中不能有//
                string msgBody = await respMsg.Content.ReadAsStringAsync();
                return msgBody.ToObject<K>();
            }
            catch (Exception ex)
            {
                LogUtil.Error($"HttpClientGetForTw{url} {objPara.ToJson()}");
                throw new Exception($"HttpClientGetForTw{url} {objPara.ToJson()}");
            }
        }

        public async Task<Stream> HttpClienGet<K>(IHttpClientFactory clientFactory, string url)
        {
            var client = clientFactory.CreateClient();
            var fileStream = await client.GetStreamAsync(url);
            return fileStream;
        }
        public async Task<T> HttpClientGetAsync<T>(IHttpClientFactory clientFactory, string url)
        {
            var client = clientFactory.CreateClient();
            var respMsg = await client.GetAsync(url);//url中不能有//
            string msgBody = await respMsg.Content.ReadAsStringAsync();

            return msgBody.ToObject<T>();
        }
        /// <summary>
        /// 返回第一条记录
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> where)
        {
            return await baseRepository.FirstOrDefaultAsync(where);
        }
        /// <summary>
        /// 去重查询
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public async Task<List<T>> Distinct(Expression<Func<T, bool>> where)
        {
            return await baseRepository.Distinct(where);
        }
        /// <summary>
        /// 是否满足条件
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public async Task<bool> Any(Expression<Func<T, bool>> where)
        {
            return await baseRepository.Any(where);
        }
        /// <summary>
        /// 返回总条数 - 通过条件过滤
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<int> CountAsync(Expression<Func<T, bool>> where)
        {
            return await baseRepository.CountAsync(where);
        }
        /// <summary>
        /// 条件查询 (异步)
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public async Task<IQueryable<T>> WhereAsync(Expression<Func<T, bool>> @where)
        {
            return await baseRepository.WhereAsync(where);
        }
        public readonly static string FileRootPath = ConfigUtil.GetValue("FileRootPath");
        public readonly static string ShareUrl = ConfigUtil.GetValue("ShareUrl");
        public readonly static TimeSpan CacheTime = new TimeSpan(30, 0, 0, 0);
    }
}
