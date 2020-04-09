using System;
using System.Collections.Generic;
using System.Linq;
using Asset.Infrastructure.Common;
using Domain.Model;
using Domain.Application._App;
using Domain.Model._App;
using Domain.Application.Repository;
using Domain.Model.Entities;
using static Asset.Infrastructure.Common.GeneralEnums;
using Asset.Infrastructure._App;

namespace Domain.Application.BaseService
{
    public abstract class BaseService<T> : IBaseService<T> where T : IBaseEntity
    {
        #region Constructor

        private readonly IRepository<T> _repository;
        private readonly IRepository<ChangeLog> _logRepository;

        protected BaseService(IRepository<T> repository)
        {
            _repository = repository;
            _logRepository = ServiceLocatorAdapter.Current.GetInstance<IRepository<ChangeLog>>();
        }

        #endregion

        #region Protected

        protected void ValidateModelByAttribute(T model, Type attribute)
        {
            var mandatoryProperties = model.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, attribute));
            foreach (var item in mandatoryProperties)
            {
                if (item.GetValue(model, null) == null)
                {
                    throw new Exception($"فیلد {item.Name} تعیین نشده است.", new Exception { Source = GeneralMessages.ExceptionSource });
                }
            }
        }

        protected void ValidateInsertModel(T model)
        {
            var mandatoryProperties = model.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(InsertMandatoryField)));
            if (mandatoryProperties.Select(item => item.GetValue(model, null)).Any(value => value == null))
            {
                throw new Exception(GeneralMessages.DefectiveEntry, new Exception { Source = GeneralMessages.ExceptionSource });
            }
        }

        protected void ValidateBulkInsertModel(IList<T> model)
        {
            if ((from t in model
                 let mandatoryProperties = t.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(InsertMandatoryField)))
                 where mandatoryProperties.Select(item => item.GetValue(t, null)).Any(value => value == null)
                 select t).Any())
            {
                throw new Exception(GeneralMessages.DefectiveEntry, new Exception { Source = GeneralMessages.ExceptionSource });
            }
        }

        protected void ValidateUpdateModel(T model)
        {
            var mandatoryProperties = model.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(UpdateMandatoryField)));
            if (mandatoryProperties.Select(item => item.GetValue(model, null)).Any(value => value == null))
            {
                throw new Exception(GeneralMessages.DefectiveEntry, new Exception { Source = GeneralMessages.ExceptionSource });
            }
        }

        protected long ValidateDeleteModel(T model)
        {
            long id = 0;
            var properties = model.GetType().GetProperties();
            foreach (var item in properties)
            {
                var key = item.Name;
                var value = item.GetValue(model, null);
                if (key.Equals(nameof(Entity.Id)) && value == null)
                    throw new Exception(GeneralMessages.DefectiveEntry, new Exception { Source = GeneralMessages.ExceptionSource });
                else if (key.Equals(nameof(Entity.Id)))
                    id = Convert.ToInt64(value);
                if (key.Equals(nameof(Entity.UpdaterId)) && value == null)
                {
                    throw new Exception(GeneralMessages.DefectiveEntry, new Exception { Source = GeneralMessages.ExceptionSource });
                }
            }
            if (id <= 0)
                throw new Exception(GeneralMessages.DefectiveEntry, new Exception { Source = GeneralMessages.ExceptionSource });
            return id;
        }

        #endregion

        public virtual IEnumerable<T> GetPaging(T model, out long totalCount, string orderBy, string order, int skip, int take)
        {
            var result = QueryGenerator<T>.GetPaging(model, orderBy, order, skip, take);
            var response = _repository.Query(result.Query, result.Parameters);
            totalCount = response.Any() ? response.First().RowsCount : 0;
            return response;
        }

        public virtual IEnumerable<T> Find(T model)
        {
            return _repository.Find(model);
        }

        public virtual T Single(T model, bool additionalInfo = false)
        {
            return _repository.Single(model, additionalInfo);
        }

        public virtual T GetById(int id, bool additionalInfo)
        {
            return _repository.GetById(id);
        }

        public virtual int Insert(T model)
        {
            ValidateModelByAttribute(model, typeof(InsertMandatoryField));
            var newId = _repository.Insert(model);
            return newId;
        }

        public virtual int BulkInsert(IList<T> model)
        {
            ValidateBulkInsertModel(model);
            return _repository.BulkInsert(model);
        }

        public virtual int Update(T model)
        {
            ValidateModelByAttribute(model, typeof(UpdateMandatoryField));
            return _repository.Update(model);
        }

        public virtual int Delete(T model)
        {
            var id = ValidateDeleteModel(model);
            return _repository.Delete(new long[] { id });
        }

        public virtual int Delete(IEnumerable<long> ids)
        {
            return _repository.Delete(ids.ToArray());
        }

        public virtual int InsertWithLog(T model, int adminId, string data = "")
        {
            var newId = _repository.Insert(model);
            _logRepository.Insert(new ChangeLog { Entity = typeof(T).Name, EntityId = newId, ActionType = Convert.ToByte(ActionType.Create), AdminId = adminId, Data = data, CreatedAt = DateTime.Now });
            return newId;
        }

        public virtual int UpdateWithLog(T model, int adminId, string data = "")
        {
            ValidateModelByAttribute(model, typeof(UpdateMandatoryField));
            var id = _repository.Update(model);

            if (id > 0)
            {
                var modelId = (int)model.GetType().GetProperties().SingleOrDefault(prop => prop.Name.Equals(nameof(Entity.Id))).GetValue(model, null);
                _logRepository.Insert(new ChangeLog { Entity = typeof(T).Name, EntityId = modelId, ActionType = Convert.ToByte(ActionType.Update), AdminId = adminId, Data = data, CreatedAt = DateTime.Now });
            }

            return id;
        }
    }
}