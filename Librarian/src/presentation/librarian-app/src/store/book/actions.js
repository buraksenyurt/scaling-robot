import * as actionTypes from "./action-types";
import { getBooks, deleteBook, addBook } from "@/store/book/services";

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

// Listeden kitap çıkarmak için kullanılan fonksiyon
export async function removeBookAction({ commit }, payload) {
    commit(actionTypes.LOADING_BOOKS, true);

    try {
        await deleteBook(payload);
        commit(actionTypes.REMOVE_BOOK, payload);
    } catch (e) {
        console.log(e);
    }

    commit(actionTypes.LOADING_BOOKS, false);
}

// Yeni bir kitap eklemek için kullanılan fonksiyon
export async function addBookAction({ commit }, payload) {
    //console.log("Book Add Function");
    //console.log(payload);
    var langs = {
        English: 0,
        Turkish: 1,
        Spanish: 2
    };
    switch (payload.language.id) {
        case 0:
            payload.language = langs.English;
            break;
        case 1:
            payload.language = langs.Turkish;
            break;
        case 2:
            payload.language = langs.Spanish;
            break;
        default:
            payload.language = langs.Turkish;
    }
    commit(actionTypes.LOADING_BOOKS, true);

    try {
        const { data } = await addBook(payload);
        payload.id = data;
        commit(actionTypes.ADD_BOOK, payload);
    } catch (e) {
        console.log(e);
    }

    commit(actionTypes.LOADING_BOOKS, false);
}