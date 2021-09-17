import * as types from "./action-types";
import {
    loginUser
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