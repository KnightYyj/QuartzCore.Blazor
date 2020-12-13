using AntDesign;
using Microsoft.AspNetCore.Components;
using QuartzCore.Blazor.Client.Core;
using QuartzCore.Blazor.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace QuartzCore.Blazor.Client.Pages
{
    public partial class QzRunLogView
    {
        QzRunLogQueryDto queryDto = new QzRunLogQueryDto();
        [Inject] public HttpClient Http { get; set; }

        [Inject] public DrawerService DrawerSvr { get; set; }

        [Inject] public MessageService MsgSvr { get; set; }

        [Inject] public ConfirmService ConfirmSvr { get; set; }

        private bool isLoading = false;

        int total = 0;
        ITable tableRef;
        List<QzRunLogDto> datas = new List<QzRunLogDto>();

        List<AppInputDto> _appItems = new List<AppInputDto>();
        List<TasksQzDto> _qzItems = new List<TasksQzDto>();

        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine($"QzRunLogView---Starting");
            _appItems = await Http.GetFromJsonAsync<List<AppInputDto>>($"api/app");
            _qzItems = await Http.GetFromJsonAsync<List<TasksQzDto>>($"api/TasksQz");
            queryDto.PageIndex = 1;
            await OnQuery();
        }
        private async Task OnSearch()
        {
            Console.WriteLine($"OnSearch");
            queryDto.PageIndex = 1;
            await OnQuery();
        }
        private async Task OnChange(AntDesign.TableModels.QueryModel<QzRunLogDto> queryModel)
        {
            Console.WriteLine(queryModel.PageIndex);
            queryDto.PageIndex = queryModel.PageIndex;
            queryDto.PageSize = queryModel.PageSize;
            await OnQuery();
        }
        private async Task OnQuery()
        {
            Console.WriteLine($"OnSearch");
        
            isLoading = true;
            var result = await Http.PostFromJsonAsync<QzRunLogQueryDto,MessageModel<List<QzRunLogDto>>>("api/TasksQz/log/list", queryDto);
            if (result.response.Any())
            {
                result.response.ForEach(x =>
                {
                    x.AppName = _appItems.Find(a => a.Id == x.AppId)?.Name;
                    x.TasksQzName = _qzItems.Find(a => a.Id == x.TasksQzId)?.Name;
                });
            }
            datas = result.response.OrderByDescending(a => a.LogTime).ToList();
            total = result.Total;
            isLoading = false;
        }
    }
}
