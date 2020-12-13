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
    public partial class AppEdit : DrawerTemplate<string?, MessageModel<bool>>
    {
        [Inject] public MessageService MsgSrv { get; set; }
        [Parameter]
        public string ModalName { get; set; }
        [Parameter]
        public bool IsShow { get; set; } = false;

        public Guid Guid = Guid.NewGuid();

        [Inject] public HttpClient Http { get; set; }

        AppInputDto model { get; set; }

        bool isLoading = false;

        bool isEdit = false;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            Console.WriteLine(base.Options);
            if (!string.IsNullOrEmpty(base.Options))
            {
                model = (await Http.GetFromJsonAsync<AppInputDto>($"api/App/{base.Options}"));
                isEdit = true;
            }
            else
            {
                model = new AppInputDto();
                isEdit = false;
            }
            await base.OnInitializedAsync();
            isLoading = false;
        }
        async Task OnSave()
        {
            if (!isEdit)
            {
                var result = await Http.PostFromJsonAsync<AppInputDto, MessageModel<bool>>($"api/App", model);
                if (result.success == true)
                {
                    await base.CloseAsync(result);
                    await MsgSrv.Success("新增成功");
                }
                else
                {
                    await MsgSrv.Error($"新增失败:{result.msg}");
                }
            }
            else
            {
                var result = await Http.PutFromJsonAsync<AppInputDto, MessageModel<bool>>($"api/App/{model.Id}", model);
                if (result.success == true)
                {
                    await base.CloseAsync(result);
                    await MsgSrv.Success("编辑成功");
                }
                else
                {
                    await MsgSrv.Error($"编辑失败:{result.msg}");
                }
            }
        }

        async Task OnCancel()
        {
            await base.CloseAsync(null);
        }

        #region back
        /*
        public void Open(string name)
        {
            model = new AppInputDto();
            ModalName = name;
            IsShow = true;
            StateHasChanged();
        }
        public void Close()
        {
            IsShow = false;
            StateHasChanged();
        }
        protected override async Task OnInitializedAsync()
        {
            loading = true;
            Console.WriteLine(base.Options);
            loading = false;
        }
        private AppInputDto model = new AppInputDto();

        protected override void OnInitialized()
        {
            //Console.WriteLine($"1111");
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //Console.WriteLine($"{firstRender}");
            }
        }

        private async Task OnFinish(EditContext editContext)
        {
            Console.WriteLine(base.Options);
            Console.WriteLine($"Success:{JsonSerializer.Serialize(model)}");
            toggle(true);
            //var json = new StringContent(JsonSerializer.Serialize(model), System.Text.Encoding.UTF8, "application/json");
            var result = await Http.PostFromJsonAsync<AppInputDto, MessageModel<bool>>($"api/App", model);
            if (result.success == true)
            {
                await MsgSrv.Success("保存成功");
            }
            else
            {
                await MsgSrv.Error("保存失败");
            }
            toggle(false);
            IsShow = false;
            StateHasChanged();
        }

        private void OnFinishFailed(EditContext editContext)
        {
            Console.WriteLine($"Failed:{JsonSerializer.Serialize(model)}");
        }

        bool loading = false;
        void toggle(bool value) => loading = value;
        */
        #endregion

    }
}
