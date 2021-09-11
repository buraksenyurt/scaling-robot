<template>

    <v-skeleton-loader v-if="loading"
                       width="500"
                       max-width="600"
                       height="100%"
                       type="card"></v-skeleton-loader>

    <v-card v-else width="500" max-width="600" height="100%">
        <v-toolbar color="pink" dark>
            <v-toolbar-title>Kitaplar</v-toolbar-title>
            <v-spacer></v-spacer>
        </v-toolbar>
        <v-list-item-group color="primary">
            <v-list-item v-for="book in books" :key="book.id">
                <v-list-item-content>
                    <v-list-item-title v-text="book.title"></v-list-item-title>
                    <v-list-item-subtitle v-text="book.authors"></v-list-item-subtitle>
                </v-list-item-content>
                <v-list-item-action>
                    <v-icon @click="removeBook(book.id)">
                        mdi-delete-outline
                    </v-icon>
                </v-list-item-action>
            </v-list-item>
        </v-list-item-group>
    </v-card>
</template>


<script>
    import { mapActions, mapGetters } from "vuex";
    export default {
        name: "BookListCard",
        computed: {
            ...mapGetters("bookModule", {
                books: "books",
                loading: "loading",
            }),
        },
        methods: {
            ...mapActions("bookModule", ["removeBookAction"]),
            removeBook(bookId) {
                const confirmed = confirm(
                    "Bu kitabı kitaplıktan çıkarmak istediğine emin misin?"
                );
                if (!confirmed) return;
                this.removeBookAction(bookId);
            },
        },
    };
</script>