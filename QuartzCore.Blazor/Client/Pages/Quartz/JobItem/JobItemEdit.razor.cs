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

namespace QuartzCore.Blazor.Client.Pages.Quartz.JobItem
{
    public partial class JobItemEdit : DrawerTemplate<int?, MessageModel<TasksQzDto>>
    {
        [Inject] public HttpClient Http { get; set; }

        [Inject] public MessageService MsgSvr { get; set; }

        TasksQzDto model;

        bool isCronValid = false;

        bool isLoading = false;

        private List<string> unitOptions = new() { "元", "万元", "件", "吨" };
        List<AppInputDto> _appItems = new List<AppInputDto>();
        bool isEdit = false;
        List<string> nextValidTime = new List<string>();
        string _nextTime = "Cron 输入即可查看";

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            Console.WriteLine(base.Options);
            _appItems = (await Http.GetFromJsonAsync<List<AppInputDto>>($"api/app")).Where(x => x.Enabled == true).ToList();
            if (base.Options.HasValue)
            {
                isEdit = true;
                model = (await Http.GetFromJsonAsync<TasksQzDto>($"api/TasksQz/{base.Options}"));
            }
            else
            {
                model = new TasksQzDto();
                model.IsApiUrl = true;
                model.IsCron = true;
                isEdit = false;
            }
            await base.OnInitializedAsync();
            isLoading = false;
        }

        async Task OnSave()
        {
            if (!isEdit)
            {
                var result = await Http.PostFromJsonAsync<TasksQzDto, MessageModel<TasksQzDto>>($" api/TasksQz", model);
                if (result.success == true)
                {
                    await base.CloseAsync(result);
                    MsgSvr.Success("新增成功");
                }
                else
                {
                    MsgSvr.Error($"新增失败:{result.msg}");
                }
            }
            else
            {
                var result = await Http.PutFromJsonAsync<TasksQzDto, MessageModel<TasksQzDto>>($"api/TasksQz/{model.Id}", model);
                if (result.success == true)
                {
                    await base.CloseAsync(result);
                    MsgSvr.Success("编辑成功");
                }
                else
                {
                    MsgSvr.Error($"编辑失败:{result.msg}");
                }
            }
        }

        async Task OnCancel()
        {
            await base.CloseAsync(null);
        }

        async Task OnValidate()
        {
            CronInputDto cronInputDto = new CronInputDto();
            cronInputDto.CronExpression = model.Cron;
            isCronValid = (await Http.PostFromJsonAsync<CronInputDto, bool>($"api/TasksQz/cron", cronInputDto));
            if (isCronValid)
            {
                nextValidTime = (await Http.PostFromJsonAsync<CronInputDto, List<string>>($"api/TasksQz/cron/next", cronInputDto));
                _nextTime = $"[{string.Join("],[", @nextValidTime.ToArray())}]";
            }
            else
            {
                _nextTime = "检查Cron是否正确 ！";
            }

        }
    }

}
