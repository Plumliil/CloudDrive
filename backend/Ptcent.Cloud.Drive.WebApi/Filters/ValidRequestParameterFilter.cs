using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Common;
using Ptcent.Cloud.Drive.WebApi.Infrastructure;

namespace Ptcent.Cloud.Drive.WebApi.Filters
{
    /// <summary>
    /// 参数验证
    /// </summary>
    public class ValidRequestParameterFilter : ActionFilterAttribute
    {
        //private readonly ILogger<FineexApiOperationFilter> logger;

        //private readonly IUserService services;
        //private WxLogin _WxUser;
        //private ILogService _LogService;
        public ValidRequestParameterFilter() { }
        //public ValidRequestParameterFilter(IUserService UserServicecs)//,ILogger<FineexApiOperationFilter> logger, WxLogin WxUser, ILogService LogService, IOptions<Config> config)
        //{
        //    //logger = logger;
        //    //services = UserServicecs;
        //    //_WxUser = WxUser;
        //    //_LogService = LogService;
        //    //_config = config.Value;
        //}
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                try
                {
                    var res = new ResponseMessageDto<string>();
                    IEnumerable<string> errorMessages = from vals in actionContext.ModelState.Values
                                                        from error in vals.Errors
                                                        select error.ErrorMessage;
                    res.IsSuccess = false;
                    res.Message = string.Join(";", errorMessages.Where(n => !string.IsNullOrWhiteSpace(n)));
                    res.Message = string.IsNullOrWhiteSpace(res.Message) || res.Message == ";" || !errorMessages.Any() ? "请检查所传参数是否符合要求" : res.Message;
                    actionContext.Result = new ObjectResult(res);
                }
                catch (Exception ex)
                {
                    LogUtil.Error($"参数验证出错了{ex}");
                }
            }
            if (actionContext.ActionArguments.Count > 0)
            {
                try
                {
                    foreach (var argument in actionContext.ActionArguments)
                    {
                        if (argument.Value == null)
                        {
                            var res = new ResponseMessageDto<string>
                            {
                                IsSuccess = false,
                                Message = "请检查参数"
                            };
                            actionContext.Result = new ObjectResult(res);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error($"参数验证出错了{ex}");
                }
            }
            base.OnActionExecuting(actionContext);
        }
    }
}
