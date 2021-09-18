import store from "@/store";
import { isLocalStorageTokenValid } from "./authService";

/*
    Araya girdiğimiz bir middleware olarak düşünebiliriz.
    Bir doğrulama gerekiyorsa ve token local storage'da zaten var veya isAuthenticated değeri true ise akışta bir sonraki fonksiyonla devam ediliyor. 
    Aksi durumda login sayfasına yönlendiriliyor.
*/
export const agentScally = (to, from, next) => {
    //console.log("Agent Scally")
    const authRequired = to.matched.some((rec) => rec.meta.requiresAuth);

    if (authRequired) {
        if (store.getters["authModule/isAuthenticated"] || isLocalStorageTokenValid()) {
            next();
            return;
        }
        next("/login");
    }
    next();
};