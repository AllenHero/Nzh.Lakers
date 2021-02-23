using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nzh.Lakers.Model
{
    public class EnumType
    {
        public enum StatusCodeType
        {
            [Text("请求(或处理)成功")]
            Success = 200,

            [Text("内部请求出错")]
            Error = 500,

            [Text("访问请求未授权! 当前 SESSION 失效, 请重新登陆")]
            Unauthorized = 401,

            [Text("请求参数不完整或不正确")]
            ParameterError = 400,

            [Text("您无权进行此操作，请求执行已拒绝")]
            Forbidden = 403,

            [Text("找不到与请求匹配的 HTTP 资源")]
            NotFound = 404,

            [Text("HTTP请求类型不合法")]
            HttpMehtodError = 405,

            [Text("HTTP请求不合法,请求参数可能被篡改")]
            HttpRequestError = 406,

            [Text("该URL已经失效")]
            URLExpireError = 407,
        }

        public enum GenderType
        {
            [Text("男")]
            Man = 0,

            [Text("女")]
            Woman = 1,
        }

        public enum StatusType
        {
            [Text("未启用")]
            DisEnable = 0,

            [Text("已启用")]
            Enabled = 1,
        }

        public enum IsDeletedType
        {
            [Text("未启用")]
            No = 0,

            [Text("已启用")]
            Yes = 1,
        }

        public enum LogTypeType
        {
            [Text("登录")]
            Login,

            [Text("新增")]
            Add,

            [Text("删除")]
            Delete,

            [Text("修改")]
            Update,

            [Text("查询")]
            Query,
        }

        public enum LogStatusType
        {
            [Text("成功")]
            Success = 0,

            [Text("失败")]
            Fail = 1,
        }
    }

    public class TextAttribute : Attribute
    {
        public TextAttribute(string value)
        {
            Value = value;
        }
        public string Value { get; set; }
    }

    public static class EnumExtension
    {
        /// <summary>
        /// 获得枚举提示文本
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetEnumText(this Enum obj)
        {
            Type type = obj.GetType();
            FieldInfo field = type.GetField(obj.ToString());
            TextAttribute attribute = (TextAttribute)field.GetCustomAttribute(typeof(TextAttribute));
            return attribute.Value;
        }
    }
}
