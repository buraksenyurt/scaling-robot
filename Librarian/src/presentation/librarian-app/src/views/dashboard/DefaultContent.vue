<template>
  <div>
    <div class="text-h2 my-4">Kitaplık Panosu</div>
    <div class="default-content">
      <div style="margin-right: 4rem; margin-bottom: 4rem">
        <BookListCard @handleShowBooks="handleShowBooks" />
      </div>
    </div>
  </div>
</template>

<script>
import { mapActions } from "vuex";
import BookListCard from "@/components/BookListCard";

export default {
  name: "DefaultContent",
  components: {
    BookListCard,
  },
  methods: {
    ...mapActions("bookModule", ["getBooksAction"]),
    handleShowBooks(show) {
      this.showBooks = show;
    },
  },
  data: () => ({
    showBooks: false,
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