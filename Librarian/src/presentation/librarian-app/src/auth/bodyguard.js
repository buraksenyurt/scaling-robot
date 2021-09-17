import store from "@/store";

/*
    Araya girdiğimiz bir middleware olarak düşünebiliriz.
    Bir doğrulama gerekiyorsa ve doğrulama yapılmış dolayısıyla isAuthenticated değeri true ise
    akışta bir sonraki fonksiyonla devam ediliyor. Aksi durumda login sayfasına yönlendiriliyor.
*/
export const agentScally = (to, from, next) => {
    const authRequired = to.matched.some((rec) => rec.meta.requiresAuth);
    if (authRequired) {
        if (store.getters["authModule/isAuthenticated"]) {
            next();
            return;
        }
        next("/login");
    }
    next();
};