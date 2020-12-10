using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZLMediaServerManagent.DataBase;
using ZLMediaServerManagent.Interface;
using ZLMediaServerManagent.Models.Dto;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.Commons
{
    /// <summary>
    /// 后台服务，判断服务器下线，完成短线重连,获取媒体信息 每隔5s
    /// </summary>
    public class ZLBackGroundTask : BackgroundService
    {
        private IZLServerService zLServerService;
        private IMapper mapper;

        private static bool isRunning = false;

        private readonly ILogger _logger;
        private Timer _timer;
        private readonly IServiceProvider _provider;

        public ZLBackGroundTask(ILogger<ZLBackGroundTask> logger, IServiceProvider _provider)
        {
            _logger = logger;
            this._provider = _provider;


            using (var scope = _provider.CreateScope())
            {
                this.zLServerService = scope.ServiceProvider.GetService<IZLServerService>();
                this.mapper = scope.ServiceProvider.GetService<IMapper>();
                // var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
                // var emailSender = scope.ServiceProvider.GetService<IEmailSender>();
                // fetch customers, send email, update DB
            }

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            if (isRunning == true)
                return;
            isRunning = true;
            if (GloableCache.ZLClient != null && GloableCache.ZLServerOnline)
            {
                var mediaListResp = GloableCache.ZLClient.getMediaList();
                if (mediaListResp.code == -300)
                {
                    GloableCache.ZLServerOnline = false;
                }
                else
                {
                    GloableCache.ZLServerOnline = true;
                    if (mediaListResp.data != null)
                    {   //定时获取媒体流列表
                        GloableCache.MediaStreams.Clear();
                        mediaListResp.data.ForEach(v =>
                        {
                            GloableCache.MediaStreams.Add(v);
                        });
                    }
                    else
                    {
                        GloableCache.MediaStreams.Clear();
                    }
                }
                //比对我的拉流和服务器拉流的差异
                diffStremPorxy();
            }
            else
            {
                if (!GloableCache.IsInitServer)
                {
                    //服务器还没初始化完毕的时候，不要重连服务器
                    return;
                }
                GloableCache.ZLServerOnline = false;

                //尝试重连服务器
                var ip = DataBaseCache.Configs.Where(p => p.ConfigKey == ConfigKeys.ZLMediaServerIp).FirstOrDefault().ConfigValue;
                var port = DataBaseCache.Configs.Where(p => p.ConfigKey == ConfigKeys.ZLMediaServerPort).FirstOrDefault().ConfigValue;
                Console.WriteLine("开始连接ZLMediaKitServer,IP:" + ip + ",Port:" + port + "......");

                var sec = DataBaseCache.Configs.Where(p => p.ConfigKey == ConfigKeys.ZLMediaServerSecret).FirstOrDefault().ConfigValue;
                var client = new STRealVideo.Lib.ZLClient("http://" + ip + ":" + port, sec);
                var configs = client.getServerConfig();
                if (configs.code == -300)
                {
                    GloableCache.ZLServerOnline = false;
                }
                else
                {
                    GloableCache.ZLClient = client;
                    GloableCache.ZLMediaServerConfig = configs.data.FirstOrDefault();
                    GloableCache.ZLServerOnline = true;
                }
            }
            isRunning = false;
        }

        public override void Dispose()
        {
            base.Dispose();
            _timer?.Dispose();
        }

        //比对我的拉流和服务器拉流的差异
        private void diffStremPorxy()
        {
            if (!GloableCache.ZLServerOnline)
            {
                Console.WriteLine("服务器不在线，5s后尝试重连....");
                return;
            }

            var allKeys = GloableCache.MediaStreams.Select(p => p.vhost + "/" + p.app + "/" + p.stream).ToList();
            var myKeys = DataBaseCache.StreamProxies.Where(p => p.State == (int)BaseStatus.Normal &&
              DataBaseCache.Domains.Where(d => d.Id == p.DomainId).First().State == (int)BaseStatus.Normal &&
              DataBaseCache.Applications.Where(d => d.Id == p.ApplicationId).First().State == (int)BaseStatus.Normal
             ).Select(q =>
             new
             {
                 key = (GloableCache.ZLMediaServerConfig.ContainsKey("general.enableVhost") && GloableCache.ZLMediaServerConfig["general.enableVhost"] != "1" ? "__defaultVhost__" : DataBaseCache.Domains.Where(d => d.Id == q.DomainId).First().DomainName) +
              "/" + DataBaseCache.Applications.Where(d => d.Id == q.ApplicationId).First().AppName + "/" + q.StreamId,
                 stream = q
             }
             ).ToList();

            foreach (var item in myKeys)
            {
                if (!allKeys.Contains(item.key))
                {
                    Console.WriteLine("流:" + item.key + ",不在线，正在重新拉流...");
                    //这个流有问题，服务器拉流列表不存在，需要重新拉流
                    var result = zLServerService.StartPullStreamProxy(mapper.Map<StreamProxyDto>(item.stream));
                    if (result.code == 0)
                    {
                        Console.WriteLine("流:" + item.key + ",拉取成功!");
                    }
                    else
                    {
                        Console.WriteLine("流:" + item.key + ",拉取失败：" + result.msg);
                    }
                }
            }

        }
    }


}
