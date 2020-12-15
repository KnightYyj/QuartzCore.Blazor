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
using System.Timers;

namespace QuartzCore.Blazor.Client.Pages.Quartz.JobItem
{
    public partial class JobItemView
    {
        JobItemQueryDto queryDto = new JobItemQueryDto();
        [Inject] public HttpClient Http { get; set; }

        [Inject] public DrawerService DrawerSvr { get; set; }

        [Inject] public MessageService MsgSvr { get; set; }

        [Inject] public ConfirmService ConfirmSvr { get; set; }

        private bool isLoading = false;

        int total = 0;
        ITable tableRef;
        List<TasksQzDto> datas = new List<TasksQzDto>();

        List<AppInputDto> _appItems = new List<AppInputDto>();



        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine($"JobItemView---Starting");
            _appItems = await Http.GetFromJsonAsync<List<AppInputDto>>($"api/app");
            queryDto.Deleted = 0;//BoolStatus.All

            //var timer = new System.Threading.Timer(async (e) =>
            //{
            //    await OnQuery();
            //    await InvokeAsync(StateHasChanged);
            //    await Task.Delay(500);
            //    Console.WriteLine("Tick");
            //}, null, 0, 5000);
        }




        private async Task OnSearch()
        {
            Console.WriteLine($"OnSearch");
            queryDto.PageIndex = 1;
           
            await OnQuery();
        }

        private async Task OnChange(AntDesign.TableModels.QueryModel<TasksQzDto> queryModel)
        {
            queryDto.PageIndex = queryModel.PageIndex;
            queryDto.PageSize = queryModel.PageSize;
            await OnQuery();
        }

        private async Task OnQuery()
        {
            isLoading = true;
            var result = await Http.PostFromJsonAsync<JobItemQueryDto, MessageModel<List<TasksQzDto>>>("api/tasksqz/list", queryDto);
            if (result.response.Any())
            {
                result.response.ForEach(x =>
                {
                    x.AppName = _appItems.Find(a => a.Id == x.AppId)?.Name;
                });
            }
            datas = result.response.OrderBy(a => a.CreateTime).ToList();

            total = result.Total;

            isLoading = false;
        }

        public async Task OnAdd()
        {
            var result = await DrawerSvr.CreateDialogAsync<JobItemEdit, int?, MessageModel<TasksQzDto>>(null, title: "新增任务项", width: 750);
            if (result != null)
            {
                result.response.AppName = _appItems.Find(a => a.Id == result.response.AppId)?.Name;
                datas.Add(result.response);
            }
        }

        public async Task OnEdit(TasksQzDto model)
        {
            var result = await DrawerSvr.CreateDialogAsync<JobItemEdit, int?, MessageModel<TasksQzDto>>(model.Id, title: $"编辑指标 {model.Name}", width: 750);
            if (result != null)
            {
                var index = datas.IndexOf(model);
                result.response.AppName = _appItems.Find(a => a.Id == result.response.AppId)?.Name;
                datas[index] = result.response;
            }
        }


        public async Task OnStart(TasksQzDto model)
        {
            var result = await Http.GetFromJsonAsync<MessageModel<TasksQzDto>>($"api/TasksQz/start/{model.Id}");
            if (result.success)
            {
                var index = datas.IndexOf(model);
                result.response.AppName = _appItems.Find(a => a.Id == result.response.AppId)?.Name;
                datas[index] = result.response;
                MsgSvr.Success($"{model.Name} 启动成功");
            }
            else
                MsgSvr.Error($"{model.Name} 启动失败，错误:{result.msg}");
        }

        public async Task OnStop(TasksQzDto model)
        {
            if (await ConfirmSvr.Show($"是否停止任务 {model.Name}", "停止", ConfirmButtons.YesNo, ConfirmIcon.Question) == ConfirmResult.Yes)
            {
                var result = await Http.GetFromJsonAsync<MessageModel<TasksQzDto>>($"api/TasksQz/stop/{model.Id}");
                if (result.success)
                {
                    var index = datas.IndexOf(model);
                    result.response.AppName = _appItems.Find(a => a.Id == result.response.AppId)?.Name;
                    datas[index] = result.response;
                    MsgSvr.Success($"{model.Name} 停止成功");
                    //await InvokeAsync(StateHasChanged);
                }
                else
                    MsgSvr.Error($"{model.Name} 停止失败，{result.msg}");
            }
        }
    

        public async Task OnDelete(TasksQzDto model)
        {
            if (await ConfirmSvr.Show($"是否删除任务 {model.Name}", "停止", ConfirmButtons.YesNo, ConfirmIcon.Question) == ConfirmResult.Yes)
            {
                var result = await Http.DeleteFromJsonAsync<MessageModel<bool>>($"api/TasksQz/{model.Id}");
                if (result.success)
                {
                    await OnQuery();
                    MsgSvr.Success($"{model.Name} 删除成功");
                }
                else
                    MsgSvr.Error($"{model.Name} 删除失败，错误:{result.msg}");
            }
        }


        public void Dispose()
        {

        }
    }
}
