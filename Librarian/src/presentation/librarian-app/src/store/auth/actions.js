import * as types from "./action-types";
import {
    loginUser,
    isLocalStorageTokenValid,
    getToken
} from "@/auth/authService";

export async function loginUserAction({ commit }, payload) {
    try {
        const { data } = await loginUser(payload);
        console.log(data);
        commit(types.LOGIN_USER, data.token);
    } catch (e) {
        console.log(e);
    }
}

export function useLocalTokenToLogInAction({ commit }) {
    if (!isLocalStorageTokenValid()) {
      return;
    }  
    const token = getToken();
    commit(types.LOCAL_STORAGE_TOKEN_LOG_IN, token);
  }