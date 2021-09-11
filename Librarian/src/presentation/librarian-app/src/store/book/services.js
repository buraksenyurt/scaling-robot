import api from "@/api/api-config";

/*
   api-config üstünden api tarafında CRUD taleplerini gönderen fonksiyonları içerir
*/
export async function getBooks() {
  return await api.get("books");
}

export async function deleteBook(id) {
  return await api.delete("books/" + id);
}
