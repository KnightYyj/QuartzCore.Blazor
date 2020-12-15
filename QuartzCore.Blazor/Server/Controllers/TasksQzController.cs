using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QuartzCore.Blazor.Shared;
using QuartzCore.Data.Entity;
using QuartzCore.IService;
using QuartzCore.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuartzCore.Blazor.Server.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class TasksQzController : ControllerBase
    {
        private readonly ITasksQzService _tasksQzService;
        private readonly IQzRunLogService _qzRunLogService;
        private readonly IMapper _mapper;
        private readonly ISchedulerCenter _schedulerCenter;
        public TasksQzController(ITasksQzService tasksQzService, IMapper mapper, ISchedulerCenter schedulerCenter, IQzRunLogService qzRunLogService)
        {
            _tasksQzService = tasksQzService;
            _mapper = mapper;
            _schedulerCenter = schedulerCenter;
            _qzRunLogService = qzRunLogService;
        }

        [HttpGet("dash")]
        public async Task<MessageModel<DashDto>> GetDash()
        {
            var res = await _tasksQzService.GetDashAsync();
            return res;
        }
        #region GET

        // GET: api/<TasksQzController>
        [HttpGet]
        public async Task<List<TasksQzDto>> Get()
        {
            var allQzServices = await _tasksQzService.GetAsync();
            return _mapper.Map<List<TasksQzDto>>(allQzServices);
        }

        // GET api/<TasksQzController>/5
        [HttpGet("{id}")]
        public async Task<TasksQzDto> Get(int id)
        {
            var apps = await _tasksQzService.GetAsync(id);
            var res = _mapper.Map<TasksQzDto>(apps);
            return res;
        }


        [HttpPost("list")]
        public async Task<MessageModel<List<TasksQzDto>>> Find([FromBody] JobItemQueryDto queryDto)
        {
            var messageModel = new MessageModel<List<TasksQzDto>>();
            messageModel.success = true;
            messageModel.msg = "成功";

            var ss = await _tasksQzService.FindAsync(queryDto);
            var dtos = _mapper.Map<List<TasksQzDto>>(ss.response);
            var logs = await _qzRunLogService.Find(dtos.Select(x => x.Id).ToArray());
            dtos.AsParallel().ForAll(a =>
            {
                var log = logs.FindAll(x => x.TasksQzId == a.Id);
                if (log != null && log.Any())
                    a.Logs = _mapper.Map<List<QzRunLogDto>>(log.OrderByDescending(x=>x.LogTime).Take(5));
            });
            messageModel.response = dtos;
            messageModel.Total = ss.Total;
            return messageModel;
        }

        [HttpPost("log/list")]
        public async Task<MessageModel<List<QzRunLogDto>>> Find([FromBody] QzRunLogQueryDto queryDto)
        {
            var messageModel = new MessageModel<List<QzRunLogDto>>();
            var result = await _qzRunLogService.Find(queryDto);
            var dtos = _mapper.Map<List<QzRunLogDto>>(result.response);
            messageModel.response = dtos;
            messageModel.success = true;
            messageModel.msg = "成功";
            messageModel.Total = result.Total;
            return messageModel;
        }
        #endregion

        // POST api/<TasksQzController>
        [HttpPost]
        public async Task<MessageModel<TasksQzDto>> Add([FromBody] TasksQzDto model)
        {
            MessageModel<TasksQzDto> response = new MessageModel<TasksQzDto>();
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            var oldItem = await _tasksQzService.GetAsync(model.Name);
            if (oldItem != null)
            {
                return new MessageModel<TasksQzDto>()
                {
                    success = false,
                    msg = "任务名称已存在，请重新输入。"
                };
            }
            var app = _mapper.Map<TasksQzEntity>(model);
            app.CreateTime = DateTime.Now;
            app.UpdateTime = null;
            if (!app.IsApiUrl)
                app.MethodType = 0;
            else
            {
                app.AssemblyName = "QuartzCore.Tasks";
                app.ClassName = "Job_HttpApi_Quartz";
            }
            var result = await _tasksQzService.AddAsync(app);
            if (result)
            {
                response.success = true;
                response.msg = "新增成功";
                response.response = _mapper.Map<TasksQzDto>(app);
                return response;
            }
            return new MessageModel<TasksQzDto>()
            {
                success = false,
                msg = !result ? "新建任务失败，请查看错误日志" : ""
            };
        }

        // PUT api/<TasksQzController>/5
        [HttpPut("{id}")]
        public async Task<MessageModel<TasksQzDto>> Put(int id, [FromBody] TasksQzDto model)
        {
            MessageModel<TasksQzDto> response = new MessageModel<TasksQzDto>();
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            var app = await _tasksQzService.GetAsync(model.Id);
            if (app == null)
            {
                return new MessageModel<TasksQzDto>()
                {
                    success = false,
                    msg = "未找到对应的作业项。"
                };
            }
            //检查一下 任务是否在运行，运行中不能修改
            if (app.IsStart)
            {
                return new MessageModel<TasksQzDto>()
                {
                    success = false,
                    msg = "请先停止作业，再尝试修改。"
                };
            }

            app = _mapper.Map<TasksQzEntity>(model);
            if (!app.IsApiUrl)
                app.MethodType = 0;
            app.UpdateTime = DateTime.Now;
            var result = await _tasksQzService.UpdateAsync(app);
            if (result)
            {
                response.success = true;
                response.msg = "修改成功";
                response.response = _mapper.Map<TasksQzDto>(app);
                return response;
            }
            return new MessageModel<TasksQzDto>()
            {
                success = false,
                msg = !result ? "修改作业失败，请查看错误日志" : ""
            };
        }

        // DELETE api/<TasksQzController>/5
        [HttpDelete("{id}")]
        public async Task<MessageModel<bool>> Delete(int id)
        {
            MessageModel<bool> response = new MessageModel<bool>();
 
            var app = await _tasksQzService.GetAsync(id);
            if (app == null)
            {
                return new MessageModel<bool>()
                {
                    success = false,
                    msg = "未找到对应的作业项。"
                };
            }
            //检查一下 任务是否在运行，运行中不能修改
            if (app.IsStart)
            {
                return new MessageModel<bool>()
                {
                    success = false,
                    msg = "请先停止作业，再尝试删除。"
                };
            }
            
            app.UpdateTime = DateTime.Now;
            app.IsDeleted = true;
            var result = await _tasksQzService.UpdateAsync(app);
            if (result)
            {
                response.success = true;
                response.msg = "删除成功";
                response.response = true;
                return response;
            }
            return new MessageModel<bool>()
            {
                success = false,
                msg = !result ? "删除作业失败，请查看错误日志" : ""
            };
        }

        [HttpPost("cron")]
        public bool isValidExpression(CronInputDto inputDto)
        {
            return _tasksQzService.isValidExpression(inputDto.CronExpression);
        }
        [HttpPost("cron/next")]
        public List<string> NextValidTime(CronInputDto inputDto)
        {
            return _tasksQzService.NextValidTime(inputDto.CronExpression);
        }



        #region 核心功能
        /// <summary> 
        /// 启动计划任务  
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet("start/{jobId}")]
        public async Task<MessageModel<TasksQzDto>> StartJob(int jobId)
        {
            var data = new MessageModel<TasksQzDto>();

            var model = await _tasksQzService.GetAsync(jobId);
            if (model != null)
            {
                if (model.IsDeleted)
                {
                    data.success = false;
                    data.msg = "此任务无效，无法启动无效的任务项 。";
                    return data;
                }
                if (model.IsStart)
                {
                    data.success = false;
                    data.msg = "此任务已经启动，无法再次启动 。";
                    return data;
                }
                var ResuleModel = await _schedulerCenter.AddScheduleJobAsync(model);
                data.msg = ResuleModel.msg;
                if (ResuleModel.success)
                {
                    model.IsStart = true;
                    data.success = await _tasksQzService.UpdateAsync(model);
                }
                if (data.success)
                {
                    data.msg = "启动成功";
                    data.response = _mapper.Map<TasksQzDto>(model);
                }
            }
            return data;

        }
        /// <summary>
        /// 停止一个计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>        
        [HttpGet("stop/{jobId}")]
        public async Task<MessageModel<TasksQzDto>> StopJob(int jobId)
        {
            var data = new MessageModel<TasksQzDto>();

            var model = await _tasksQzService.GetAsync(jobId);
            if (model != null)
            {
                var ResuleModel = await _schedulerCenter.StopScheduleJobAsync(model);
                if (ResuleModel.success)
                {
                    model.IsStart = false;
                    data.success = await _tasksQzService.UpdateAsync(model);
                }
                if (data.success)
                {
                    data.msg = "暂停成功";
                    data.response = _mapper.Map<TasksQzDto>(model);
                }
            }
            return data;
        }



        /// <summary>
        /// 重启一个计划任务 配合PauseJob
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet("recovery")]
        public async Task<MessageModel<string>> ReCovery(int jobId)
        {
            var data = new MessageModel<string>();

            var model = await _tasksQzService.GetAsync(jobId);
            if (model != null)
            {
                var ResuleModel = await _schedulerCenter.ResumeJob(model);
                if (ResuleModel.success)
                {
                    model.IsStart = true;
                    data.success = await _tasksQzService.UpdateAsync(model);
                }
                if (data.success)
                {
                    data.msg = "重启成功";
                    data.response = jobId.ToString();
                }
            }
            return data;

        }
        #endregion

    }
}
