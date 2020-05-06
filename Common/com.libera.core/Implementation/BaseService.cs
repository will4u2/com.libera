using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace com.libera.core
{
    public abstract class BaseService : IRepository
    {
        public readonly IRepository repo;

        public BaseService(IRepository _repo)
        {
            repo = _repo;
        }

        public IDbConnection Connection { get { return repo.Connection; } set { } }
        public bool SoftDelete { get { return repo.SoftDelete; } set { } }

        public IStandardReply<T> Delete<T>(long id, long userId, long applicationId) where T : BaseClass
        {
            return repo.Delete<T>(id, userId, applicationId);
        }

        public async Task<IStandardReply<T>> DeleteAsync<T>(long id, long userId, long applicationId) where T : BaseClass
        {
            return await repo.DeleteAsync<T>(id, userId, applicationId);
        }

        public async Task<IStandardReply<T>> DeleteAsync<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass
        {
            var item = await repo.GetSingleAsync(filter, userId, applicationId);
            return await DeleteAsync<T>(item.Response.ID, userId, applicationId);
        }

        public IStandardReply<IEnumerable<T>> GetEnumerable<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass
        {
            return repo.GetEnumerable<T>(filter, userId, applicationId);
        }

        public async Task<IStandardReply<IEnumerable<T>>> GetEnumerableAsync<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass
        {
            return await repo.GetEnumerableAsync<T>(filter, userId, applicationId);
        }

        public async Task<IStandardReply<IEnumerable<T>>> GetEnumerableAsync<T>(IRequestObject request)
        {
            return await repo.GetEnumerableAsync<T>(request);
        }

        public IStandardReply<T> GetSingle<T>(long id, long userId, long applicationId) where T : BaseClass
        {
            return GetSingle<T>(id, userId, applicationId);
        }

        public IStandardReply<T> GetSingle<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass
        {
            return GetSingle<T>(filter, userId, applicationId);
        }

        public async Task<IStandardReply<T>> GetSingleAsync<T>(long id, long userId, long applicationId) where T : BaseClass
        {
            return await repo.GetSingleAsync<T>(id, userId, applicationId);
        }

        public async Task<IStandardReply<T>> GetSingleAsync<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass
        {
            return await repo.GetSingleAsync<T>(filter, userId, applicationId);
        }

        public IStandardReply<T> Save<T>(T data, long userId, long applicationId, Func<T, bool> dupeCheck = null) where T : BaseClass
        {
            return repo.Save<T>(data, userId, applicationId, dupeCheck);
        }

        public async Task<IStandardReply<T>> SaveAsync<T>(T data, long userId, long applicationId, Func<T, bool> dupeCheck = null) where T : BaseClass
        {
            return await repo.SaveAsync<T>(data, userId, applicationId, dupeCheck);
        }

        public IStandardReply<IEnumerable<T>> Search<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass
        {
            return repo.Search<T>(filter, userId, applicationId);
        }

        public async Task<IStandardReply<IEnumerable<T>>> SearchAsync<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass
        {
            return await repo.SearchAsync<T>(filter, userId, applicationId);
        }
    }
}
