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
    public class DomainAndAppService : IDomainAndAppService
    {

        private readonly ZLDataBaseContext dbContext;
        private IMapper mapper;
        public DomainAndAppService(ZLDataBaseContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }


        public TableQueryModel<ApplicationDto> QueryApplicationList(QueryModel queryModel)
        {
            TableQueryModel<ApplicationDto> tableQueryModel = new TableQueryModel<ApplicationDto>();
            try
            {
                if (!queryModel.parentId.HasValue || queryModel.parentId == 0)
                    return tableQueryModel.Failed("请选择域名!");
                var query = DataBaseCache.Applications.Where(p =>p.DomainId==queryModel.parentId&& p.State != (int)RoleStatus.Deleted).AsQueryable();
                if (!String.IsNullOrWhiteSpace(queryModel.keyword))
                {
                    query = query.Where(p => p.AppName.Contains(queryModel.keyword)).AsQueryable();
                }
                tableQueryModel.count = query.Count();
                if (!String.IsNullOrWhiteSpace(queryModel.field) && !String.IsNullOrWhiteSpace(queryModel.order))
                {
                    query = query.SortBy(queryModel.field + " " + queryModel.order.ToUpper());
                }
                query = query.Skip((queryModel.page - 1) * queryModel.limit);
                query = query.Take(queryModel.limit);
                tableQueryModel.code = 0;
                tableQueryModel.data = query.Select(p => mapper.Map<ApplicationDto>(p)).ToList();

            }
            catch (Exception ex)
            {
                tableQueryModel.code = 1;
                tableQueryModel.msg = ex.Message;
            }
            return tableQueryModel;
        }

        public TableQueryModel<DomainDto> QueryDomainList(QueryModel queryModel)
        {
            TableQueryModel<DomainDto> tableQueryModel = new TableQueryModel<DomainDto>();
            try
            {
                var query = DataBaseCache.Domains.Where(p => p.State != (int)RoleStatus.Deleted).AsQueryable();


                if (!String.IsNullOrWhiteSpace(queryModel.keyword))
                {
                    query = query.Where(p => p.DomainName.Contains(queryModel.keyword) || (p.Description != null && p.Description.Contains(queryModel.keyword))).AsQueryable();
                }
                tableQueryModel.count = query.Count();
                if (!String.IsNullOrWhiteSpace(queryModel.field) && !String.IsNullOrWhiteSpace(queryModel.order))
                {
                    query = query.SortBy(queryModel.field + " " + queryModel.order.ToUpper());
                }
                query = query.Skip((queryModel.page - 1) * queryModel.limit);
                query = query.Take(queryModel.limit);

                tableQueryModel.code = 0;
                tableQueryModel.data = query.Select(p => mapper.Map<DomainDto>(p)).ToList();

            }
            catch (Exception ex)
            {
                tableQueryModel.code = 1;
                tableQueryModel.msg = ex.Message;
            }
            return tableQueryModel;
        }

        public BaseModel<string> AddDoamin(DomainDto dto, UserDto owner)
        {
            BaseModel<string> result = new BaseModel<string>();
            if (dto == null || String.IsNullOrWhiteSpace(dto.DomainName))
                return result.Failed("参数错误,域名不能为空!");

            dto.DomainName = dto.DomainName.Trim();
            if (DataBaseCache.Domains.Where(p => p.DomainName == dto.DomainName && p.State != 400).Any())
            {
                return result.Failed("添加失败，已存在相同域名!");
            }
            dto.Id = Tools.NewID;
            dto.CreateBy = owner.Id;
            dto.CreateTs = DateTime.Now;
            dto.UpdateBy = owner.Id;
            dto.UpdateTs = DateTime.Now;

            var dbModel = mapper.Map<TbDomain>(dto);

            dbContext.Domains.Add(dbModel);
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                DataBaseCache.Domains.Add(dbModel);
            }
            return result.Success("添加成功!");
        }

        public BaseModel<string> DeleteDoamin(long[] ids, UserDto owner)
        {
            BaseModel<string> result = new BaseModel<string>();
            if (ids == null || ids.Length == 0)
                return result.Failed("参数错误!");

            var dbModels = dbContext.Domains.Where(p => ids.Contains(p.Id)).ToList();
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
                    DataBaseCache.Domains.Remove(DataBaseCache.Domains.Where(p => p.Id == v.Id).First());
                    DataBaseCache.Domains.Add(v);
                });
                result.Success("删除成功!");
            }
            else
            {
                result.Failed("删除失败！");
            }
            return result;
        }

        public BaseModel<string> EditDoamin(DomainDto dto, UserDto owner)
        {
            BaseModel<string> result = new BaseModel<string>();
            if (dto == null || String.IsNullOrWhiteSpace(dto.DomainName))
                return result.Failed("参数错误,域名不能为空!");

            dto.DomainName = dto.DomainName.Trim();
            var dbModel = dbContext.Domains.Where(p => p.Id == dto.Id).FirstOrDefault();
            if (dbModel == null)
            {
                return result.Failed("参数错误,找不到该域名!");
            }

            if (DataBaseCache.Domains.Where(p => p.DomainName == dto.DomainName && p.State != 400 && p.Id != dbModel.Id).Any())
            {
                return result.Failed("域名冲突，已存在相同域名，修改失败!");
            }
            else
            {
                dbModel.DomainName = dto.DomainName;
                dbModel.Description = dto.Description;
                dbModel.UpdateBy = owner.Id;
                dbModel.UpdateTs = DateTime.Now;
                var flag = dbContext.SaveChanges() > 0 ? true : false;
                if (flag)
                {
                    DataBaseCache.Domains.Remove(DataBaseCache.Domains.Where(p => p.Id == dto.Id).First());
                    DataBaseCache.Domains.Add(dbModel);
                    return result.Success("修改成功!");
                }
                else
                {
                    return result.Failed("修改失败,数据无变化!");
                }
            }
        }

        public DomainDto FindDomain(long domainId)
        {
            return DataBaseCache.Domains.Where(p => p.Id == domainId).Select(p => mapper.Map<DomainDto>(p)).FirstOrDefault();
        }

        public ApplicationDto FindApplication(long applicationId)
        {
            return DataBaseCache.Applications.Where(p => p.Id == applicationId).Select(p => mapper.Map<ApplicationDto>(p)).FirstOrDefault();
        }

        public BaseModel<string> AddApplication(ApplicationDto dto, UserDto owner)
        {
            BaseModel<string> result = new BaseModel<string>();
            if (dto == null || String.IsNullOrWhiteSpace(dto.AppName))
                return result.Failed("参数错误,应用名称不能为空!");

            var domain = FindDomain(dto.DomainId);
            if (domain == null)
            {
                return result.Failed("请检查域名是否选择有误!");
            }


            dto.AppName = dto.AppName.Trim();
            if (DataBaseCache.Applications.Where(p => p.AppName == dto.AppName && p.State != 400 && p.DomainId == dto.DomainId).Any())
            {
                return result.Failed("添加失败，已存在相同应用!");
            }
            dto.Id = Tools.NewID;
            dto.CreateBy = owner.Id;
            dto.CreateTs = DateTime.Now;
            dto.UpdateBy = owner.Id;
            dto.UpdateTs = DateTime.Now;

            var dbModel = mapper.Map<TbApplication>(dto);

            dbContext.Applications.Add(dbModel);
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                DataBaseCache.Applications.Add(dbModel);
            }
            return result.Success("添加成功!");
        }

        public BaseModel<string> EditApplication(ApplicationDto dto, UserDto owner)
        {
            BaseModel<string> result = new BaseModel<string>();
            if (dto == null || String.IsNullOrWhiteSpace(dto.AppName))
                return result.Failed("参数错误,应用名称不能为空!");

            var domain = FindDomain(dto.DomainId);
            if (domain == null)
            {
                return result.Failed("请检查域名是否选择有误!");
            }

            dto.AppName = dto.AppName.Trim();
            var dbModel = dbContext.Applications.Where(p => p.Id == dto.Id).FirstOrDefault();
            if (dbModel == null)
            {
                return result.Failed("参数错误,找不到该应用!");
            }

            if (DataBaseCache.Applications.Where(p => p.AppName == dto.AppName && p.State != 400 && p.Id != dbModel.Id && p.DomainId == dto.DomainId).Any())
            {
                return result.Failed("应用冲突，已存在相同应用，修改失败!");
            }
            else
            {
                dbModel.AppName = dto.AppName;
                dbModel.Description = dto.Description;
                dbModel.UpdateBy = owner.Id;
                dbModel.UpdateTs = DateTime.Now;
                var flag = dbContext.SaveChanges() > 0 ? true : false;
                if (flag)
                {
                    DataBaseCache.Applications.Remove(DataBaseCache.Applications.Where(p => p.Id == dto.Id).First());
                    DataBaseCache.Applications.Add(dbModel);
                    return result.Success("修改成功!");
                }
                else
                {
                    return result.Failed("修改失败,数据无变化!");
                }
            }
        }

        public BaseModel<string> DeleteApplication(long[] ids, UserDto owner)
        {
            BaseModel<string> result = new BaseModel<string>();
            if (ids == null || ids.Length == 0)
                return result.Failed("参数错误!");

            var dbModels = dbContext.Applications.Where(p => ids.Contains(p.Id)).ToList();
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
                    DataBaseCache.Applications.Remove(DataBaseCache.Applications.Where(p => p.Id == v.Id).First());
                    DataBaseCache.Applications.Add(v);
                });
                result.Success("删除成功!");
            }
            else
            {
                result.Failed("删除失败！");
            }
            return result;
        }
    }
}