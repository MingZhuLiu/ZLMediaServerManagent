using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZLMediaServerManagent.Models
{
    public static class Enums
    {

        public enum MessageType
        {
            Success = 200,
            Failed = 500,
            Warning = 400,
            Info = 100,
        }
        /// <summary>
        /// 企业状态
        /// </summary>
        public enum EnterpriseStatus
        {

            /// <summary>
            /// 待审核
            /// </summary>
            Audit = 100,

            /// <summary>
            /// 正常
            /// </summary>
            Normal = 200,

            /// <summary>
            /// 禁止
            /// </summary>
            Forbid = 500,

            /// <summary>
            /// 删除
            /// </summary>
            Deleted = 400,
        }

        public enum BaseStatus
        {
            /// <summary>
            /// 正常
            /// </summary>
            Normal = 200,

            /// <summary>
            /// 禁止
            /// </summary>
            Forbid = 500,

            /// <summary>
            /// 删除
            /// </summary>
            Deleted = 400,
        }

        /// <summary>
        /// 流程计划状态
        /// </summary>
        public enum WorkFlowState
        {
            /// <summary>
            /// 正常
            /// </summary>
            Normal = 200,

            /// <summary>
            /// 禁止
            /// </summary>
            Forbid = 500,

            /// <summary>
            /// 删除
            /// </summary>
            Deleted = 400,
        }

        /// <summary>
        /// 流程状态
        /// </summary>
        public enum WorkState
        {
            /// <summary>
            /// 草稿
            /// </summary>
            Audit = 100,
            /// <summary>
            /// 正常
            /// </summary>
            Normal = 200,

            /// <summary>
            /// 禁止
            /// </summary>
            Forbid = 500,

            /// <summary>
            /// 删除
            /// </summary>
            Deleted = 400,
        }

        public enum WorkApplyState
        {
            /// <summary>
            /// 待审核
            /// </summary>
            Audit = 100,

            /// <summary>
            /// 暂时通过
            /// </summary>
            Normal = 200,

            /// <summary>
            /// 全流程通过
            /// </summary>
            FlowFinish = 222,

            /// <summary>
            /// 拒绝
            /// </summary>
            Forbid = 500,

            /// <summary>
            /// 等待选择时间
            /// </summary>
            WaitSelectTime = 301,

            /// <summary>
            /// 等待面试
            /// </summary>
            WaitInterviewer = 302,

            /// <summary>
            /// 删除
            /// </summary>
            Deleted = 400,

        }

        public enum RecruitmentApplyStatus
        {

            /// <summary>
            /// 待审核
            /// </summary>
            Audit = 100,

            /// <summary>
            /// 待选择时间
            /// </summary>
            Normal = 200,

            /// <summary>
            /// 禁止
            /// </summary>
            Forbid = 500,

            /// <summary>
            ///  待面试
            /// </summary>
            WaitInterViewer = 300,

            /// <summary>
            /// 已取消
            /// </summary>
            Cancel = 700,

            /// <summary>
            /// 面试完毕
            /// </summary>
            Finish = 800,
        }
        public enum RecruitmentPlanStatus
        {

            /// <summary>
            /// 正常
            /// </summary>
            Normal = 200,

            /// <summary>
            /// 删除
            /// </summary>
            Deleted = 400,
        }

        /// <summary>
        /// 用户性别
        /// </summary>
        public enum SexEnum
        {

            /// <summary>
            /// 男
            /// </summary>
            Man = 1,

            /// <summary>
            /// 女
            /// </summary>
            WoMen = 0
        }

        /// <summary>
        /// 链接类型
        /// </summary>
        public enum LinkType
        {
            ///
            /// 话题
            /// 
            Topic = 1001,


            ///
            /// 问题
            /// 
            Question = 1002
        }


        public enum DomainState
        {
            Normal = 200,

            Forbid = 500,

            Deleted = 400
        }
        public enum ApplicationState
        {
            Normal = 200,

            Forbid = 500,

            Deleted = 400
        }

        public enum StreamProxyState
        {
            Normal = 200,

            Forbid = 500,

            Deleted = 400
        }


        public enum RTPType
        {
            TCP = 0,

            UDP = 1,

            Multicast = 2
        }

        public enum MonitorStatus
        {
            Normal = 200,

            Forbid = 500,

            Deleted = 400
        }

        /// <summary>
        /// 招聘状态
        /// </summary>
        public enum RecruitmentStatus
        {

            /// <summary>
            /// 草稿
            /// </summary>
            Audit = 100,
            /// <summary>
            /// 正常
            /// </summary>
            Normal = 200,

            /// <summary>
            /// 禁止
            /// </summary>
            Forbid = 500,

            /// <summary>
            /// 删除
            /// </summary>
            Deleted = 400,
        }

        /// <summary>
        /// 用户状态
        /// </summary>
        public enum UserStatus
        {

            /// <summary>
            /// 正常
            /// </summary>
            Normal = 200,

            /// <summary>
            /// 禁止
            /// </summary>
            Forbid = 500,

            /// <summary>
            /// 删除
            /// </summary>
            Deleted = 400,
        }

        /// <summary>
        /// 会议状态
        /// </summary>
        public enum MeetStatus
        {

            /// <summary>
            /// 正常
            /// </summary>
            Normal = 200,

            /// <summary>
            /// 禁止
            /// </summary>
            Forbid = 500,

            /// <summary>
            /// 删除
            /// </summary>
            Deleted = 400,
        }

        /// <summary>
        /// 登录平台
        /// </summary>
        public enum LoginPlatform
        {

            /// <summary>
            /// 网页
            /// </summary>
            Web = 1000,

            /// <summary>
            /// 安卓
            /// </summary>
            Android = 1001,

            /// <summary>
            /// IOS
            /// </summary>
            IOS = 1002,

            /// <summary>
            /// 微信小程序
            /// </summary>
            WeiXinProgram = 1003

        }

        /// <summary>
        /// Redis缓存Key
        /// </summary>
        public enum RedisCacheTables
        {
            /// <summary>
            /// WebToken
            /// </summary>
            WebTokenDto = 10000001,

            /// <summary>
            /// 角色与菜单对应关系
            /// </summary>
            Role_Menus = 10000002,

            /// <summary>
            /// 用户与角色对应关系
            /// </summary>
            User_Roles = 10000003,

            /// <summary>
            /// 发布的招聘
            /// </summary>
            Publish_Recruitment = 10000004,

            /// <summary>
            /// 审核通过的面试申请
            /// </summary>
            AuditAccessApply = 10000005,
        }

        /// <summary>
        /// Cookies 键值
        /// </summary>
        public enum CookieKeys
        {
            /// <summary>
            /// 网页Token
            /// </summary>
            WebToken = 10000001,

            /// <summary>
            /// 最后一次登录的账号
            /// </summary>
            LastLoginAccout = 10000002
        }

        /// <summary>
        /// 角色状态
        /// </summary>
        public enum RoleStatus
        {
            /// <summary>
            /// 正常
            /// </summary>
            Normal = 200,

            /// <summary>
            /// 禁止
            /// </summary>
            Forbid = 500,

            /// <summary>
            /// 删除
            /// </summary>
            Deleted = 400,
        }

        /// <summary>
        /// 菜单状态
        /// </summary>
        public enum MenuStatus
        {
            /// <summary>
            /// 正常
            /// </summary>
            Normal = 200,

            /// <summary>
            /// 禁止
            /// </summary>
            Forbid = 500,

            /// <summary>
            /// 删除
            /// </summary>
            Deleted = 400,
        }

    }
}