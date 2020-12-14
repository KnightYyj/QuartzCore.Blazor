using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QuartzCore.Blazor.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzCore.Blazor.Server.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ExceptionHandlingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            DateTime beginTime = DateTime.Now;
            Exception exception = null;
            try
            {
                var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, ex.Message);
                exception = ex;
            }
            finally
            {
                DateTime endTime = DateTime.Now;
                TimeSpan ts = endTime - beginTime;
                if (exception != null)
                {
                    //发生异常的时候，执行异常处理
                    await HandleExceptionAsync(context, exception, (long)ts.TotalMilliseconds);
                }
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex, long times)
        {
            var uuidN = Guid.NewGuid().ToString("N");  //错误唯一ID，用于排查问题
            MessageModel<string> res = new MessageModel<string>();
            //拼装服务器信息
            var connInfo = context.Connection;
            if (ex != null)
            {
                res.msg = $"{ex.Message}{ex.StackTrace}[errorid={uuidN}]";
            }
            else
            {
                res.msg = $"未知异常，请联系管理员! {ex.Message}[errorid={uuidN}]";
            }
            var result = JsonConvert.SerializeObject(res);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;//这里的http status=200前端会认为接口是通的
            return context.Response.WriteAsync(result);
        }
    }

    public static class ErrorHandlingExtensions
    {
        /// <summary>
        /// 异常中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>

        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
