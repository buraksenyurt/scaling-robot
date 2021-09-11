<template>
    <v-row justify="center">
        <v-dialog v-model="dialog" persistent max-width="600px">
            <template v-slot:activator="{ on, attrs }">
                <v-btn style="margin-top: 1rem"
                       rounded
                       color="light-blue"
                       dark
                       v-bind="attrs"
                       v-on="on">
                    <v-icon left>mdi-plus</v-icon>
                    Kütüphaneye Kitap Ekle
                </v-btn>
            </template>
            <v-card>
                <form @submit.prevent="
            addBookAction(body);
            bodyRequest = {};
          ">
                    <v-card-title>
                        <span class="headline">Kitap Bilgileri</span>
                    </v-card-title>
                    <v-card-text>
                        <v-container>
                            <v-row>
                                <v-col cols="12" sm="6">
                                    <v-text-field label="Adı"
                                                  v-model="body.title"
                                                  required></v-text-field>
                                </v-col>
                                <v-col cols="12" sm="6">
                                    <v-text-field label="Yayıncı"
                                                  v-model="body.publisher"
                                                  required></v-text-field>
                                </v-col>
                                <v-col cols="12">
                                    <v-textarea label="Yazarları"
                                                v-model="body.authors"
                                                required></v-textarea>
                                </v-col>

                                <v-col cols="12" sm="6">
                                    <v-slider v-model="body.row"
                                              color="green"
                                              label="Raf"
                                              min="1"
                                              max="5"
                                              thumb-label></v-slider>
                                </v-col>

                                <v-col cols="12" sm="6">
                                    <v-slider v-model="body.column"
                                              color="orange"
                                              label="Blok"
                                              min="1"
                                              max="5"
                                              thumb-label></v-slider>
                                </v-col>

                                <v-col cols="12" sm="6">
                                    <v-select v-model="body.language"
                                              :items="languages"
                                              item-text="name"
                                              item-value="id"
                                              label="Dili"
                                              persistent-hint
                                              return-object
                                              single-line>
                                    </v-select>
                                </v-col>
                            </v-row>
                        </v-container>
                    </v-card-text>
                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn color="blue darken-1" text @click="dialog = false">
                            Close
                        </v-btn>
                        <v-btn color="blue darken-1"
                               text
                               @click="dialog = false"
                               type="submit">
                            Save
                        </v-btn>
                    </v-card-actions>
                </form>
            </v-card>
        </v-dialog>
    </v-row>
</template>

<script>
    import { mapActions } from "vuex";
    export default {
        name: "AddBookForm",
        data: () => ({
            body: {
                title: "",
                authors: "",
                publisher: "",
                row: 0,
                column: 0,
                language: 1
            },
            dialog: false,
            languages: [
                { id: 0, name: "İngilizce" }
                , { id: 1, name: "Türkçe" }
                , { id: 2, name: "İspanyolca" }
            ]
        }),
        methods: {
            ...mapActions("bookModule", ["addBookAction"]),
        },
    };
</script>