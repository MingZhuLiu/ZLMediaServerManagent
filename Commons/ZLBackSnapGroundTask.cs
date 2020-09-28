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
    public class ZLBackSnapGroundTask : BackgroundService
    {
        // private IMapper mapper;

        private static bool isRunning = false;

        private readonly ILogger _logger;
        private Timer _timer;
        // private readonly IServiceProvider _provider;

        public ZLBackSnapGroundTask(ILogger<ZLBackGroundTask> logger)
        {
            _logger = logger;



        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            if (isRunning == true)
                return;
            isRunning = true;
            if (GloableCache.ZLClient != null && GloableCache.ZLServerOnline)
            {

                var myStreamProxys = DataBaseCache.StreamProxies.Where(p => p.State == (int)BaseStatus.Normal &&
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

                foreach (var item in myStreamProxys)
                {
                    var imageResponse = GloableCache.ZLClient.getSnap(item.stream.PullStreamUrl, 10, 0);
                    if (imageResponse.code == 0)
                    {
                        if (GloableCache.StreamProxyImages.ContainsKey(item.stream.Id))
                        {
                            GloableCache.StreamProxyImages[item.stream.Id] = imageResponse.data;
                        }
                        else
                        {
                            GloableCache.StreamProxyImages.Add(item.stream.Id, imageResponse.data);
                        }
                    }
                }

            }
            else
            {
            }
            isRunning = false;
        }

        public override void Dispose()
        {
            base.Dispose();
            _timer?.Dispose();
        }


    }


}
