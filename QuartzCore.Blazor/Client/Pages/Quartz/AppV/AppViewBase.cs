using AntDesign;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using QuartzCore.Blazor.Client.Core;
using QuartzCore.Blazor.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace QuartzCore.Blazor.Client.Pages.Quartz
{
    public class AppViewBase : ComponentBase
    {
        private static readonly string[] Summaries = new[]
         {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        [Inject] public ConfirmService ConfirmSvr { get; set; }
        [Inject] public MessageService MsgSvr { get; set; }
        public Data[] data;
        private bool isLoading = false;
        public List<AppInputDto> datas = new List<AppInputDto>();
        public string size = "default";
        [Inject] public HttpClient Http { get; set; }
        protected async  override Task OnInitializedAsync()
        {
             
            await OnQuery();
            //return base.OnInitializedAsync();
        }


        public void OnRowClick(RowData<Data> row)
        {
            Console.WriteLine($"row {row.Data.Key} was clicked");
        }

        [Inject] public DrawerService DrawerSvr { get; set; }

        //private async Task OnChange(AntDesign.TableModels.QueryModel<AppInputDto> queryModel)
        //{
        //    queryDto.PageIndex = queryModel.PageIndex;
        //    queryDto.PageSize = queryModel.PageSize;
        //    await OnQuery();
        //}

        private async Task OnQuery()
        {
            isLoading = true;
            var result = await  Http.GetFromJsonAsync<List<AppInputDto>>("api/app");  
            datas = result.OrderBy(a=>a.CreateTime).ToList();
            isLoading = false;
        }
        public async Task OnAdd()
        {
            var result = await DrawerSvr.CreateDialogAsync<AppEdit, string?, MessageModel<bool>>(null, title: "新增应用", width: 450);
            if (result != null && result.success)
            {
                await OnQuery();
            }
        }

        public async Task OnEdit(AppInputDto model)
        {
            var result = await DrawerSvr.CreateDialogAsync<AppEdit, string?, MessageModel<bool>>(model.Id, title: $"编辑应用 【{model.Name}】", width: 450);
            if (result != null && result.success)
            {
                await OnQuery();
            }
        }

        public async Task OnDelete(AppInputDto model)
        {
            if (await ConfirmSvr.Show($"是否删除任务 {model.Name}", "停止", ConfirmButtons.YesNo, ConfirmIcon.Question) == ConfirmResult.Yes)
            {
                var result = await Http.DeleteFromJsonAsync<MessageModel<bool>>($"api/app/{model.Id}");
                if (result.success)
                {
                    await OnQuery();
                    MsgSvr.Success($"{model.Name} 删除成功");
                }
                else
                    MsgSvr.Error($"{model.Name} 删除失败，错误:{result.msg}");
            }
        }
    }

    public class Data
    {
        [DisplayName("Key")]
        public string Key { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Age")]
        public int Age { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Tags")]
        public string[] Tags { get; set; }
    }
}
