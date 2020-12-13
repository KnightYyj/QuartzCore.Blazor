using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QuartzCore.Blazor.Shared;
using QuartzCore.Data.Entity;
using QuartzCore.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuartzCore.Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private readonly IAppService _appService;
        private readonly IMapper _mapper;

        private readonly ITasksQzService _tasksQzService;
        public AppController(IAppService appService, IMapper mapper, ITasksQzService tasksQzService)
        {
            _appService = appService;
            _mapper = mapper;
            _tasksQzService = tasksQzService;
        }
        // GET: api/<AppController>
        [HttpGet]
        public async Task<List<AppInputDto>> Get()
        {
            var apps = await _appService.GetAsync();
            var res= _mapper.Map<List<AppInputDto>>(apps);
            return res;
        }

        [HttpGet("{id}")]
        public async Task<AppInputDto> Get(string id)
        {
            var app = await _appService.GetAsync(id);
            var res = _mapper.Map<AppInputDto>(app);
            return res;
        }

        [HttpPost]
        public async Task<MessageModel<bool>> Add([FromBody] AppInputDto model)
        {
            MessageModel<bool> response = new MessageModel<bool>();
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            var oldApp = await _appService.GetAsync(model.Id);
            if (oldApp != null)
            {
                return new MessageModel<bool>()
                {
                    success = false,
                    msg = "应用Id已存在，请重新输入。"
                };
            }
            var app = new AppEntity();
            app.Id = model.Id;
            app.Name = model.Name;
            app.Enabled = model.Enabled;
            app.CreateTime = DateTime.Now;
            app.UpdateTime = null;

            var result = await _appService.AddAsync(app);
            if (result)
            {
                response.success = true;
                response.msg = "新增成功";
                return response;
            }
            return new MessageModel<bool>()
            {
                success = false,
                msg = !result ? "新建应用失败，请查看错误日志" : ""
            };
        }

 

        // PUT api/<AppController>/5
        [HttpPut("{id}")]
        public async Task<MessageModel<bool>> Put(string id, [FromBody] AppInputDto model)
        {
            MessageModel<bool> response = new MessageModel<bool>();
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            var app = await _appService.GetAsync(model.Id);
            if (app == null)
            {
                return new MessageModel<bool>()
                {
                    success = false,
                    msg = "未找到对应的应用程序。"
                };
            }
            app.Name = model.Name;
            app.Enabled = model.Enabled;
            app.UpdateTime = DateTime.Now;
            var result = await _appService.UpdateAsync(app);
            if (result)
            {
                response.success = true;
                response.msg = "修改成功";
                return response;
            }
            return new MessageModel<bool>()
            {
                success = false,
                msg = !result ? "修改应用失败，请查看错误日志" : ""
            };
        }

        // DELETE api/<AppController>/5
        [HttpDelete("{id}")]
        public async Task<MessageModel<bool>> Delete(string id)
        {
            MessageModel<bool> response = new MessageModel<bool>();
             
            var app = await _appService.GetAsync(id);
            if (app == null)
            {
                return new MessageModel<bool>()
                {
                    success = false,
                    msg = "未找到对应的应用程序。"
                };
            }
            //是否有作业关联
            var qzModel = await _tasksQzService.GetByAppIdAsync(id);
            if(qzModel!=null && qzModel.Id > 0)
            {
                return new MessageModel<bool>()
                {
                    success = false,
                    msg = "无法删除，应用已经绑定作业。"
                };
            }

            var result = await _appService.DeleteAsync(id);
            if (result)
            {
                response.success = true;
                response.msg = "删除成功";
                return response;
            }
            return new MessageModel<bool>()
            {
                success = false,
                msg = !result ? "修改应用失败，请查看错误日志" : ""
            };
        }
    }
}
