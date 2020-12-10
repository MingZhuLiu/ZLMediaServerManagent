using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLMediaServerManagent.Commons;
using ZLMediaServerManagent.Models.Dto;

namespace ZLMediaServerManagent.Models.ViewDto
{
    public class PlayStreamProxyDto
    {
        public PlayStreamProxyDto()
        {
            ZLServerIp = DataBaseCache.Configs.Where(p => p.ConfigKey == ConfigKeys.ZLMediaServerIp).FirstOrDefault().ConfigValue;
        }


        public PlayStreamProxyDto(StreamProxyDto streamProxy, DomainDto domain, ApplicationDto application) : this()
        {
            this.StreamProxy = streamProxy;
            this.Domain = domain;
            this.Application = application;
            vHost = domain.DomainName;
            if (GloableCache.ZLMediaServerConfig.ContainsKey("general.enableVhost") && GloableCache.ZLMediaServerConfig["general.enableVhost"] != "1")
            {
                vHost = "__defaultVhost__";
            }

        }
        public string ZLServerIp { get; set; }

        public string vHost { get; set; }

        public DomainDto Domain { get; set; }
        public ApplicationDto Application { get; set; }

        public StreamProxyDto StreamProxy { get; set; }


        public virtual string WebSocketUrl
        {
            get
            {
                var port = GloableCache.ZLMediaServerConfig.Where(p => p.Key == "http.port").First().Value;
                return "ws://" + ZLServerIp + ":" + port + "/" + Application.AppName + "/" + StreamProxy.StreamId + ".flv?vhost=" + vHost;
            }
        }

          public virtual string HttpFlvtUrl
        {
            get
            {
                var port = GloableCache.ZLMediaServerConfig.Where(p => p.Key == "http.port").First().Value;
                return "http://" + ZLServerIp + ":" + port + "/" + Application.AppName + "/" + StreamProxy.StreamId + ".flv?vhost=" + vHost;
            }
        }


        public virtual string WebSocketSUrl
        {
            get
            {
                var port = GloableCache.ZLMediaServerConfig.Where(p => p.Key == "http.sslport").First().Value;
                return "wss://" + ZLServerIp + ":" + port + "/" + Application.AppName + "/" + StreamProxy.StreamId + ".flv?vhost=" + vHost;
            }
        }


        public virtual string RTMPUrl
        {
            get
            {
                var port = GloableCache.ZLMediaServerConfig.Where(p => p.Key == "rtmp.port").First().Value;
                return "rtmp://" + ZLServerIp + ":" + port + "/" + Application.AppName + "/" + StreamProxy.StreamId + ".flv?vhost=" + vHost;
            }
        }

        public virtual string RTMPSUrl
        {
            get
            {
                var port = GloableCache.ZLMediaServerConfig.Where(p => p.Key == "rtmp.sslport").First().Value;
                return "rtmps://" + ZLServerIp + ":" + port + "/" + Application.AppName + "/" + StreamProxy.StreamId + ".flv?vhost=" + vHost;
            }
        }

        public virtual string RTSPUrl
        {
            get
            {
                var port = GloableCache.ZLMediaServerConfig.Where(p => p.Key == "rtsp.port").First().Value;
                return "rtsp://" + ZLServerIp + ":" + port + "/" + Application.AppName + "/" + StreamProxy.StreamId + ".flv?vhost=" + vHost;
            }
        }

        public virtual string RTSPSUrl
        {
            get
            {
                var port = GloableCache.ZLMediaServerConfig.Where(p => p.Key == "rtsp.sslport").First().Value;
                return "rtsps://" + ZLServerIp + ":" + port + "/" + Application.AppName + "/" + StreamProxy.StreamId + ".flv?vhost=" + vHost;
            }
        }

        public virtual string HLSUrl
        {
            get
            {
                var port = GloableCache.ZLMediaServerConfig.Where(p => p.Key == "http.port").First().Value;
                return "http://" + ZLServerIp + ":" + port + "/" + Application.AppName + "/" + StreamProxy.StreamId + "/hls.m3u8?vhost=" + vHost;
            }
        }

        public virtual string HLSSUrl
        {
            get
            {
                var port = GloableCache.ZLMediaServerConfig.Where(p => p.Key == "http.sslport").First().Value;
                return "https://" + ZLServerIp + ":" + port + "/" + Application.AppName + "/" + StreamProxy.StreamId + "/hls.m3u8?vhost=" + vHost;
            }
        }


        public virtual string HTTP_TSUrl
        {
            get
            {
                var port = GloableCache.ZLMediaServerConfig.Where(p => p.Key == "http.port").First().Value;
                return "http://" + ZLServerIp + ":" + port + "/" + Application.AppName + "/" + StreamProxy.StreamId + ".live.ts?vhost=" + vHost;
            }
        }

        public virtual string HTTPS_TSUrl
        {
            get
            {
                var port = GloableCache.ZLMediaServerConfig.Where(p => p.Key == "http.sslport").First().Value;
                return "https://" + ZLServerIp + ":" + port + "/" + Application.AppName + "/" + StreamProxy.StreamId + ".live.ts?vhost=" + vHost;
            }
        }
        public virtual string WebSocket_TSUrl
        {
            get
            {
                var port = GloableCache.ZLMediaServerConfig.Where(p => p.Key == "http.port").First().Value;
                return "ws://" + ZLServerIp + ":" + port + "/" + Application.AppName + "/" + StreamProxy.StreamId + ".live.ts?vhost=" + vHost;
            }
        }

        public virtual string WebSocketS_TSUrl
        {
            get
            {
                var port = GloableCache.ZLMediaServerConfig.Where(p => p.Key == "http.sslport").First().Value;
                return "wss://" + ZLServerIp + ":" + port + "/" + Application.AppName + "/" + StreamProxy.StreamId + ".live.ts?vhost=" + vHost;
            }
        }



        public virtual string HTTP_MP4Url
        {
            get
            {
                var port = GloableCache.ZLMediaServerConfig.Where(p => p.Key == "http.port").First().Value;
                return "http://" + ZLServerIp + ":" + port + "/" + Application.AppName + "/" + StreamProxy.StreamId + ".live.mp4?vhost=" + vHost;
            }
        }

        public virtual string HTTPS_MP4Url
        {
            get
            {
                var port = GloableCache.ZLMediaServerConfig.Where(p => p.Key == "http.sslport").First().Value;
                return "https://" + ZLServerIp + ":" + port + "/" + Application.AppName + "/" + StreamProxy.StreamId + ".live.mp4?vhost=" + vHost;
            }
        }
        public virtual string WebSocket_MP4Url
        {
            get
            {
                var port = GloableCache.ZLMediaServerConfig.Where(p => p.Key == "http.port").First().Value;
                return "ws://" + ZLServerIp + ":" + port + "/" + Application.AppName + "/" + StreamProxy.StreamId + ".live.mp4?vhost=" + vHost;
            }
        }

        public virtual string WebSocketS_MP4Url
        {
            get
            {
                var port = GloableCache.ZLMediaServerConfig.Where(p => p.Key == "http.sslport").First().Value;
                return "wss://" + ZLServerIp + ":" + port + "/" + Application.AppName + "/" + StreamProxy.StreamId + ".live.mp4?vhost=" + vHost;
            }
        }
    }
}
