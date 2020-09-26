using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLMediaServerManagent.Commons;
using ZLMediaServerManagent.DataBase;
using ZLMediaServerManagent.Interface;
using ZLMediaServerManagent.Models;
using ZLMediaServerManagent.Models.Dto;
using ZLMediaServerManagent.Models.ViewDto;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.Implements
{
    public class ZLServerService : IZLServerService
    {

        private readonly ZLDataBaseContext dbContext;
        private IMapper mapper;
        private IDomainAndAppService domainAndAppService;
        public ZLServerService(ZLDataBaseContext dbContext, IMapper mapper, IDomainAndAppService domainAndAppService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.domainAndAppService = domainAndAppService;
        }




        public BaseModel<string> AddStreamProxy(StreamProxyDto dto, UserDto owner)
        {
            BaseModel<string> result = new BaseModel<string>();
            if (dto == null || String.IsNullOrWhiteSpace(dto.StreamId) || String.IsNullOrWhiteSpace(dto.StreamName) || String.IsNullOrWhiteSpace(dto.PullStreamUrl))
                return result.Failed("参数错误,流Id/名称/拉流地址不能为空!");

            dto.StreamId = dto.StreamId.Trim();

            var domain = domainAndAppService.FindDomain(dto.DomainId);
            if (domain == null)
            {
                return result.Failed("参数错误,找不到域名!");
            }

            var application = domainAndAppService.FindApplication(dto.ApplicationId);
            if (application == null)
            {
                return result.Failed("参数错误,找不到应用!");
            }

            if (DataBaseCache.StreamProxies.Where(p => p.DomainId == dto.DomainId && p.ApplicationId == dto.ApplicationId && p.StreamId == dto.StreamId).Any())
            {
                return result.Failed("参数错误,流ID已存在!");
            }

            dto.Id = Tools.NewID;
            dto.CreateBy = owner.Id;
            dto.CreateTs = DateTime.Now;
            dto.UpdateBy = owner.Id;
            dto.UpdateTs = DateTime.Now;

            var dbModel = mapper.Map<TbStreamProxy>(dto);
            dbContext.StreamProxies.Add(dbModel);
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {

                DataBaseCache.StreamProxies.Add(dbModel);
                if (dbModel.State == (int)BaseStatus.Normal)
                {
                    //开始拉流
                    StartPullStreamProxy(dto, domain, application);
                }
                else
                {
                    //停止拉流
                }
            }
            return result.Success("添加成功!");
        }

        public BaseModel<string> DeleteStreamProxy(long[] ids, UserDto owner)
        {
            BaseModel<string> result = new BaseModel<string>();
            if (ids == null || ids.Length == 0)
                return result.Failed("参数错误!");

            var dbModels = dbContext.StreamProxies.Where(p => ids.Contains(p.Id)).ToList();
            dbModels.ForEach(v =>
            {
                v.State = (int)BaseStatus.Deleted;
                v.UpdateBy = owner.Id;
                v.UpdateTs = DateTime.Now;
            });
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                dbModels.ForEach(v =>
                {
                    DataBaseCache.StreamProxies.Remove(DataBaseCache.StreamProxies.Where(p => p.Id == v.Id).First());
                    DataBaseCache.StreamProxies.Add(v);
                });
                result.Success("删除成功!");
            }
            else
            {
                result.Failed("删除失败！");
            }
            return result;
        }

        public BaseModel<string> EditStreamProxy(StreamProxyDto dto, UserDto owner)
        {
            BaseModel<string> result = new BaseModel<string>();
            if (dto == null || String.IsNullOrWhiteSpace(dto.StreamId) || String.IsNullOrWhiteSpace(dto.StreamName) || String.IsNullOrWhiteSpace(dto.PullStreamUrl))
                return result.Failed("参数错误,流Id/名称/拉流地址不能为空!");

            dto.StreamId = dto.StreamId.Trim();

            var domain = domainAndAppService.FindDomain(dto.DomainId);
            if (domain == null)
            {
                return result.Failed("参数错误,找不到域名!");
            }

            var application = domainAndAppService.FindApplication(dto.ApplicationId);
            if (application == null)
            {
                return result.Failed("参数错误,找不到应用!");
            }

            if (DataBaseCache.StreamProxies.Where(p => p.DomainId == dto.DomainId && p.ApplicationId == dto.ApplicationId && p.StreamId == dto.StreamId && p.Id != dto.Id).Any())
            {
                return result.Failed("参数错误,流ID已存在!");
            }

            var dbModel = dbContext.StreamProxies.Where(p => p.Id == dto.Id).FirstOrDefault();
            if (dbModel == null)
            {
                return result.Failed("参数错误,找不到该记录!");
            }


            dbModel.StreamId = dto.StreamId;
            dbModel.Description = dto.Description;
            dbModel.StreamName = dto.StreamName;
            dbModel.PullStreamUrl = dto.PullStreamUrl;
            dbModel.EnableHLS = dto.EnableHLS;
            dbModel.EnableMP4 = dto.EnableMP4;
            dbModel.RtpType = (int)dto.RtpType;
            dbModel.UpdateBy = owner.Id;
            dbModel.UpdateTs = DateTime.Now;
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                DataBaseCache.StreamProxies.Remove(DataBaseCache.StreamProxies.Where(p => p.Id == dto.Id).First());
                DataBaseCache.StreamProxies.Add(dbModel);
                if (dto.State == (int)BaseStatus.Normal)
                {
                    //开始拉流
                    StartPullStreamProxy(dto, domain, application);
                }
                else
                {
                    //停止拉流
                }
                return result.Success("修改成功!");
            }
            else
            {
                return result.Failed("修改失败,数据无变化!");
            }
        }

        public StreamProxyDto FindStreamProxy(long StreamProxyId)
        {
            return DataBaseCache.StreamProxies.Where(p => p.Id == StreamProxyId).Select(p => mapper.Map<StreamProxyDto>(p)).FirstOrDefault();
        }

        public STRealVideo.Lib.ZLResponse<STRealVideo.Lib.Models.BodyKey> StartPullStreamProxy(StreamProxyDto streamProxy, DomainDto domain = null, ApplicationDto application = null)
        {
            STRealVideo.Lib.ZLResponse<STRealVideo.Lib.Models.BodyKey> result = new STRealVideo.Lib.ZLResponse<STRealVideo.Lib.Models.BodyKey>();
            if (domain == null)
            {
                domain = DataBaseCache.Domains.Where(p => p.Id == streamProxy.DomainId).Select(p => mapper.Map<DomainDto>(p)).FirstOrDefault();
            }
            if (application == null)
            {
                application = DataBaseCache.Applications.Where(p => p.Id == streamProxy.ApplicationId).Select(p => mapper.Map<ApplicationDto>(p)).FirstOrDefault();
            }
            if (domain == null || application == null)
            {
                result.Failed("找不到域名或应用!");
            }
            if (domain.State != (int)BaseStatus.Normal)
            {
                result.Failed("域名未启用!");
            }
            if (application.State != (int)BaseStatus.Normal)
            {
                result.Failed("应用未启用!");
            }
            result = GloableCache.ZLClient.addStreamProxy(domain.DomainName, application.AppName, streamProxy.StreamId, streamProxy.PullStreamUrl, streamProxy.EnableHLS, streamProxy.EnableMP4, (STRealVideo.Lib.RTPPullType)((int)streamProxy.RtpType));
            return result;
        }

        public void StopPullStreamProxy()
        {
            // return GloableCache.ZLClient.d()
        }

        public TableQueryModel<StreamProxyDto> StreamProxy(QueryModel queryModel)
        {
            TableQueryModel<StreamProxyDto> tableQueryModel = new TableQueryModel<StreamProxyDto>();
            try
            {
                var query = DataBaseCache.StreamProxies.Where(p => p.State != (int)RoleStatus.Deleted).AsQueryable();
                if (queryModel.DomainId.HasValue && queryModel.DomainId != 0)
                {
                    query = query.Where(p => p.DomainId == queryModel.DomainId);
                }
                if (queryModel.ApplicationId.HasValue && queryModel.ApplicationId != 0)
                {
                    query = query.Where(p => p.ApplicationId == queryModel.ApplicationId);
                }
                if (!String.IsNullOrWhiteSpace(queryModel.keyword))
                {
                    query = query.Where(p => p.StreamName.Contains(queryModel.keyword)
                    || p.StreamId.Contains(queryModel.keyword)
                     || (p.Description != null && p.Description.Contains(queryModel.keyword))
                     || (p.PullStreamUrl != null && p.PullStreamUrl.Contains(queryModel.keyword))
                     ).AsQueryable();
                }
                tableQueryModel.count = query.Count();
                if (!String.IsNullOrWhiteSpace(queryModel.field) && !String.IsNullOrWhiteSpace(queryModel.order))
                {
                    query = query.SortBy(queryModel.field + " " + queryModel.order.ToUpper());
                }
                query = query.Skip((queryModel.page - 1) * queryModel.limit);
                query = query.Take(queryModel.limit);

                tableQueryModel.code = 0;
                tableQueryModel.data = query.Select(p => mapper.Map<StreamProxyDto>(p)).ToList();

                foreach (var item in tableQueryModel.data)
                {
                    var domain = DataBaseCache.Domains.Where(c => c.Id == item.DomainId).First();
                    var application = DataBaseCache.Applications.Where(c => c.Id == item.ApplicationId).First();

                    item.DomainName = domain.DomainName;
                    item.ApplicationName = application.AppName;

                    var vHost = domain.DomainName;
                    if (GloableCache.ZLMediaServerConfig.ContainsKey("general.enableVhost") && GloableCache.ZLMediaServerConfig["general.enableVhost"] == "1")
                    {
                        vHost="__defaultVhost__";
                    }

                    var streamMiedas = GloableCache.MediaStreams.Where(p => p.vhost == vHost && p.app == application.AppName && p.stream == item.StreamId).ToList();

                    item.WatchCount = streamMiedas == null || streamMiedas.Count == 0 ? 0 : streamMiedas.Sum(q => q.readerCount);
                    item.WatchTotalCount = streamMiedas == null || streamMiedas.Count == 0 ? 0 : streamMiedas.Sum(q => q.totalReaderCount);
                    if ((streamMiedas == null || streamMiedas.Count == 0) && item.State == (int)BaseStatus.Forbid)
                    {
                        item.ShowStatus = "<font color=\"e1473c\">已停用</font>";
                    }
                    else if ((streamMiedas == null || streamMiedas.Count == 0) && item.State == (int)BaseStatus.Normal)
                    {
                        item.isNeedRePullStreamProxy = true;
                        item.ShowStatus = "<font color=\"e1473c\">拉流失败</font>";
                    }
                    else if (!(streamMiedas == null || streamMiedas.Count == 0) && item.State == (int)BaseStatus.Normal)
                    {
                        item.ShowStatus = "<font color=\"a9e879\">正常</font>";
                    }
                    else if (!(streamMiedas == null || streamMiedas.Count == 0) && item.State == (int)BaseStatus.Forbid)
                    {
                        item.ShowStatus = "<font color=\"e1473c\">已停用,正在拉流</font>";
                    }
                    item.ShowStreamMediaJson = (streamMiedas == null || streamMiedas.Count == 0) ? "" : Tools.FormatJsonString(Newtonsoft.Json.JsonConvert.SerializeObject(streamMiedas));
                }
            }
            catch (Exception ex)
            {
                tableQueryModel.code = 1;
                tableQueryModel.msg = ex.Message;
            }
            return tableQueryModel;
        }
    }
}