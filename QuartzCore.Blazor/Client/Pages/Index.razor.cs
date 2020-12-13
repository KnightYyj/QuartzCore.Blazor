using AntDesign;
using AntDesign.Charts;
using Microsoft.AspNetCore.Components;
using QuartzCore.Blazor.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace QuartzCore.Blazor.Client.Pages
{
    public partial class Index //:  DrawerTemplate<int?, MessageModel<DashDto>>
    {
        DashDto dashDto = new DashDto();

        [Inject] public HttpClient Http { get; set; }

        [Inject] public MessageService MsgSvr { get; set; }

        IChartComponent chart1;
        ChartData[] data1;

        protected override async Task OnInitializedAsync()
        {
            await OnQuery();
            await base.OnInitializedAsync();
        }

        public async Task OnQuery()
        {
            //Console.WriteLine("OnQuery");
            var result = await Http.GetFromJsonAsync<MessageModel<DashDto>>($"api/tasksqz/dash");
            if (result.success)
            {
                dashDto = result.response;
                data1 = result.response.ChartDatas;
                await chart1.ChangeData(data1);
                MsgSvr.Success($"初始化成功");
            }
            else
                MsgSvr.Error($"初始化失败，错误:{result.msg}");
        }
 
        private async Task OnRefash()
        {
            //Console.WriteLine($"OnRefash");
            await OnQuery();
        }
        

    }
}
