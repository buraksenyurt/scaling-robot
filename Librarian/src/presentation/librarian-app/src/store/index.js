import Vue from "vue";
import Vuex from "vuex";
import createLogger from "vuex/dist/logger";
import bookModule from "./book";
import authModule from "./auth";

Vue.use(Vuex);

const debug = process.env.NODE_ENV !== "production";
const plugins = debug ? [createLogger({})] : [];

export default new Vuex.Store({
  modules: {
    bookModule,
    authModule
  },
  plugins
});