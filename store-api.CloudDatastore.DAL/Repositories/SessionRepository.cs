using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using store_api.CloudDatastore.DAL.Interfaces;
using store_api.Objects;
using store_api.Objects.Helpers;
using store_api.Objects.InternalObjects;
using store_api.Objects.StoreObjects;
using static store_api.Objects.InternalObjects.DbKinds;

namespace store_api.CloudDatastore.DAL.Repositories
{
    public class SessionRepository : Repository, ISessionRepository
    {
        private const DbCollections Kind = DbCollections.Baskets;

        public SessionRepository(
            ILogger<ProductRepository> logger, IOptions<ConnectionStrings> connectionStrings) : base(logger, Kind, connectionStrings.Value.ProjectName)
        {
        }

        public async Task<Basket> GetCurrentBasket(string requestUid)
        {
            var result = (await GetLatestBasket(requestUid)).ToList();

            if (!result.Any()) 
                result = (await CreateNewBasket(requestUid)).ToList();

            return result.Select(entity => new Basket
            {
                DataStoreId = entity.Key.ToId(),
                HasPlacedOrder = entity.Properties["HasPlacedOrder"].BooleanValue,
                SelectedProducts = entity.Properties["SelectedProducts"].ArrayValue?.Values
                    ?.Select(x => (int) x.IntegerValue).ToList(),
                UserUid = entity.Properties["UserUid"].StringValue,
                DateOrdered = null
            }).FirstOrDefault();
        }

        public Task<bool> UpdateBasket(Basket basketToUpdate)
        {
            return Update(basketToUpdate, basketToUpdate.DataStoreId.ToKey(Kind));
        }

        public Task<bool> OrderItems(Basket basketToOrder)
        {
            basketToOrder.HasPlacedOrder = true;
            basketToOrder.DateOrdered = DateTime.Now;

            return Update(basketToOrder, basketToOrder.DataStoreId.ToKey(Kind));
        }

        public async Task<IEnumerable<Basket>> GetHistoricOrders(string requestUid)
        {
            var result = await Get(Filter.And(Filter.Equal("HasPlacedOrder", true),
                Filter.Equal("UserUid", new Value {StringValue = requestUid})));
            return result.Select(entity => new Basket
            {
                DataStoreId = entity.Key.ToId(),
                HasPlacedOrder = entity.Properties["HasPlacedOrder"].BooleanValue,
                SelectedProducts = entity.Properties["SelectedProducts"].ArrayValue?.Values
                    ?.Select(x => (int) x.IntegerValue).ToList(),
                UserUid = entity.Properties["UserUid"].StringValue,
                DateOrdered = entity.Properties["DateOrdered"].TimestampValue.ToDateTime()
            });
        }

        private async Task<IEnumerable<Entity>> CreateNewBasket(string requestUid)
        {
            await Insert(new Basket {UserUid = requestUid, DateOrdered = null, SelectedProducts = null, HasPlacedOrder = false});

            return await GetLatestBasket(requestUid);
        }

        private async Task<IEnumerable<Entity>> GetLatestBasket(string requestUid)
        {
            return await Get(Filter.And(Filter.Equal("HasPlacedOrder", false),Filter.Equal("UserUid", new Value {StringValue = requestUid})));
        }
    }
}