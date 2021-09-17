const getters = {
    username: state => {
      return state.signInState.username;
    },
    isAuthenticated: state => {
      return state.signInState.token;
    }
  };
  export default getters;