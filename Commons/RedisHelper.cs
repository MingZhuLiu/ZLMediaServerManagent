// using Microsoft.Extensions.Configuration;
// using StackExchange.Redis;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using static ZLMediaServerManagent.Models.Enums;

// namespace ZLMediaServerManagent.Commons
// {
//     public class RedisHelper
//     {
//         private ConnectionMultiplexer redis { get; set; }
//         private IDatabase db { get; set; }
//         private RedisHelper(string connection)
//         {
//             redis = ConnectionMultiplexer.Connect(connection);
//             db = redis.GetDatabase();
//         }

//         private static RedisHelper _instance = null;
//         public static RedisHelper Instance
//         {
//             get
//             {
//                 if (_instance == null)
//                 {
//                     var connStr = Startup.Instance.Configuration.GetValue<String>("RedisConnectStr");
//                     _instance = new RedisHelper(connStr);
//                 }
//                 return _instance;
//             }
//         }


//         #region 键值对

//         /// <summary>
//         /// 增加/修改
//         /// </summary>
//         /// <param name="key"></param>
//         /// <param name="value"></param>
//         /// <returns></returns>
//         public bool SetValue(string key, string value)
//         {
//             return db.StringSet(key, value);
//         }

//         /// <summary>
//         /// 查询
//         /// </summary>
//         /// <param name="key"></param>
//         /// <returns></returns>
//         public string GetValue(string key)
//         {
//             return db.StringGet(key);
//         }

//         /// <summary>
//         /// 删除
//         /// </summary>
//         /// <param name="key"></param>
//         /// <returns></returns>
//         public bool DeleteKey(string key)
//         {
//             return db.KeyDelete(key);
//         }


//         public bool DeleteKey(RedisCacheTables key)
//         {

//             return db.KeyDelete(key.ToString());
//         }


//         public bool DeleteKey<T>()
//         {
//             return db.KeyDelete(typeof(T).Name);
//         }

//         public T GetValue<T>(string key)
//         {
//             if (String.IsNullOrWhiteSpace(key))
//                 return default(T);
//             string json = GetValue(typeof(T).Name + key);
//             if (String.IsNullOrWhiteSpace(json))
//             {
//                 return default(T);
//             }
//             else
//             {
//                 return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
//             }
//         }

//         public bool SetValue<T>(string key, Object value)
//         {
//             var json = Newtonsoft.Json.JsonConvert.SerializeObject(value);
//             return db.StringSet(typeof(T).Name + key, json);
//         }


//         #endregion


//         #region Hash操作


//         public bool SetHash(string setName, long key, string value)
//         {
//             return db.HashSet(setName, key, value);
//         }
//         public bool SetHash(string setName, String key, string value)
//         {
//             return db.HashSet(setName, key, value);
//         }
//         public bool SetHash(RedisCacheTables tableName, long key, string value)
//         {
//             return db.HashSet(tableName.ToString(), key, value);
//         }
//         public bool SetHash(RedisCacheTables tableName, String key, string value)
//         {
//             return db.HashSet(tableName.ToString(), key, value);
//         }

//         public bool SetHash(RedisCacheTables tableName, String key, Object value)
//         {
//             return db.HashSet(tableName.ToString(), key, Newtonsoft.Json.JsonConvert.SerializeObject(value));
//         }
//         public bool SetHash(RedisCacheTables tableName, long key, Object value)
//         {
//             return db.HashSet(tableName.ToString(), key, Newtonsoft.Json.JsonConvert.SerializeObject(value));
//         }

//         public bool DeleteHash(string setName, string key)
//         {
//             return db.HashDelete(setName, key);
//         }

//         public bool DeleteHash(RedisCacheTables setName, string key)
//         {
//             return db.HashDelete(setName.ToString(), key);
//         }


//         public bool DeleteHash(string setName, long key)
//         {
//             return db.HashDelete(setName, key);
//         }

//         public bool DeleteHash(RedisCacheTables setName, long key)
//         {
//             return db.HashDelete(setName.ToString(), key);
//         }

//         public List<String> GetHashAll<T>(string tableName = null)
//         {
//             var datas = db.HashGetAll(tableName == null ? typeof(T).Name : tableName);
//             if (datas == null)
//                 return null;
//             else
//             {
//                 List<String> result = new List<string>();
//                 foreach (var item in datas)
//                 {
//                     result.Add(item.Value);
//                 }
//                 return result;
//             }
//         }

//         public List<T> GetHashAllObj<T>(string tableName = null)
//         {
//             var list = !String.IsNullOrWhiteSpace(tableName) ? GetHashAll<T>(tableName) : GetHashAll<T>(typeof(T).Name);
//             if (list == null)
//                 return default(List<T>);
//             else
//             {
//                 var result = new List<T>();

//                 foreach (var item in list)
//                 {
//                     try
//                     {
//                         result.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<T>(item));
//                     }
//                     catch (Exception ex)
//                     {
//                         Console.WriteLine(ex.Message);
//                     }
//                 }
//                 return result;
//             }
//         }

//         public List<T> GetHashAllObj<T>(RedisCacheTables tableName)
//         {
//             return GetHashAllObj<T>(tableName.ToString());
//         }

//         public String GetHash(string setName, String key)
//         {
//             return db.HashGet(setName, key);
//         }
//         public String GetHash(string setName, long key)
//         {
//             return db.HashGet(setName, key);
//         }
//         public T GetHash<T>(string tableName, String key)
//         {
//             var json = GetHash(typeof(T).Name, key);
//             if (String.IsNullOrWhiteSpace(json))
//             {
//                 return default(T);
//             }
//             else
//             {
//                 if (typeof(T) == typeof(String))
//                     return (T)(Object)json;
//                 else
//                     return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
//             }
//         }
//         public T GetHash<T>( long key)
//         {
//             var json = GetHash(typeof(T).Name, key);
//             if (String.IsNullOrWhiteSpace(json))
//             {
//                 return default(T);
//             }
//             else
//             {
//                 if (typeof(T) == typeof(String))
//                     return (T)(Object)json;
//                 else
//                     return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
//             }
//         }

//         public T GetHash<T>(RedisCacheTables tableName, String key)
//         {
//             var data = GetHash(tableName.ToString(), key);
//             if (data == null)
//             {
//                 return default(T);
//             }
//             else
//             {
//                 if (typeof(T) == typeof(String))
//                     return (T)(Object)data;
//                 else
//                     return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);
//             }
//         }

//         public T GetHash<T>(RedisCacheTables tableName, long key)
//         {
//             var data = GetHash(tableName.ToString(), key);
//             if (data == null)
//             {
//                 return default(T);
//             }
//             else
//             {
//                 if (typeof(T) == typeof(String))
//                     return (T)(Object)data;
//                 else
//                     return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);
//             }
//         }

//         public T GetHash<T>(String key)
//         {
//             var json = GetHash(typeof(T).Name, key);
//             if (String.IsNullOrWhiteSpace(json))
//             {
//                 return default(T);
//             }
//             else
//             {
//                 return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
//             }
//         }

//         public long GetHashCount<T>()
//         {
//             return db.HashLength(typeof(T).Name);
//         }

//         //public long GetHashCount(RedisCacheTables tableName)
//         //{
//         //    return db.HashLength(tableName.ToString());
//         //}

//         public bool SetHash<T>(String key, Object value)
//         {

//             return SetHash(typeof(T).Name, key, Newtonsoft.Json.JsonConvert.SerializeObject(value));
//         }
//         //public bool SetHash(RedisCacheTables tableName, String key, Object value)
//         //{
//         //    return SetHash(tableName.ToString(), key, Newtonsoft.Json.JsonConvert.SerializeObject(value));
//         //}

//         public bool SetHash(String tableName, String key, Object value)
//         {
//             if (value.GetType() != typeof(String))
//                 value = Newtonsoft.Json.JsonConvert.SerializeObject(value);
//             return SetHash(tableName, key, value);
//         }


//         public bool DeleteHash<T>(String key)
//         {
//             return DeleteHash(typeof(T).Name, key);
//         }


//         #endregion


//         #region Set
//         public bool AddSet(string key, string value)
//         {
//             return db.SetAdd(key, value);
//         }

//         public bool AddSet(string key, Object value)
//         {
//             return AddSet(key, Newtonsoft.Json.JsonConvert.SerializeObject(value));
//         }
//         public bool AddSet<T>(Object value)
//         {
//             return AddSet(typeof(T).Name, Newtonsoft.Json.JsonConvert.SerializeObject(value));
//         }

//         public List<String> GetSetAll(string tableName)
//         {
//             List<String> result = new List<string>();
//             var temp = db.SetMembers(tableName);
//             if (temp != null)
//             {
//                 foreach (var item in temp)
//                 {
//                     result.Add(item);
//                 }
//             }
//             return result;
//         }

//         public List<T> GetSetAll<T>(string tableName)
//         {
//             List<T> result = new List<T>();
//             var temp = db.SetMembers(tableName);
//             if (temp != null)
//             {
//                 foreach (var item in temp)
//                 {
//                     result.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<T>(item));
//                 }
//             }
//             return result;
//         }

//         public List<T> GetSetAll<T>()
//         {
//             List<T> result = new List<T>();
//             var temp = db.SetMembers(typeof(T).Name);
//             if (temp != null)
//             {
//                 foreach (var item in temp)
//                 {
//                     result.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<T>(item));
//                 }
//             }
//             return result;
//         }

//         #endregion

//     }

// }
