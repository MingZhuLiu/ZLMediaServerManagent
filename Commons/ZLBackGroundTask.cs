using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZLMediaServerManagent.DataBase;
using ZLMediaServerManagent.Models.Dto;

namespace ZLMediaServerManagent.Commons
{
    public class ZLBackGroundTask : BackgroundService
    {
        public ZLBackGroundTask()
        {

        }

        private readonly ILogger _logger;
        private Timer _timer;

        public ZLBackGroundTask(ILogger<ZLBackGroundTask> logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            if (GloableCache.ZLClient != null)
            {
                var mediaListResp = GloableCache.ZLClient.getMediaList();
                if (mediaListResp.code == -300)
                {
                    GloableCache.ZLServerOnline = false;
                }
                else
                {
                    if (mediaListResp.data != null)
                    {   //定时获取媒体流列表
                        GloableCache.MediaStreams.Clear();
                        mediaListResp.data.ForEach(v =>
                        {
                            GloableCache.MediaStreams.Add(v);
                        });
                    }
                }
            }
            else
            {
                GloableCache.ZLServerOnline = false;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _timer?.Dispose();
        }
    }
}
