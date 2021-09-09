import * as actionTypes from "./action-types";
import { getBooks } from "@/store/book/services";

export async function getBooksAction({ commit }) {
    // Kitapların yüklendiğine dair bir durum bildiriyor
    commit(actionTypes.LOADING_BOOKS, true);

    try {
        // servis fonksiyonundan veri çekiliyor
        const { data } = await getBooks();
        // kitap listesinin alınması farklı bir durum ve payload olarak da listenin kendisi bildiriliyor
        commit(actionTypes.GET_BOOKS, data.bookList);
    } catch (e) {
        console.log(e);
    }
    // kitapların yüklenme durumu sona erdiği için false ile bir durum bilgilendirilmesi yapılıyor
    commit(actionTypes.LOADING_BOOKS, false);
}