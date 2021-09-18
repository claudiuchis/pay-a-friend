using Pay.Common;

namespace Pay.Identity
{
    public static class ProjectionMapping
    {
        public static void MapProjections()
        {
            EsProjectionMap.AddProjection(new Projection(
                "user-details", 
                0,
                @"
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
                "user-registrations",
                0,
                @"
                    fromAll().when( {
                        'UserRegistered' : function(s,e) {linkTo(""user-registrations"", e)},
                        })            
                ")
            );
        }
    }
}