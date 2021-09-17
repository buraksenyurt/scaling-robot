import api from "@/api/api-config";

export async function loginUser(login){
    return await api.post("user/auth",login); // Web API tarafındaki UserController'ın Authenticate fonksiyonuna talep gönderir.
}