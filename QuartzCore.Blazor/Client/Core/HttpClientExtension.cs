using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace QuartzCore.Blazor.Client.Core
{
    public static class HttpClientExtension
    {
        #region 标准的Http请求扩展

        public static async Task<TRsp> PostFromJsonAsync<TReq, TRsp>(this HttpClient client, string requestUri, TReq value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        {
            var httpResponse = await client.PostAsJsonAsync<TReq>(requestUri, value, options, cancellationToken);
            TRsp rsp = await httpResponse.Content.ReadFromJsonAsync<TRsp>();
            return rsp;

        }

        public static async Task<TRsp> PutFromJsonAsync<TReq, TRsp>(this HttpClient client, string requestUri, TReq value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        {
            var httpResponse = await client.PutAsJsonAsync<TReq>(requestUri, value, options, cancellationToken);
            TRsp rsp = await httpResponse.Content.ReadFromJsonAsync<TRsp>();
            return rsp;
        }

        public static async Task<TRsp> DeleteFromJsonAsync<TRsp>(this HttpClient client, string requestUri, CancellationToken cancellationToken = default)
        {
            var httpResponse = await client.DeleteAsync(requestUri, cancellationToken);
            TRsp rsp = await httpResponse.Content.ReadFromJsonAsync<TRsp>();
            return rsp;
        }

        #endregion
    }
}
