import api from "@/api/api-config";

/*
    aslında api-config içerisindeki ayarları kullanarak books controller'ına bir talep gönderen fonksiyon içeriyor
*/
export async function getBooks() {
  return await api.get("books");
}