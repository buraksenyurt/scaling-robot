import api from "@/api/api-config";
import * as jwt from "jsonwebtoken";
const key = "token";
export async function loginUser(login) {
    return await api.post("user/auth", login); // Web API tarafındaki UserController'ın Authenticate fonksiyonuna talep gönderir.
}

export function isLocalStorageTokenValid() {
    const token = localStorage.getItem(key);
    console.log("Token:",token);
    if (!token) {
        return false;
    }

    const decoded = jwt.decode(token);
    const expiresAt = decoded.exp * 1000;
    const dateNow = Date.now();

    return dateNow <= expiresAt;
}

export function getUsernameFromToken() {
    const token = localStorage.getItem(key);
    if (!token) return false;
  
    const decoded = jwt.decode(token);
    console.log("Decoded:",decoded);
    /* username bilgisini token içerisindeki claim set'ten alıyoruz. 
    Yani Librarian.Idendity projesindeki UserService sınfının GenerateJwtToken metodundaki SecurityTokenDescriptor nesnesinin Subject özelliğinde
    token ile istemciye aktarılmasını istediğimiz bilgiler varsa(username, email gibi) eklemeliyiz.
    */
    console.log("Username:",decoded.username);
    return decoded.username;
  }

export function getToken(){
    return  localStorage.getItem(key);
}

export function logout(){
    localStorage.clear();
    window.location="/login";
}