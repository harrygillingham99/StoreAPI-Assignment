using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using store_api.Objects;
using store_api.Objects.Helpers;
using Value = Google.Cloud.Datastore.V1.Value;

namespace store_api.CloudDatastore.DAL
{
    public abstract class Repository
    {
        private readonly ILogger<Repository> _logger;
        private readonly DatastoreDb _db;

        protected Repository(ILogger<Repository> logger)
        {
            _logger = logger;
            _db = DatastoreDb.Create("e-commerce-assignment-295115");
        }

        protected async Task<List<Entity>> Get(DbKinds.DbCollections kind)
        {
            try
            {
                Query query = new Query(kind.GetDescription());
                return (await _db.RunQueryAsync(query)).Entities.ToList();
            }
            catch (Exception ex)
            {
                var exceptionMsg = $"{GetType().FullName}.Do experienced a {ex.GetType()}";
                _logger.LogError(ex, exceptionMsg);
                throw new Exception(exceptionMsg, ex);
            }
        }

        protected async Task Delete(Key key)
        {
            try
            {
                await _db.DeleteAsync(key);
            }
            catch (Exception ex)
            {
                var exceptionMsg = $"{GetType().FullName}.Do experienced a {ex.GetType()}";
                _logger.LogError(ex, exceptionMsg);
                throw new Exception(exceptionMsg, ex);
            }
        }

        protected async Task<bool> Insert<T>(T item, DbKinds.DbCollections kind)
        {
            try
            {
                KeyFactory keyFactory = _db.CreateKeyFactory(kind.GetDescription());
                using var tran = await _db.BeginTransactionAsync();
                var entityToInsert = new Entity
                {
                    Key = keyFactory.CreateIncompleteKey()
                };
                
                foreach (PropertyInfo propertyInfo in item.GetType().GetProperties().Where(x=> x.Name != "DataStoreId"))
                {
                    entityToInsert.Properties.Add(propertyInfo.Name, GetValueForItem(propertyInfo, item));
                }

                tran.Insert(entityToInsert);

                return (await tran.CommitAsync()).IndexUpdates > 0;
            }
            catch (Exception ex)
            {
                var exceptionMsg = $"{GetType().FullName}.ExecuteFunc experienced a {ex.GetType()}";
                _logger.LogError(ex, exceptionMsg);
                throw new Exception(exceptionMsg, ex);
            }
        }

        private Value GetValueForItem<T>(PropertyInfo propertyInfo, T item)
        {
            var propertyValue = propertyInfo?.GetValue(item)?.ToString();

            if (propertyValue == null)
            {
                return new Value
                {
                    NullValue = NullValue.NullValue
                };
            }
            if (int.TryParse(propertyValue, out var integerResult))
            {
                return new Value
                {
                    IntegerValue = integerResult
                };
            }

            if (double.TryParse(propertyValue, out var doubleResult))
            {
                return new Value
                {
                    DoubleValue = doubleResult
                };
            }
            if (bool.TryParse(propertyInfo.GetValue(item).ToString(), out var booleanResult))
            {
                return new Value
                {
                    BooleanValue = booleanResult
                };
            }

            if (DateTime.TryParse(propertyValue, out var dateTimeResult))
            {
                return new Value
                {
                    TimestampValue = new Timestamp(Timestamp.FromDateTime(dateTimeResult.ToUniversalTime()))
                };

            }

            return new Value
            {
                StringValue = propertyValue
            };
        }
    }
}