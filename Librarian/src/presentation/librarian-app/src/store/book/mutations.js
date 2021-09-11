import * as actionTypes from "./action-types";

const mutations = {
    [actionTypes.GET_BOOKS](state, books) {
        state.books = books;
    },

    [actionTypes.LOADING_BOOKS](state, value) {
        state.loading = value;
    },

    [actionTypes.REMOVE_BOOK](state, id) {
        state.books = state.books.filter((tl) => tl.id !== id);
    },

    [actionTypes.ADD_BOOK](state, newBook) {
        state.books.unshift(newBook);
    }
};

export default mutations;