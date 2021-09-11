<template>
    <div>
        <div class="text-h2 my-4">Kitaplık Panosu</div>
        <div class="default-content">
            <div style="margin-right: 4rem; margin-bottom: 4rem">
                <BookListCard @handleShowBooks="handleShowBooks" />
                <AddBookForm />
            </div>
        </div>
        <div v-if="showBooks">
            <AddBookForm :bookId="bookId" />
        </div>
    </div>
</template>

<script>
    import { mapActions } from "vuex";
    import BookListCard from "@/components/BookListCard";
    import AddBookForm from "@/components/AddBookForm";

    export default {
        name: "DefaultContent",
        components: {
            BookListCard,
            AddBookForm
        },
        methods: {
            ...mapActions("bookModule", ["getBooksAction"]),
            handleShowBooks(show, id) {
                this.showBooks = show;
                this.bookId = id;
            },
        },
        data: () => ({
            showBooks: false,
            bookId: 0
        }),
        mounted() {
            this.getBooksAction();
            this.showBooks = false;
        },
    };
</script>


<style scoped>
    .default-content {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        justify-content: flex-start;
    }
</style>