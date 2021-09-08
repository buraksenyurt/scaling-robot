<template>
  <v-container>
    <div class="text-h4 mb-10">Kitap Listesi</div>
    <div class="v-picker--full-width d-flex justify-center" v-if="loading">
      <v-progress-circular
        :size="70"
        :width="7"
        color="purple"
        indeterminate
      ></v-progress-circular>
    </div>

    <v-simple-table>
      <template v-slot:default>
        <thead>
          <tr>
            <th>Title</th>
            <th>Authors</th>
            <th>Publisher</th>
            <th>Language</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="b in books" :key="b.id">
            <td>{{ b.title }}</td>
            <td>{{ b.athors }}</td>
            <td>{{ b.publisher }}</td>
            <td>{{ getLang(b.language) }}</td>
          </tr>
        </tbody>
      </template>
    </v-simple-table>
  </v-container>
</template>

<script>
import { getBooksAxios } from "@/api/book-service";
export default {
  name: "BookList",
  async mounted() {
    this.loading = true;
    await this.getBooksAxios();
    this.loading = false;
  },
  data() {
    return {
      books: [],
      loading: false,
    };
  },
  methods: {
    async getBooksAxios() {
      this.loading = true;
      try {
        const { data } = await getBooksAxios();
        //console.log(data);
        //console.log(this.books);
        this.books = data.bookList;
      } catch (e) {
        console.log(e);
        alert("Sanırım uçuş sistemleri çalışmıyor. Düşüyoruzzzz!!!");
      } finally {
        this.loading = false;
      }
    },

    getLang(language) {
      switch (language) {
        case 0:
          return "İngilizce";
        case 1:
          return "Türkçe";
        case 2:
          return "İspanyolca";
        default:
          return "Bilemedim";
      }
    },
  },
  
};
</script>