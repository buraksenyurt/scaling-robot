import api from "@/api/api-config";

export async function getBooksAxios() {
  return await api.get(`Books/`);
}