using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Bson;

using Eventuous.Projections.MongoDB;
using static Pay.Identity.Projections.ReadModels;
using static Pay.Identity.Domain.Users.Events;

namespace Pay.Identity.Projections
{
    public class UserDetailsProjection : MongoProjection<UserDetails>
    {
        public UserDetailsProjection(IMongoDatabase database, string subscriptionGroup, ILoggerFactory loggerFactory)
            : base(database, subscriptionGroup, loggerFactory) {
            }

        protected override ValueTask<Operation<UserDetails>> GetUpdate(object @event, long? position)
        {
            return @event switch
            {
                V1.UserRegistered e => new(new CollectionOperation<UserDetails>( (collection, cancelationToken) => collection.InsertOneAsync(new UserDetails(
                    e.UserId, e.Email, e.EncryptedPassword, e.FullName), null, cancelationToken))),
                _ => NoOp
            };   
        }

    }
}