# QuartzCore.Blazor
这是一个基于.net5开发的轻量级Quartz配置中心

1. 部署简单，支持docker部署
2. 支持定时Http WebApi调用(推荐)，亦支持本程序集直接调用
3. 方便统计接入应用和任务项
4. Blazor wasm模式，使用了ant-design-blazor UI 
5. 支持随时修改Trigger,启动立刻生效，无需重启应用
6. 使用Freesql为数据访问组件，亦可学习交流

## 更新记录
1. 删除了no_mongo分支,若不使用mongo自行注释
2. 修复post请求传参,默认需要JSON格式
3. 目前初版功能上大体实现，但耦合度太高，下一步计划把数据库操作,抽象化, 调用任务抽象化。dev_qzbv2分支，matser主分支，先维护已知bug

### Todo

- [ ] 暂无登陆，初始化管理员密码，第一次运行程序需要初始化管理员密码

- [ ] IDS4

- [ ] 监听任务状态，避免运行状态不统一

- [ ] 可支持手动上传DLL方式，动态加载dll并运行,热插拔(暂缓---个人觉得Job补偿机制webapi模式也够用)

- [ ] ant-design-blazor的RangePicker时间选择器还未完善，无法选择具体的时间。目前只能选择日期

- [ ] 首页图表比较丑，目前还没有适合的，要么组件冲突(因为已经使用ant-design-blazor)（有点遗憾）

## 给个星星! ⭐️

如果你喜欢这个项目或者它帮助你, 请给 Star~（辛苦星咯）

## 数据库

使用数据库来存储数据，提供了sqlite和mysql 可以根据用户配置选择，其他数据库亦可支持。使用Freesql为数据访问组件。Freesql对多数据库的支持更加强劲，特别是对国产数据库的支持。但是因为没有国产数据库的测试环境，本项目并未支持，如果有需要我可是开分支尝试支持，但是测试工作就要靠用户啦。

## 初始化数据库

用户只需要手工建一个空库，所有的表在第一次启动的时候都会自动生成。provider对照：
mysql = MySql
sqlite = Sqlite

## Mongo

注：master分支默认有Mongo组件，运行时需要配置mongo地址，若不需要使用mongo，可以选择no_mongo分支直接部署运行即可；Mongo数据主要用于首页实时图表数据展示。

## 运行服务端

```
sudo docker run --name qzBlazor -e db:provider=sqlite -e db:conn="Data Source=dev_qzblazor.db" -p 5001:5001 qzblazor/apkimg
```

注意：qzblazor/apkimg 是我构建的镜像，我未上传仓库 需要docker build   （本例子使用no_mongo分支构建）



## 框架功能

在线项目演示：

✔ [在线展示](http://49.232.221.48:5001)  (blazor wasm 首次加载会慢一些)

首页监控
![01](https://github.com/SmartforXiaoYuan/QuartzCore.Blazor/blob/master/Picture/first.jpg)
   																				 (首页图表)

![02](https://github.com/SmartforXiaoYuan/QuartzCore.Blazor/blob/master/Picture/yingyong01.jpg)
​																									（应用列表）
![03](https://github.com/SmartforXiaoYuan/QuartzCore.Blazor/blob/master/Picture/yingyong02.png)
​																									（应用新增）
![04](https://github.com/SmartforXiaoYuan/QuartzCore.Blazor/blob/master/Picture/zuoye02.png)
​																									（任务作业列表）
![05](https://github.com/SmartforXiaoYuan/QuartzCore.Blazor/blob/master/Picture/zuoye01.png)

​																									（任务作业新增）

功能：

- 实时时间区间执行统计

- 应用管理

- 作业管理

- 执行日志

- 提供Cron表达式验证


## 框架技术栈
![06](https://github.com/SmartforXiaoYuan/QuartzCore.Blazor/blob/master/Picture/mind.jpg)

## 分层介绍
上述的思维导图层次也很清晰，整体上是前后分离2层+share dto层

- QuartzCore.Blazor.Client是单独的前端(类似vue，编译生成是静态文件)

![07](https://github.com/SmartforXiaoYuan/QuartzCore.Blazor/blob/master/Picture/blazorapp.jpg)
(项目文件)

![08](https://github.com/SmartforXiaoYuan/QuartzCore.Blazor/blob/master/Picture/bbianyi.jpg)
(编译生成)

- QuartzCore.Blazor.Share

  ![09](https://github.com/SmartforXiaoYuan/QuartzCore.Blazor/blob/master/Picture/shared.jpg)
  ​(Share DTO)

- QuartzCore.Blazor.Server（api层）正好也是10层，哈哈

  ![10](https://github.com/SmartforXiaoYuan/QuartzCore.Blazor/blob/master/Picture/api.jpg)																		

api层 用到的知识点应该大家都知道，这也不展开说了。


### 为什么写quartzBlazor

1.  方便大家能使用，中小公司开箱即用很是方便。（之前也写过vue的版本[ScheduleJob.Core](https://github.com/SmartforXiaoYuan/ScheduleJob.Core)）

2. 为了实操Blazor和FreeSql  能和大家一起动手写ant-design-blazor,能够体验一下,让更多人的知道Blazor和FreeSql


### 为什么 独立QuartzCore.Tasks类库

1. 主要的原因是反射的Job注入的生命周期和service生命周期不一致，会报错 ; 

### Quartz使用场景

- redis缓存预热
- 业务补偿机制
- 数据同步

### 新增任务项
1. Http WebApi调用方式比较独立，只需要配置api地址支持GET和POST，无需重新部署平台
2. 程序集调用，需要继承JobBase，方便记录日志，需求重新部署平台
注释:报警邮箱是预留的字段，由于没有公共的邮箱服务器，而且也没必要这边先预留，小伙伴若有需求可自己添加上逻辑

### 任务日志
默认只保留每条任务的20条执行记录，亦可根据需要配置

#####  关键问题说明，

_scheduler.ScheduleJob是添加任务，需要等待触发时间才开始执行

暂停调用  _scheduler.PauseJob 那么如果你修改了任务项的信息，那便不会重新添加到_scheduler中去，之前我判断CheckExists 若存在就_scheduler.ResumeJob 执行恢复操作 是不正确的

1. AddJob 貌似没有重新指定 trigger	 
2. 官网的issue中也有发表疑惑（https://github.com/quartznet/quartznet/issues/844）
3. _scheduler.RescheduleJob 可更新时间表达式、

暂时解决方案：目前先选用PauseJob之后,再去DeleteJob

##### SimpleTrigger 

- [x] SimpleTrigger模式下 WithRepeatCount 若设置了自动退出，目前需要手动更新下TasksQz.IsStart=false








### 结尾

本项目的在于提供快捷方便的轻量级可视化quartz配置中心。如你使用中有遇到问题 可以提issue或者提交pro

**感谢大家**

