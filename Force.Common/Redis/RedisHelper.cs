using Force.Common.RedisTool.Base;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Force.Common.RedisTool.Helper
{
    public class RedisHelper : IRedisHelper
    {
        private IRedisBase _redis;

        public RedisHelper(IRedisBase redis)
        {
            _redis = redis;
        }

        public IDatabase DB() {
            return _redis.DB();
        }
        /// <summary>
        /// 获取值，不支持的类型将不会被赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T HashGet<T>(string key, bool isLock = false) where T : class
        {
            if (isLock)
            {
                var lock_name = _redis.GetKeyStr("lock", key, true);
                return _redis.DoWithLock(lock_name, () =>
                {
                    return HashGetPrivate<T>(key);
                }, true);
            }
            else
            {
                return HashGetPrivate<T>(key);
            }
        }

        /// <summary>
        /// 存值，使用前请看清楚支持的类型，不支持的类型将不会被存储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="model"></param>
        public void HashSet<T>(string key, T model, bool isLock = false) where T : class
        {
            if (isLock)
            {
                var lock_name = _redis.GetKeyStr("lock", key, true);
                _redis.DoWithLock(lock_name, () =>
                {
                    HashSetPrivate(key, model);
                }, true);
            }
            else
            {
                HashSetPrivate(key, model);
            }
        }

        private T HashGetPrivate<T>(string key)
        {
            T newModel = Activator.CreateInstance<T>();
            if (_redis.DB().KeyExists(key))
            {
                var hashValue = _redis.DB().HashGetAll(key);
                var type = newModel.GetType();
                foreach (var item in hashValue)
                {
                    var info = type.GetProperty(item.Name);
                    ///decimal 类型暂时无法支持，因为存储的时候decimal类型是无法存进redis的
                    switch (info.PropertyType.FullName)
                    {
                        case "System.Byte":
                            info.SetValue(newModel, Convert.ToByte(item.Value.ToString()), null);
                            break;
                        case "System.Int16":
                            info.SetValue(newModel, Convert.ToInt16(item.Value.ToString()), null);
                            break;
                        case "System.Int32":
                            info.SetValue(newModel, Convert.ToInt32(item.Value.ToString()), null);
                            break;
                        case "System.Int64":
                            info.SetValue(newModel, Convert.ToInt64(item.Value.ToString()), null);
                            break;
                        case "System.SByte":
                            info.SetValue(newModel, Convert.ToSByte(item.Value.ToString()), null);
                            break;
                        case "System.UInt16":
                            info.SetValue(newModel, Convert.ToUInt16(item.Value.ToString()), null);
                            break;
                        case "System.UInt32":
                            info.SetValue(newModel, Convert.ToUInt32(item.Value.ToString()), null);
                            break;
                        case "System.UInt64":
                            info.SetValue(newModel, Convert.ToUInt64(item.Value.ToString()), null);
                            break;
                        case "System.Double":
                            info.SetValue(newModel, Convert.ToDouble(item.Value.ToString()), null);
                            break;
                        case "System.Single"://float
                            info.SetValue(newModel, Convert.ToSingle(item.Value.ToString()), null);
                            break;
                        case "System.String":
                            info.SetValue(newModel, item.Value.ToString(), null);
                            break;
                        case "System.Decimal"://不这样做会有精度损失问题
                            info.SetValue(newModel, (decimal)Convert.ToDouble(item.Value.ToString()), null);
                            break;
                        case "System.Boolean":
                            info.SetValue(newModel, Convert.ToBoolean((int)item.Value), null);
                            break;
                        case "System.DateTime":
                            info.SetValue(newModel, Convert.ToDateTime(item.Value.ToString()), null);
                            break;
                        case "System.TimeSpan":
                            info.SetValue(newModel, TimeSpan.Parse(item.Value.ToString()), null);
                            break;
                        default:
                            continue;
                    }
                }
            }
            return newModel;
        }

        private void HashSetPrivate<T>(string key, T model)
        {
            Type type = typeof(T);
            var members = Cache.ModelCache.Get(type);
            if (members == null)
            {
                members = parseModel(type.GetMembers());
                Cache.ModelCache.Add(type, members);
            }
            foreach (var item in members)
            {
                dynamic value = model.GetType().GetProperty(item.Key).GetValue(model, null);
                switch (value.GetType().FullName)
                {
                    case "System.Byte":
                        break;
                    case "System.Int16":
                        break;
                    case "System.Int32":
                        break;
                    case "System.Int64":
                        break;
                    case "System.SByte":
                        break;
                    case "System.UInt16":
                        break;
                    case "System.UInt32":
                        break;
                    case "System.UInt64":
                        break;
                    case "System.Double":
                        break;
                    case "System.Single"://float
                        break;
                    case "System.String":
                        break;
                    case "System.Boolean":
                        break;
                    case "System.TimeSpan":
                        break;
                    case "System.Decimal":
                        value = (double)value;
                        break;
                    case "System.DateTime":
                        value = value.ToString("yyyy-MM-dd HH:mm:ss:fff");
                        break;
                    default:
                        continue;
                        //throw new NotSupportedException("存在不支持的类型!");
                }
                _redis.DB().HashSet(key, item.Key, value);

            }
        }

        /// <summary>
        /// 先把用的字段找出来
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        private Dictionary<string, MemberInfo> parseModel(MemberInfo[] members)
        {
            Dictionary<string, MemberInfo> result = new Dictionary<string, MemberInfo>();

            foreach (var member in members)
            {
                if (member.Name.StartsWith("set_"))
                {
                    result.Add(member.Name.Replace("set_", ""), member);
                }
                continue;
            }

            return result;
        }
    }
}
