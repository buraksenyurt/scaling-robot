import api from "@/api/api-config";

export async function loginUser(login){
    return await api.post("user/auth",login); // Web API tarafındaki UserController'ın Authenticate fonksiyonuna talep gönderir.
}

export function isTokenFromLocalStorageValid() {
    const token = localStorage.getItem(key);
    if (!token) {
      return false;
    }
  
    const decoded = jwt.decode(token);
    const expiresAt = decoded.exp * 1000;
    const dateNow = Date.now();
  
    return dateNow <= expiresAt;
  }