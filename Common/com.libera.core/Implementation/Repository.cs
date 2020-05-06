using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Linq;
using Newtonsoft.Json;
using System.Data.SQLite;

namespace com.libera.core
{
    public partial class Repository : IRepository
    {
        private readonly ILogger logger;
        public readonly IDbConnection conn;

        public bool SoftDelete { get; set; }
        public IDbConnection Connection { 
            get { return conn; } 
            set { } 
        }

        public Repository(ILogger<Repository> _logger, IDbConnection _conn)
        {
            logger = _logger;
            conn = _conn;
            SoftDelete = false;
        }

        public IStandardReply<T> Delete<T>(long id, long userId, long applicationId) where T : BaseClass
        {
            IStandardReply<T> reply = StandardReply<T>.CreateInstance();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                var deleteObject = conn.Get<T>(id);
                deleteObject.DeleteApplication = applicationId;
                deleteObject.DeleteDate = DateTime.Now;
                deleteObject.DeleteUser = userId;
                if (SoftDelete)
                {
                    conn.Update<T>(deleteObject);
                }
                else
                {
                    conn.Delete<T>(deleteObject);
                }
                reply.Response = deleteObject;
                reply.Messages.Add($"Deletion for id: {id} was successful.");
            }
            catch (Exception exc)
            {
                reply.ProcessException(exc, new { id, userId, applicationId }, logger, MethodBase.GetCurrentMethod().Name, false);
            }
            return reply;
        }

        public async Task<IStandardReply<T>> DeleteAsync<T>(long id, long userId, long applicationId) where T : BaseClass
        {
            IStandardReply<T> reply = StandardReply<T>.CreateInstance();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                var deleteObject = conn.Get<T>(id);
                deleteObject.DeleteApplication = applicationId;
                deleteObject.DeleteDate = DateTime.Now;
                deleteObject.DeleteUser = userId;
                if (SoftDelete)
                {
                    await conn.UpdateAsync<T>(deleteObject);
                }
                else
                {
                    await conn.DeleteAsync<T>(deleteObject);
                }
                reply.Response = deleteObject;
                reply.Messages.Add($"Deletion for id: {id} was successful.");
            }
            catch (Exception exc)
            {
                reply.ProcessException(exc, new { id, userId, applicationId }, logger, MethodBase.GetCurrentMethod().Name, false);
            }
            return reply;
        }

        public async Task<IStandardReply<T>> DeleteAsync<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass
        {
            IStandardReply<T> reply = StandardReply<T>.CreateInstance();
            long id = (await GetSingleAsync<T>(filter, userId, applicationId)).Response.ID;
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                var deleteObject = conn.Get<T>(id);
                deleteObject.DeleteApplication = applicationId;
                deleteObject.DeleteDate = DateTime.Now;
                deleteObject.DeleteUser = userId;
                if (SoftDelete)
                {
                    await conn.UpdateAsync<T>(deleteObject);
                }
                else
                {
                    await conn.DeleteAsync<T>(deleteObject);
                }
                reply.Response = deleteObject;
                reply.Messages.Add($"Deletion for id: {id} was successful.");
            }
            catch (Exception exc)
            {
                reply.ProcessException(exc, new { id, userId, applicationId }, logger, MethodBase.GetCurrentMethod().Name, false);
            }
            return reply;
        }

        public IStandardReply<IEnumerable<T>> GetEnumerable<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass
        {
            IStandardReply<IEnumerable<T>> reply = StandardReply<IEnumerable<T>>.CreateInstance();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                if (filter != null)
                {
                    reply.Response = conn.GetAll<T>().Where(filter).ToList();
                }
                else
                {
                    reply.Response = conn.GetAll<T>().ToList();
                }
                reply.Messages.Add($"Retrival of type {typeof(T).Name} was successful.");
            }
            catch (Exception exc)
            {
                reply.ProcessException(exc, new { type = typeof(T).Name }, logger, MethodBase.GetCurrentMethod().Name, false);
            }
            return reply;
        }

        public async Task<IStandardReply<IEnumerable<T>>> GetEnumerableAsync<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass
        {
            IStandardReply<IEnumerable<T>> reply = StandardReply<IEnumerable<T>>.CreateInstance();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                if (filter != null)
                {
                    reply.Response = (await conn.GetAllAsync<T>()).Where(filter).ToList();
                }
                else
                {
                    reply.Response = (await conn.GetAllAsync<T>()).ToList();
                }
                reply.Messages.Add($"Retrival of type {typeof(T).Name} was successful.");
            }
            catch (Exception exc)
            {
                reply.ProcessException(exc, new { type = typeof(T).Name }, logger, MethodBase.GetCurrentMethod().Name, false);
            }
            return reply;
        }

        public IStandardReply<T> GetSingle<T>(long id, long userId, long applicationId) where T : BaseClass
        {
            IStandardReply<T> reply = StandardReply<T>.CreateInstance();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                reply.Response = conn.Get<T>(id);
                reply.Messages.Add($"Retrival for id: {id} was successful.");
            }
            catch (Exception exc)
            {
                reply.ProcessException(exc, new { id, userId, applicationId }, logger, MethodBase.GetCurrentMethod().Name, false);
            }
            return reply;
        }

        public async Task<IStandardReply<T>> GetSingleAsync<T>(long id, long userId, long applicationId) where T : BaseClass
        {
            IStandardReply<T> reply = StandardReply<T>.CreateInstance();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                reply.Response = await conn.GetAsync<T>(id);
                reply.Messages.Add($"Retrival for id: {id} was successful.");
            }
            catch (Exception exc)
            {
                reply.ProcessException(exc, new { id, userId, applicationId }, logger, MethodBase.GetCurrentMethod().Name, false);
            }
            return reply;
        }

        public IStandardReply<T> GetSingle<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass
        {
            IStandardReply<T> reply = StandardReply<T>.CreateInstance();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                reply.Response = conn.GetAll<T>().Where(filter).FirstOrDefault();
                reply.Messages.Add($"Retrival for id: {filter} was successful.");
            }
            catch (Exception exc)
            {
                reply.ProcessException(exc, new { filter, userId, applicationId }, logger, MethodBase.GetCurrentMethod().Name, false);
            }
            return reply;
        }

        public async Task<IStandardReply<T>> GetSingleAsync<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass
        {
            IStandardReply<T> reply = StandardReply<T>.CreateInstance();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                reply.Response = (await conn.GetAllAsync<T>()).Where(filter).FirstOrDefault();
                reply.Messages.Add($"Retrival for id: {filter} was successful.");
            }
            catch (Exception exc)
            {
                reply.ProcessException(exc, new { filter, userId, applicationId }, logger, MethodBase.GetCurrentMethod().Name, false);
            }
            return reply;
        }

        public IStandardReply<T> Save<T>(T data, long userId, long applicationId, Func<T, bool> dupeCheck = null) where T : BaseClass
        {
            data.InUser = data.InUser == 0 ? userId : data.InUser;
            data.InApplication = data.InApplication == 0 ? applicationId : data.InApplication;
            data.InDate = data.InUser == 0 ? DateTime.Now : data.InDate;

            IStandardReply<T> reply = StandardReply<T>.CreateInstance();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                if (data.ID > 0)
                {
                    data.ModificationApplication = applicationId;
                    data.ModificationDate = DateTime.Now;
                    data.ModificationUser = userId;
                    var updateResult = conn.Update<T>(data);
                    reply.Response = data;
                }
                else
                {
                    if (dupeCheck != null)
                    {
                        var localData = conn.GetAll<T>().FirstOrDefault(dupeCheck);
                        if (localData != null)
                        {
                            data.InDate = localData.InDate;
                            data.InApplication = localData.InApplication;
                            data.InUser = localData.InUser;
                            data.ModificationApplication = applicationId;
                            data.ModificationDate = DateTime.Now;
                            data.ModificationUser = userId;
                            data.ID = localData.ID;
                            var updateResult = conn.Update<T>(data);
                            reply.Response = data;
                        }
                        else
                        {
                            var insertResult = conn.Insert<T>(data);
                            data.ID = insertResult;
                            reply.Response = data;
                        }
                    }
                    else
                    {
                        var insertResult = conn.Insert<T>(data);
                        data.ID = insertResult;
                        reply.Response = data;
                    }
                }
                reply.Messages.Add($"Save for type: {typeof(T).Name} was successful.");
            }
            catch (Exception exc)
            {
                reply.ProcessException(exc, data, logger, MethodBase.GetCurrentMethod().Name, false);
            }
            return reply;
        }

        public async Task<IStandardReply<T>> SaveAsync<T>(T data, long userId, long applicationId, Func<T, bool> dupeCheck = null) where T : BaseClass
        {
            data.InUser = data.InUser == 0 ? userId : data.InUser;
            data.InApplication = data.InApplication == 0 ? applicationId : data.InApplication;
            data.InDate = data.InUser == 0 ? DateTime.Now : data.InDate;

            IStandardReply<T> reply = StandardReply<T>.CreateInstance();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                if (data.ID > 0)
                {
                    data.ModificationApplication = applicationId;
                    data.ModificationDate = DateTime.Now;
                    data.ModificationUser = userId;
                    var updateResult = await conn.UpdateAsync<T>(data);
                    if (!updateResult)
                    {
                        throw new Exception($"Update failed for {JsonConvert.SerializeObject(data)}");
                    }
                    reply.Response = data;
                }
                else
                {
                    if (dupeCheck != null)
                    {
                        var localData = conn.GetAll<T>().FirstOrDefault(dupeCheck);
                        if (localData != null)
                        {
                            data.InDate = localData.InDate;
                            data.InApplication = localData.InApplication;
                            data.InUser = localData.InUser;
                            data.ModificationApplication = applicationId;
                            data.ModificationDate = DateTime.Now;
                            data.ModificationUser = userId;
                            data.ID = localData.ID;
                            var updateResult = await conn.UpdateAsync<T>(data);
                            reply.Response = data;
                            if (!updateResult)
                            {
                                throw new Exception($"Update failed for {JsonConvert.SerializeObject(data)}");
                            }
                        }
                        else
                        {
                            var insertResult = await conn.InsertAsync<T>(data);
                            data.ID = insertResult;
                            reply.Response = data;
                            if (insertResult < 1)
                            {
                                throw new Exception($"Insert failed for {JsonConvert.SerializeObject(data)}");
                            }
                        }
                    }
                    else
                    {
                        var insertResult = await conn.InsertAsync<T>(data);
                        data.ID = insertResult;
                        reply.Response = data;
                        if (insertResult < 1)
                        {
                            throw new Exception($"Insert failed for {JsonConvert.SerializeObject(data)}");
                        }
                    }
                }
                reply.Messages.Add($"Save for type: {typeof(T).Name} was successful.");
            }
            catch (Exception exc)
            {
                reply.ProcessException(exc, data, logger, MethodBase.GetCurrentMethod().Name, false);
            }
            return reply;
        }

        public IStandardReply<IEnumerable<T>> Search<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass
        {
            IStandardReply<IEnumerable<T>> reply = StandardReply<IEnumerable<T>>.CreateInstance();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                reply.Response = conn.GetAll<T>().Where(filter).ToList();
                reply.Messages.Add($"Retrival of type {typeof(T).Name} was successful.");
            }
            catch (Exception exc)
            {
                reply.ProcessException(exc, new { type = typeof(T).Name }, logger, MethodBase.GetCurrentMethod().Name, false);
            }
            return reply;
        }

        public async Task<IStandardReply<IEnumerable<T>>> SearchAsync<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass
        {
            IStandardReply<IEnumerable<T>> reply = StandardReply<IEnumerable<T>>.CreateInstance();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                reply.Response = (await conn.GetAllAsync<T>()).Where(filter).ToList();
                reply.Messages.Add($"Retrival of type {typeof(T).Name} was successful.");
            }
            catch (Exception exc)
            {
                reply.ProcessException(exc, new { type = typeof(T).Name }, logger, MethodBase.GetCurrentMethod().Name, false);
            }
            return reply;
        }

        public IStandardReply<IEnumerable<T>> GetEnumerable<T>(IRequestObject request)
        {
            logger.LogInformation("{0} started.", MethodBase.GetCurrentMethod().Name);
            IStandardReply<IEnumerable<T>> reply = StandardReply<IEnumerable<T>>.CreateInstance("Get successful.");
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                reply.Response = conn.Query<T>(request.SqlStatement, request.Data, request.Transaction, request.Buffered, request.Timeout, request.CommandType);
            }
            catch (Exception exc)
            {
                reply.ProcessException(exc, request, logger, MethodBase.GetCurrentMethod().Name);
            }
            logger.LogInformation("{0} finished.", MethodBase.GetCurrentMethod().Name);
            return reply;
        }

        public async Task<IStandardReply<IEnumerable<T>>> GetEnumerableAsync<T>(IRequestObject request)
        {
            logger.LogInformation("{0} started.", MethodBase.GetCurrentMethod().Name);
            IStandardReply<IEnumerable<T>> reply = StandardReply<IEnumerable<T>>.CreateInstance("Get successful.");
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                reply.Response = (await conn.QueryAsync<T>(request.SqlStatement, request.Data, request.Transaction, request.Timeout, request.CommandType));
            }
            catch (Exception exc)
            {
                reply.ProcessException(exc, request, logger, MethodBase.GetCurrentMethod().Name);
            }
            logger.LogInformation("{0} finished.", MethodBase.GetCurrentMethod().Name);
            return reply;
        }
    }
}
