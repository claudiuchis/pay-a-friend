using Pay.Common;

namespace Pay.Identity.Infrastructure
{
    public static class ProjectionMapping
    {
        public static const string UserDetailsProjection = "user-details";
        public static const string UserRegistrationsProjection = "user-registrations";
        public static void MapProjections()
        {
            EsProjectionMap.AddProjection(new Projection(
                Name: UserDetailsProjection, 
                Version: 0,
                Query: @"
                    fromCategory('User')
                    .foreachStream()
                    .when({
                    UserRegistered: function(state, event) {
                        state.UserId = event.userId;
                        state.Email = event.email;
                        state.FullName = event.fullName;
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