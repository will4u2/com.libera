using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.libera.core
{
    public abstract class BaseClass
    {
        [Key]
        public long ID { get; set; }
        public bool Active { get; set; }
        public long InUser { get; set; }
        public DateTime InDate { get; set; }
        public long InApplication { get; set; }
        public long? ModificationUser { get; set; }
        public DateTime? ModificationDate { get; set; }
        public long? ModificationApplication { get; set; }
        public long? DeleteUser { get; set; }
        public DateTime? DeleteDate { get; set; }
        public long? DeleteApplication { get; set; }
        public static T CreateInstance<T>(long creatorId, long applicationId) where T : BaseClass
        {
            T newObj = Activator.CreateInstance<T>();
            newObj.ID = -1;
            newObj.Active = true;
            newObj.InUser = creatorId;
            newObj.InApplication = applicationId;
            newObj.InDate = DateTime.Now;
            return newObj;
        }
        public static T CreateInstance<T>(long creatorId, long modUserId, DateTime modDate, long applicationId) where T : BaseClass
        {
            T newObj = Activator.CreateInstance<T>();
            newObj.ID = -1;
            newObj.Active = true;
            newObj.InUser = creatorId;
            newObj.InApplication = applicationId;
            newObj.InDate = DateTime.Now;
            newObj.ModificationUser = modUserId;
            newObj.ModificationApplication = applicationId;
            newObj.ModificationDate = modDate;
            return newObj;
        }
    }
}
