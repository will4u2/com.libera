using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace com.libera.core
{
    public interface IRepository
    {
        IDbConnection Connection { get; set; }
        bool SoftDelete { get; set; }
        IStandardReply<T> GetSingle<T>(long id, long userId, long applicationId) where T : BaseClass;
        Task<IStandardReply<T>> GetSingleAsync<T>(long id, long userId, long applicationId) where T : BaseClass;
        IStandardReply<T> GetSingle<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass;
        Task<IStandardReply<T>> GetSingleAsync<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass;
        IStandardReply<IEnumerable<T>> GetEnumerable<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass;
        Task<IStandardReply<IEnumerable<T>>> GetEnumerableAsync<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass;
        IStandardReply<IEnumerable<T>> Search<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass;
        Task<IStandardReply<IEnumerable<T>>> SearchAsync<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass;
        IStandardReply<T> Save<T>(T data, long userId, long applicationId, Func<T, bool> dupeCheck = null) where T : BaseClass;
        Task<IStandardReply<T>> SaveAsync<T>(T data, long userId, long applicationId, Func<T, bool> dupeCheck = null) where T : BaseClass;
        IStandardReply<T> Delete<T>(long id, long userId, long applicationId) where T : BaseClass;
        Task<IStandardReply<T>> DeleteAsync<T>(long id, long userId, long applicationId) where T : BaseClass;
        Task<IStandardReply<T>> DeleteAsync<T>(Func<T, bool> filter, long userId, long applicationId) where T : BaseClass;
        Task<IStandardReply<IEnumerable<T>>> GetEnumerableAsync<T>(IRequestObject request);
    }
}
