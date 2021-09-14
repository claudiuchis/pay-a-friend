options({
    $includeLinks: false,
    reorderEvents: false,
    processingLag: 0
  })
  
  fromCategory('User')
  .foreachStream()
  .when({
    $init: function() {
      return {
        UserId : '',
        Email: '',
        FullName: '',
        Token: '',
        ValidTo: ''
      };
    },
    UserRegistered: function(state, event) {
      state.UserId = event.userId;
      state.Email = event.email;
      state.FullName = event.fullName;
    },
    ConfirmationEmailSent: function(state, event) {
      state.Token = event.token;
      state.ValidTo = event.validTo;
    }
  })
  .outputState();

-----------

  fromCategory('User')
  .foreachStream()
  .when({
    $init: function() {
      return {
          count : ''
      };
    },
    UserRegistered: function(state, event) {
        state.count += event.data.userId;
    },
    ConfirmationEmailSent: function(state, event) {
        state.count += '1';
    }
  })
  .outputState();