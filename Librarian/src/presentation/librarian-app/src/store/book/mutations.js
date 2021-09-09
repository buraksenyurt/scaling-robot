import * as actionTypes from "./action-types";

const mutations = {
  [actionTypes.GET_BOOKS](state, lists) {
    state.lists = lists;
  },

  [actionTypes.LOADING_BOOKS](state, value) {
    state.loading = value;
  }
};

export default mutations;