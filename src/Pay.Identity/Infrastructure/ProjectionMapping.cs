using Pay.Common;

namespace Pay.Identity.Infrastructure
{
    public static class ProjectionMapping
    {
        public const string UserDetailsProjection = "user-details";
        public const string UserRegistrationsProjection = "user-registrations";
        public static void MapProjections()
        {
            EsProjectionMap.AddProjection(new Projection(
                Name: UserDetailsProjection, 
                Version: 0,
                Query: @"
                    fromCategory('User')
                        .partitionBy(function(event) {
                            return event.data.email
                        })
                        .when({
                            UserRegistered: function(state, event) {
                                state.UserId = event.data.userId;
                                state.Email = event.data.email;
                                state.FullName = event.data.fullName;
                            }
                        })
                        .outputState();
                ")
            );

            EsProjectionMap.AddProjection(new Projection(
                Name: UserRegistrationsProjection,
                Version: 0,
                Query: @"
                    fromAll().when( {
                        'UserRegistered' : function(s,e) {linkTo(""user-registrations"", e)},
                        })            
                ")
            );
        }
    }
}